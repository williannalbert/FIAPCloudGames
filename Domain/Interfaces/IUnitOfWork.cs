using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    ILibraryRepository LibraryRepository { get; }
    IGameRepository GameRepository { get; }
    ISaleRepository SaleRepository{ get; }
    IPromotionRepository PromotionRepository { get; }
    IGamePromotionRepository GamePromotionRepository{ get; }
    IWalletRepository WalletRepository { get; }
    IUserRepository UserRepository { get; }
    Task CommitAsync();
}
