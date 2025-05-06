using Application.DTOs.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User;

public class UserDTO
{
    public string Name { get; set; }
    public string ApplicationUserId { get; set; }
    public LibraryDTO Library { get; set; }
}
