using EducationSystem.Entity.DTO.User;
using EducationSystem.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EducationSystem.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;


        public AccountController(UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;

        }


        // api/account/register

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (registerDto.Roles is null)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                foreach (var role in registerDto.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }


            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Account Created Sucessfully!"
            });

        }



        //api/account/login
        /*  [AllowAnonymous]
          [HttpPost("login")]

          public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
          {
              if (!ModelState.IsValid)
              {
                  return BadRequest(ModelState);
              }

              var user = await _userManager.FindByEmailAsync(loginDto.Email);

              if (user is null)
              {
                  return Unauthorized(new AuthResponseDto
                  {
                      IsSuccess = false,
                      Message = "User not found with this email",
                  });
              }

              var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

              if (!result)
              {
                  return Unauthorized(new AuthResponseDto
                  {
                      IsSuccess = false,
                      Message = "Invalid Password."
                  });
              }


              var token = GenerateToken(user);

              return Ok(new AuthResponseDto
              {
                  Token = token,
                  IsSuccess = true,
                  Message = "Login Success."
              });


          }

          */


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found with this email",
                });
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid Password."
                });
            }

            // Generate JWT access token
            var token = GenerateToken(user);

            // Generate refresh token and store it in the database
            var refreshToken = GenerateRefreshToken();
            // await SaveRefreshToken(user, refreshToken);

            return Ok(new AuthResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                IsSuccess = true,
                Message = "Login Success."
            });
        }


        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);  // Can be stored in database
        }

        private string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII
            .GetBytes(_configuration.GetSection("JWTSetting").GetSection("securityKey").Value!);

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.Name, user.FullName ?? ""),
                new(JwtRegisteredClaimNames.NameId, user.Id ?? ""),
                new(JwtRegisteredClaimNames.Aud,
                _configuration.GetSection("JWTSetting").GetSection("validAudience").Value!),
                new(JwtRegisteredClaimNames.Iss, _configuration.GetSection("JWTSetting").GetSection("validIssuer").Value!)
            ];


            foreach (var role in roles)

            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }

        //api/account/detail
        [HttpGet("detail")]
        public async Task<ActionResult<UserDetailDto>> GetUserDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(currentUserId!);


            if (user is null)
            {
                return NotFound(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found"
                });
            }

            return Ok(new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = [.. await _userManager.GetRolesAsync(user)],
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount,

            });

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Ensure page and pageSize are valid values
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            // Fetch users from the database
            var usersQuery = _userManager.Users
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var users = await usersQuery.ToListAsync();

            // Fetch roles asynchronously for each user
            var userDetails = new List<UserDetailDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDetails.Add(new UserDetailDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Roles = roles.ToArray()
                });
            }

            return Ok(userDetails);
        }

        /*  [HttpGet]
          public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
          {
              // Ensure page and pageSize are valid values to prevent errors
              page = page < 1 ? 1 : page;
              pageSize = pageSize < 1 ? 10 : pageSize;

              var usersQuery = _userManager.Users
                  .OrderBy(u => u.Id) // Ensure consistent ordering
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .Select(u => new UserDetailDto
                  {
                      Id = u.Id,
                      Email = u.Email,
                      FullName = u.FullName,
                      Roles = _userManager.GetRolesAsync(u).Result.ToArray()
                  });

              var users = await usersQuery.ToListAsync();

              return Ok(users);
          }
          */
        /* [HttpGet]
         public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
         {
             var users = await _userManager.Users.Select(u => new UserDetailDto
             {
                 Id = u.Id,
                 Email = u.Email,
                 FullName = u.FullName,
                 Roles = _userManager.GetRolesAsync(u).Result.ToArray()
             }).ToListAsync();


             var query = _context.Tasks
                    .Include(t => t.CreatedBy)
                    .Include(t => t.AssignedTo)
                    .AsNoTracking()
                    .OrderBy(t => t.DueDate);
             var tasks = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(t => new TaskDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        DueDate = t.DueDate,
                        IsCompleted = t.IsCompleted,
                        CreatedByName = t.CreatedBy.FullName,
                        AssignedToName = t.AssignedTo.FullName
                    })
                    .ToListAsync();



             return Ok(users);
         }
     */
    }
}
