using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User;

public class ChangeEmailUserAdminDTO
{
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string ApplicationUserId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato do campo incorreto.")]
    public string NewEmail { get; set; }
}
