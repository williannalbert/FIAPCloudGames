using Application.DTOs.Wallet;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class WalletService(IMapper _mapper, IUnitOfWork _unitOfWork) : IWalletService
{
    public async Task<WalletDTO> AddMoneyAsync(Guid userId, decimal value)
    {
        try
        {
            var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);
            if (wallet is null)
                throw new Exception("Usuário não possui carteira cadastrada");

            var walletDTO = _mapper.Map<WalletDTO>(wallet);
            walletDTO.Balance += value;

            var walletUpdate = _mapper.Map<Wallet>(walletDTO);
            _unitOfWork.WalletRepository.Update(walletUpdate);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<WalletDTO>(walletUpdate);
        }
        catch (Exception e)
        {

            throw;
        }
    }

    public async Task<WalletDTO> CreateAsync(Guid userId)
    {
        try
        {
            var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);
            if (wallet is not null)
                throw new Exception("Usuário já possui carteira cadastrada");

            var newWallet = _mapper.Map<Wallet>(new CreateWalletDTO() { UserId = userId });

            var createdWallet = await _unitOfWork.WalletRepository.CreateAsync(newWallet);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<WalletDTO>(createdWallet);
        }
        catch (Exception e)
        {

            throw;
        }
        
    }

    public async Task<WalletDTO> DeductMoneyAsync(Guid userId, decimal value, bool commitChanges = false)
    {
        try
        {
            var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);
            if (wallet is null)
                throw new Exception("Usuário não possui carteira cadastrada");

            var walletDTO = _mapper.Map<WalletDTO>(wallet);

            if (walletDTO.Balance < 0)
                throw new Exception("Carteira com saldo negativo");

            if ((walletDTO.Balance - value) < 0)
                throw new Exception("Carteira não possui saldo suficiente para realizar operação");

            walletDTO.Balance -= value;

            var walletUpdate = _mapper.Map<Wallet>(walletDTO);
            _unitOfWork.WalletRepository.Update(walletUpdate);
            if(commitChanges)
                await _unitOfWork.CommitAsync();

            return _mapper.Map<WalletDTO>(walletUpdate);
        }
        catch (Exception e)
        {

            throw;
        }
    }

    public async Task<WalletDTO> GetByUserIdAsync(Guid userId)
    {
        try
        {
            var wallet = await _unitOfWork.WalletRepository.GetByUserIdAsync(userId);
            if (wallet is null)
                return null;

            return _mapper.Map<WalletDTO>(wallet);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
