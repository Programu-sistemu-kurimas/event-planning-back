using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Event_planning_back.Core.Abstractions;
using Event_planning_back.Core.Models;


namespace Event_planning_back.Core.Security;


public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }


    public string GenerateToken(User user)
    {
        Claim[] claims = [new("userId", user.Id.ToString()), new ("userEmail", user.Email), new ("userName", user.UserSurname)];

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_options.ExpireHours)
            );
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }

    public Guid GetUserId(string token)
    {  
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userIdString = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (Guid.TryParse(userIdString, out Guid userId))
        {
            return userId;
        }
        return Guid.Empty;

    }
    
    public string GetUserName(string token)
    {  
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
        
        return userName ?? string.Empty;
    }
    
}