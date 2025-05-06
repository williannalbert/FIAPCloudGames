using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class GamePromotion : BaseEntity
{
    public Guid GameId { get; set; }
    public Game Game { get; set; }

    public Guid PromotionId { get; set; }
    public Promotion Promotion { get; set; }
}
