using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Library : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
