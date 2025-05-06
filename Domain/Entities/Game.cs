using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Game : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public ICollection<GamePromotion> GamePromotions { get; set; } = new List<GamePromotion>();
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
