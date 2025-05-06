using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Promotion;

public class PromotionDTO
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public DateTime InitialDate { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public DateTime FinalDate { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [Range(1, 100, ErrorMessage = "Porcentagem de desconto deve ser entre 1 e 100")]
    public decimal Percentage { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public bool Enable { get; set; }
}
