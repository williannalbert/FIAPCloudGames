using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Wallet;

public class MoneyWalletAdminDTO
{
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [Range(0, 10000, ErrorMessage = "Valor mínimo: 0 | Valor máximo: 10000")]
    public decimal Value { get; set; } = 0;
}
