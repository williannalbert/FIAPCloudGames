using Application.DTOs.Library;
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

public class LibraryService : ILibraryService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWalletService _walletService;
    public LibraryService(IMapper mapper, IUnitOfWork unitOfWork, IWalletService walletService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _walletService = walletService;
    }

    public async Task<LibraryDTO> CreateAsync(CreateLibraryDTO createLibraryDTO)
    {
        try
        {
            var libraryUser = await _unitOfWork.LibraryRepository.GetLibraryByUserIdAsync(createLibraryDTO.UserId);

            if (libraryUser is not null)
                throw new Exception("Usuário já possui Library cadastrada");

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
                throw new Exception("Library possui Games cadastrados. Não é possível realizar a exclusão");

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
                throw new Exception("Library não localizada");

            var gameExist = library.Sales.Where(x => x.GameId == addGameLibraryDTO.GameId).FirstOrDefault();


            if (gameExist is null)
            {
                var game = await _unitOfWork.GameRepository.GetCompleteAsync(addGameLibraryDTO.GameId);

                var promotion = game.GamePromotions == null ? null : await _unitOfWork.PromotionRepository.GetAsync(p => p.Id == addGameLibraryDTO.PromotionId);

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
