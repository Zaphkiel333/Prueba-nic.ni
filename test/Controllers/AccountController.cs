using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Monografia.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Monografia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(
            SignInManager<ApplicationUser> signInManager, 
            IConfiguration configuration, 
            UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new {result = "User not created"});
                }

                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = signUpModel.FirstName,
                    LastName = signUpModel.LastName,
                    Email = signUpModel.Email,
                    UserName = signUpModel.Email
                };

                var result = await userManager.CreateAsync(user,signUpModel.Password);
                if (result.Succeeded)
                {
                    return Ok(new { result ="User created succesfully"});
                }else
                {
                    return BadRequest(new { result = "User not created" });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] SignInModel signInModel)
        {
            try
            {
                string token = await GenerateToken(signInModel);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized();
                }

                return Ok(new { token = token });

            }catch (Exception ex)
            {
                return BadRequest($"Failed to login {ex.Message}");
            }
        }

        private async Task<string> GenerateToken(SignInModel signInModel)
        {
            var result = await signInManager.PasswordSignInAsync(
                signInModel.Email,
                signInModel.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if(!result.Succeeded)
            {
                return null;
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey authSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"]));

            JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
