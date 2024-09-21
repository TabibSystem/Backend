using System.IdentityModel.Tokens.Jwt;
using TabibApp.Application.Dtos;

namespace TabibApp.Application.Interfaces;

public interface ITokenService
{
    Task<AuthResultDto> RefreshTokenAsync(string token);
    public Task<bool> RevokeTokenAsync(string email);
    public Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user);
    public  RefreshToken GenerateRefreshToken();
}
