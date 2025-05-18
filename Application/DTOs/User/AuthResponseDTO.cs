using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User;

public class AuthResponseDTO
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
