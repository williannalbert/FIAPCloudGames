using Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BusinessRules.Interfaces;

public interface IUserRules
{
    bool ValidatePassword(string password);
}
