using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IGameRepository : IRepository<Game>
{
    Task<IEnumerable<Game>> GetAllCompleteAsync();
    Task<Game> GetCompleteAsync(Guid id);
}
