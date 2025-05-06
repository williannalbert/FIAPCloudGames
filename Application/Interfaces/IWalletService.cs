using Application.DTOs.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IWalletService
{
    Task<WalletDTO> CreateAsync(Guid userId);
    Task<WalletDTO> GetByUserIdAsync(Guid userId);
    Task<WalletDTO> AddMoneyAsync(Guid userId, decimal value);
    Task<WalletDTO> DeductMoneyAsync(Guid userId, decimal value, bool commitChanges = false);
}
