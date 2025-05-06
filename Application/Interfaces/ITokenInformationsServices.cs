using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface ITokenInformationsServices
{
    Guid GetUserId();
    Guid GetUserIdByToken(string token);
    UserClaimsDTO GetUserClaims();
    UserClaimsDTO GetUserClaimsByToken(string token);
}
