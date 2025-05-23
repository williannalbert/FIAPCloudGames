﻿using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class UnitOfWork(AppDbContext _context) : IUnitOfWork
{
    public ILibraryRepository _libraryRepository;
    public IGameRepository _gameRepository;
    public ISaleRepository _saleRepository;
    public IPromotionRepository _promotionRepository;
    public IGamePromotionRepository _gamePromotionRepository;
    public IWalletRepository _walletRepository;
    public IUserRepository _userRepository;
    
    public ILibraryRepository LibraryRepository
    {
        get
        {
            return _libraryRepository = _libraryRepository ?? new LibraryRepository(_context);
        }
    }
    public IGameRepository GameRepository
    {
        get
        {
            return _gameRepository = _gameRepository ?? new GameRepository(_context);
        }
    }
    public ISaleRepository SaleRepository
    {
        get
        {
            return _saleRepository = _saleRepository ?? new SaleRepository(_context);
        }
    }

    public IPromotionRepository PromotionRepository
    {
        get
        {
            return _promotionRepository = _promotionRepository ?? new PromotionRepository(_context);
        }
    }

    public IGamePromotionRepository GamePromotionRepository
    {
        get
        {
            return _gamePromotionRepository = _gamePromotionRepository ?? new GamePromotionRepository(_context);
        }
    }
    
    public IWalletRepository WalletRepository
    {
        get
        {
            return _walletRepository = _walletRepository ?? new WalletRepository(_context);
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            return _userRepository = _userRepository ?? new UserRepository(_context);
        }
    }
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
}
