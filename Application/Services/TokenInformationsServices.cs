using Application.DTOs.User;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class TokenInformationsServices : ITokenInformationsServices
{
    private readonly IHttpContextAccessor _contextAccessor;

    public TokenInformationsServices(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public UserClaimsDTO GetUserClaims()
    {
        var httpClaims = _contextAccessor.HttpContext?.User?.Claims;

        if (!Guid.TryParse(httpClaims.FirstOrDefault(c => c.Type == "userId")?.Value, out var guid))
            throw new Exception("Erro ao obter userId no token");

        var userClaims = new UserClaimsDTO()
        {
            UserId = guid,
            ApplicationUserId = httpClaims.FirstOrDefault(c => c.Type == "applicationUserId")?.Value ?? "",
            Name = httpClaims.FirstOrDefault(c => c.Type == "name")?.Value ?? "",
            Email = httpClaims.FirstOrDefault(c => c.Type == "email")?.Value ?? ""
        };
        return userClaims;
    }

    public UserClaimsDTO GetUserClaimsByToken(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");

            if (!Guid.TryParse(userIdClaim?.Value, out var guid))
                throw new Exception("Erro ao obter userId no token");

            var userClaims = new UserClaimsDTO()
            {
                UserId = guid,
                ApplicationUserId = jwtToken.Claims.FirstOrDefault(c => c.Type == "applicationUserId")?.Value ?? "",
                Name = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "",
                Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? ""
            };
            
            return userClaims;

        }
        throw new Exception("Token inválido");
    }

    public Guid GetUserId()
    {
        var userId =_contextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (Guid.TryParse(userId, out var guid))
            return guid;

        throw new Exception("Ero ao obter UserId");
    }

    public Guid GetUserIdByToken(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");

            if (Guid.TryParse(userIdClaim?.Value, out var guid))
                return guid;

            throw new Exception("Ocorreu um erro ao obter userId");
        }
        throw new Exception("Token inválido");
    }
}
