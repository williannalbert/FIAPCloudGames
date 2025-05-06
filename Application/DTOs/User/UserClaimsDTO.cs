using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User;

public class UserClaimsDTO
{
    public Guid UserId { get; set; }
    public string ApplicationUserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
