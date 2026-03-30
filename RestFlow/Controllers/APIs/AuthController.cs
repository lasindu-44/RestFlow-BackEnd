using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestFlow.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var userExists = await _userManager.FindByNameAsync(model.email);
        var userByUsername = await _userManager.FindByNameAsync(model.username);
        if (userExists != null || userByUsername != null)
            return BadRequest(new { message = "User already exists" });

        var user = new ApplicationUser
        {
            UserName = model.username,
            Email = model.email,
            PhoneNumber = model.phone.ToString(),
            SecurityStamp = Guid.NewGuid().ToString()
        };

        user.FullName = model.firstName + " " + model.lastName;
        user.IsRestaurantOwner = model.isRestaurantOwner;
        var result = await _userManager.CreateAsync(user, model.password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (model.isRestaurantOwner == true)
        {

            await _userManager.AddToRoleAsync(user, "SystemAdmin");
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "Customer");

        }

        return Ok(new { message = "User created successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null)
            return Unauthorized(new { message = "Invalid username or password" });

        var validPassword = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!validPassword)
            return Unauthorized(new { message = "Invalid username or password" });

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = GetToken(authClaims);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }



    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin(GoogleLoginDto model)
    {
        GoogleJsonWebSignature.Payload payload;
      

        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { _configuration["GoogleAuth:ClientId"] },
                IssuedAtClockTolerance = TimeSpan.FromMinutes(5),
                ExpirationTimeClockTolerance = TimeSpan.FromMinutes(5)
            };

            //var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);
            

            payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);
        }
        catch(Exception ex)
        {
            return Unauthorized(new { message = "Invalid Google token" });
        }

        var email = payload.Email;
        var name = payload.Name;

        // Check if user exists
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                IsRestaurantOwner = false
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Customer");
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = GetToken(authClaims);

        return Ok(new
        {
            message = "Login Sucessfully",
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var tokenValidityInMinutes = Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]);

        return new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}