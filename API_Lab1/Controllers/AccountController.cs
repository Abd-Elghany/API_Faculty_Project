using API_Lab1.DTOs;
using API_Lab1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<ApplicationUser> _userManager, IConfiguration Config)
        {
            userManager = _userManager;
            config = Config;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO userfromrequest)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = userfromrequest.UserName;
                user.Email = userfromrequest.Email;
                IdentityResult result = await userManager.CreateAsync(user, userfromrequest.Password);
                if (result.Succeeded)
                {
                    return Ok("Created");
                }
                else
                {
                    // return BadRequest(result.Errors);
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("password", item.Description);

                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO userfromrequest)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userFromDb = await userManager.FindByNameAsync(userfromrequest.UserName);
                if (userFromDb != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userFromDb, userfromrequest.Password);
                    if (found)
                    {
                        // claims to be added in Token 

                        List<Claim> userClaims = new List<Claim>();

                        userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userFromDb.Id));
                        userClaims.Add(new Claim(ClaimTypes.Name, userFromDb.UserName));

                        // Generated Id (JWT Predefined claims)
                        userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        // user Roles
                        var userRules = await userManager.GetRolesAsync(userFromDb);
                        foreach (var rule in userRules) 
                        {
                            userClaims.Add(new Claim(ClaimTypes.Role,rule));
                        }



                        // preparing signingCredentials
                        SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecertKey"]));
                        SigningCredentials userCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);



                        // Design the Token

                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer : config["JWT:IssuerIP"],
                            audience : config["JWT:AudienceIP"],
                            expires : DateTime.Now.AddHours(1),
                            claims : userClaims,
                            signingCredentials : userCredentials
                        );

                                        // generate the Token response
                        return Ok (new  
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(myToken),
                            Expiration = DateTime.Now.AddHours(1)  // or  myToken.ValidTo
                        });
                    }
                }
                ModelState.AddModelError("Username", "Username or Passowrd is invalid");
            }
            return BadRequest(ModelState);
        }





    }
}
