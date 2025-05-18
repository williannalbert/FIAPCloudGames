using Application.BusinessRules.Interfaces;
using Application.DTOs.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.BusinessRules;

public class UserRules : IUserRules
{
    public bool ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        bool hasMinimumLength = password.Length >= 8;
        bool hasDigit = Regex.IsMatch(password, @"\d");
        bool hasLetter = Regex.IsMatch(password, @"[a-zA-Z]");
        bool hasSpecialChar = Regex.IsMatch(password, @"[\W_]");

        return hasMinimumLength && hasDigit && hasLetter && hasSpecialChar;
    }
}
