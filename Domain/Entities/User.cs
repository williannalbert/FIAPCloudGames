using Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string ApplicationUserId { get; set; } 
    public bool IsActive { get; set; } = true;
    public Library Library { get; set; }
    public Wallet Wallet { get; set; }
}
