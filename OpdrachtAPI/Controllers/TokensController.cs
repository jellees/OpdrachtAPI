using DeviceDetectorNET.Parser.Device;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpdrachtAPI.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OpdrachtAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokensController : ControllerBase
{
    private readonly IConfiguration _config;

    public TokensController(IConfiguration config)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult<TokenResultDto> GetToken(string name, string password)
    {
        var result = new TokenResultDto();
        result.Valid = false;

        if (password != "secret")
            return NotFound(result);

        result.Valid = true;
        result.Token = GenerateToken(name);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("valid/{token}")]
    public async Task<ActionResult<TokenResultDto>> CheckTokenValidity(string token)
    {
        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _config["Jwt:Issuer"],
            ValidAudience = _config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
        };

        TokenValidationResult validation = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, tokenValidationParameters);

        TokenResultDto result = new TokenResultDto();
        result.Valid = validation.IsValid;

        if (result.Valid)
        {
            result.Token = token;
        }
        return Ok(result);
    }

    private string GenerateToken(string name)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        Claim[] claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, name)
        };
        JwtSecurityToken token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
