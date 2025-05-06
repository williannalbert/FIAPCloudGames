using Application.DTOs.Game;
using Application.DTOs.Promotion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs.GamePromotion;

public class GamePromotionDTO
{
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public Guid GameId { get; set; }
    [JsonIgnore]
    public GameDTO? Game { get; set; }
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public Guid PromotionId { get; set; }
    [JsonIgnore]
    public PromotionDTO? Promotion { get; set; }
}
