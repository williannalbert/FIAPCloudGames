using Application.DTOs.GamePromotion;
using Application.DTOs.Library;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class LibraryService(IMapper _mapper, IUnitOfWork _unitOfWork, IWalletService _walletService, IGameService _gameService, IPromotionService _promotionService) : ILibraryService
{
    public async Task<LibraryDTO> CreateAsync(CreateLibraryDTO createLibraryDTO)
    {
        try
        {
            var libraryUser = await _unitOfWork.LibraryRepository.GetLibraryByUserIdAsync(createLibraryDTO.UserId);

            if (libraryUser is not null)
                throw new BusinessException("Usuário já possui Library cadastrada");

            var library = _mapper.Map<Library>(createLibraryDTO);

            var newLibrary = await _unitOfWork.LibraryRepository.CreateAsync(library);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<LibraryDTO>(newLibrary);
        }
        catch (Exception e)
        {

            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var library = await _unitOfWork.LibraryRepository.GetLibraryByUserIdAsync(id);
                if (library == null)
                    return false;

            if (library.Sales.Count > 0)
                throw new BusinessException("Library possui Games cadastrados. Não é possível realizar a exclusão");

            _unitOfWork.LibraryRepository.Delete(library);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            throw;
        }
        
    }

    public async Task<LibraryDTO> GetLibraryByUserIdAsync(Guid userId)
    {
        try
        {
            var library = await _unitOfWork.LibraryRepository.GetLibraryByUserIdAsync(userId);
            if (library == null)
                return null;

            return _mapper.Map<LibraryDTO>(library);
        }
        catch (Exception e)
        {
            throw;
        }        
    }

    public async Task<LibraryDTO> AddGamesLibraryAsync(Guid userId, AddGameLibraryDTO addGameLibraryDTO)
    {
        try
        {
            var library = await _unitOfWork.LibraryRepository.GetAsync(l => l.User.Id == userId, l => l.Sales);
            if (library is null)
                throw new BusinessException("Usuário não possui biblioteca cadastrada");

            var gameLibraryExist = library.Sales.Where(x => x.GameId == addGameLibraryDTO.GameId).FirstOrDefault();


            if (gameLibraryExist is null)
            {
                var game = await _gameService.GetAsync(addGameLibraryDTO.GameId);
                if (game is null)
                    throw new NotFoundException("Jogo não localizado");

                var promotionValid = await _promotionService.GetValidPromotionAsync(addGameLibraryDTO.PromotionId.GetValueOrDefault());
                var promotionGame = await _promotionService.GetGamePromotionAsync(new GamePromotionDTO() { GameId = addGameLibraryDTO.GameId, PromotionId = addGameLibraryDTO.PromotionId.GetValueOrDefault() });

                Promotion promotion = null;
                if (promotionGame is not null && promotionGame is not null)
                    promotion = await _unitOfWork.PromotionRepository.GetAsync(p => p.Id == addGameLibraryDTO.PromotionId);


                if (game is not null)
                {
                    decimal price = promotion == null || game.Price == 0 ? game.Price : game.Price - (game.Price * (promotion.Percentage / 100));

                    await _walletService.DeductMoneyAsync(userId, price);

                    var newSale = new Sale
                    {
                        LibraryId = library.Id,
                        GameId = game.Id,
                        Promotion = promotion,
                        FinalPrice = price
                    };

                    await _unitOfWork.SaleRepository.CreateAsync(newSale);
                }
            }

            await _unitOfWork.CommitAsync();

            var updatedLibrary = await _unitOfWork.LibraryRepository.GetLibraryWithGamesAsync(library.Id);

            var libraryDTO = _mapper.Map<LibraryDTO>(updatedLibrary);

            return libraryDTO;
        }
        catch (Exception e)
        {

            throw;
        }
    }
}
