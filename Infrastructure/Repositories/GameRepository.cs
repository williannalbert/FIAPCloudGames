using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class GameRepository : Repository<Game>, IGameRepository
{
    public GameRepository(AppDbContext context) : base(context)
    {
        
    }

    public async Task<IEnumerable<Game>> GetAllCompleteAsync()
    {
        try
        {
            var datenow = DateTime.Now;

            var games = await _context.Games
                .Include(g => g.GamePromotions
                    .Where(gp => gp.Promotion.Enable &&
                                 gp.Promotion.InitialDate <= datenow &&
                                 gp.Promotion.FinalDate >= datenow))
                .ThenInclude(gp => gp.Promotion)
                .AsNoTracking()
                .ToListAsync();

            return games;
        }
        catch (Exception e)
        {

            throw;
        }
    }

    public async Task<Game> GetCompleteAsync(Guid id)
    {
        try
        {
            var datenow = DateTime.Now;

            var game = await _context.Games
                .Where(g => g.Id == id)
                .Include(g => g.GamePromotions
                    .Where(gp => gp.Promotion.Enable &&
                                 gp.Promotion.InitialDate <= datenow &&
                                 gp.Promotion.FinalDate >= datenow))
                .ThenInclude(gp => gp.Promotion)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return game;
        }
        catch (Exception e)
        {

            throw;
        }
    }
}
