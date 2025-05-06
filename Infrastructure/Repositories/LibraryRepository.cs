using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class LibraryRepository : Repository<Library>, ILibraryRepository
{
    public LibraryRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Library?> GetLibraryByUserIdAsync(Guid userId)
    {

        return await _context.Libraries
            .Include(l => l.Sales)
                .ThenInclude(s => s.Game)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.UserId == userId);
    }

    public async Task<Library?> GetLibraryWithGamesAsync(Guid id)
    {
        return await _context.Libraries
            .Include(l => l.Sales)
                .ThenInclude(s => s.Game)
                .FirstOrDefaultAsync(l => l.Id == id);
    }
}
