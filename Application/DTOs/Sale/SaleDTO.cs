using Application.DTOs.Game;
using Application.DTOs.Library;
using Application.DTOs.Promotion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Sale;

public class SaleDTO
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public Guid LibraryId { get; set; }
    public LibraryDTO Library { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public Guid GameId { get; set; }
    public GameDTO Game { get; set; }
    public Guid? PromotionId { get; set; }
    public PromotionDTO? Promotion { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public decimal FinalPrice { get; set; }
    public DateTime SaleDate { get; set; }
}
