using Application.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User;

public class ResetPasswordUserDto
{
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato do campo incorreto.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Token { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [Password]
    public string NewPassword { get; set; }
}
