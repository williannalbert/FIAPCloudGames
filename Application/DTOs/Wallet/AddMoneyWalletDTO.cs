using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Wallet;

public class AddMoneyWalletDTO
{
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public decimal Value { get; set; } = 0;
}
