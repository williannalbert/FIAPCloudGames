using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Sale : BaseEntity
{
    public Guid LibraryId { get; set; }
    public Library Library { get; set; }

    public Guid GameId { get; set; }
    public Game Game { get; set; }

    public Guid? PromotionId { get; set; }
    public Promotion? Promotion { get; set; }
    public decimal FinalPrice { get; set; }
    public DateTime SaleDate { get; set; } = DateTime.Now;
}
