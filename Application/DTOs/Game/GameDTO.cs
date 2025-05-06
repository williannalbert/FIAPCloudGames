using Application.DTOs.GamePromotion;
using Application.DTOs.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.Game;

public class GameDTO
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public decimal Price { get; set; }
    [JsonIgnore]
    public ICollection<GamePromotionDTO> GamePromotions { get; set; } 
}
