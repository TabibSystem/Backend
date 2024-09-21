using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TabibApp.Application.Dtos;
using TabibApp.Application.Interfaces;
using TabibApp.Infrastructure.DependancyInjection;

namespace TabibApp.Application.Implementation;

public class TokenService:ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public TokenService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings)
    {
        this._userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    public RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var generator = new RNGCryptoServiceProvider();
        generator.GetBytes(randomNumber);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            ExpiresOn = DateTime.UtcNow.AddDays(10),
            CreatedOn = DateTime.UtcNow
        };
    }
    public async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier,user.Id)
          
            
        }.Union(userClaims).Union(roleClaims);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            signingCredentials: signingCredentials
        );
        return jwtSecurityToken;



    }
    public async Task<bool> RevokeTokenAsync(string token)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

        if (user == null)
            return false;
        var refreshToken = user.RefreshTokens.Single(t => t.Token==token);
        if(!refreshToken.IsActive)
        {
            return false;
        }
        refreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        
        return true;


    }

    public async Task<AuthResultDto> RefreshTokenAsync(string token)
    {
        var authResultDto = new AuthResultDto();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
        if (user is null)
        {
            authResultDto.Message = "Invalid Token";
            return authResultDto;
        }

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
        if (!refreshToken.IsActive)
        {
            authResultDto.Message = "Inactive Token";
            return authResultDto;
        }

        refreshToken.RevokedOn = DateTime.UtcNow;
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens.Add(newRefreshToken);
        await _userManager.UpdateAsync(user);

        var jwtToken = await GenerateJwtToken(user);
            
        authResultDto.IsSucceeded = true;
        authResultDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        authResultDto.FirstName= user.FirstName;
        authResultDto.LastName= user.LastName;
        authResultDto.Email = user.Email;
        authResultDto.Roles = await _userManager.GetRolesAsync(user) as List<string>;

        return authResultDto;
    }
}