using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Validation;

public class PasswordAttribute : ValidationAttribute
{
    public PasswordAttribute()
    {
        ErrorMessage = "A senha deve ter no mínimo 8 caracteres, incluir letra, número e caractere especial.";
    }

    public override bool IsValid(object value)
    {
        var password = value as string;

        if (string.IsNullOrWhiteSpace(password))
            return false;

        bool hasMinimumLength = password.Length >= 8;
        bool hasDigit = Regex.IsMatch(password, @"\d");
        bool hasLetter = Regex.IsMatch(password, @"[a-zA-Z]");
        bool hasSpecialChar = Regex.IsMatch(password, @"[\W_]");

        return hasMinimumLength && hasDigit && hasLetter && hasSpecialChar;
    }
}
