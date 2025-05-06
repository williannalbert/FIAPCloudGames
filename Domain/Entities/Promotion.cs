using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Promotion : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
    public decimal Percentage { get; set; }
    public bool Enable { get; set; }
    public ICollection<GamePromotion> GamePromotions { get; set; } = new List<GamePromotion>();
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
