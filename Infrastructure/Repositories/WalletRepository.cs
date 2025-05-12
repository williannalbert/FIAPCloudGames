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

public class WalletRepository(AppDbContext _context) : Repository<Wallet>(_context), IWalletRepository
{
    public async Task<Wallet> GetByUserIdAsync(Guid userId)
    {
        try
        {
            return await _context.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.UserId == userId);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
