using Application.DTOs.Game;
using Application.DTOs.GamePromotion;
using Application.DTOs.Promotion;
using Application.Exceptions;
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

public class PromotionService(IMapper _mapper, IUnitOfWork _unitOfWork, IGameService _gameService) : IPromotionService
{
    public async Task<GameDTO> AddGamePromotionAsync(GamePromotionDTO gamePromotionDTO)
    {
        try
        {
            var game = await _gameService.GetAsync(gamePromotionDTO.GameId);
            if (game is null)
                throw new NotFoundException($"Jogo não foi localizado. Id: {gamePromotionDTO.GameId}");

            var promotion = await GetValidPromotionAsync(gamePromotionDTO.PromotionId);
            if (promotion is null)
                throw new NotFoundException($"Promoção não foi localizada. Id: {gamePromotionDTO.PromotionId}");

            var gamepromotion = await GetGamePromotionAsync(gamePromotionDTO);
            if (gamepromotion is not null)
                throw new BusinessException("Jogo já está com a promoção cadastrada");

            var gamePromotion = _mapper.Map<GamePromotion>(gamePromotionDTO);

            await _unitOfWork.GamePromotionRepository.CreateAsync(gamePromotion);
            await _unitOfWork.CommitAsync();

            var gameDiscount = await _gameService.GetAsync(gamePromotionDTO.GameId);
            return _mapper.Map<GameDTO>(gameDiscount);
        }
        catch (Exception e)
        {
            throw;
        }
    }
    public async Task<bool> DeleteGamePromotionAsync(GamePromotionDTO gamePromotionDTO)
    {
        try
        {
            var game = await _gameService.GetAsync(gamePromotionDTO.GameId);
            if (game is null)
                throw new NotFoundException($"Jogo não foi localizado. Id: {gamePromotionDTO.GameId}");

            var promotion = await _unitOfWork.PromotionRepository.GetAsync(p => p.Id == gamePromotionDTO.PromotionId);
            if (promotion is null)
                throw new NotFoundException($"Promoção não foi localizada. Id: {gamePromotionDTO.PromotionId}");

            var gamepromotion = await _unitOfWork.GamePromotionRepository.GetAsync(g => g.PromotionId == gamePromotionDTO.PromotionId && g.GameId == gamePromotionDTO.GameId);
            if(gamepromotion is not null)
            {
                _unitOfWork.GamePromotionRepository.Delete(gamepromotion);
                await _unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }
        catch (Exception )
        {

            throw;
        }
    }


    public async Task<PromotionDTO> CreateAsync(PromotionDTO promotionDTO)
    {
        try
        {
            var promotion = _mapper.Map<Promotion>(promotionDTO);

            var newPromotion = await _unitOfWork.PromotionRepository.CreateAsync(promotion);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<PromotionDTO>(newPromotion);
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
            var promotion = await _unitOfWork.PromotionRepository.GetAsync(p => p.Id == id);
            if (promotion is null)
                return false;

            _unitOfWork.PromotionRepository.Delete(promotion);
            await _unitOfWork.CommitAsync();

            return true;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<IEnumerable<PromotionDTO>> GetAllAsync()
    {
        try
        {
            var promotions = await _unitOfWork.PromotionRepository.GetAllAsync();
            if(promotions is null)
                return new List<PromotionDTO>();

            return _mapper.Map<IEnumerable<PromotionDTO>>(promotions);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<PromotionDTO> GetAsync(Guid id)
    {
        try
        {
            var promotion = await _unitOfWork.PromotionRepository.GetAsync(p => p.Id==id);
            if (promotion is null)
                return null;

            return _mapper.Map<PromotionDTO>(promotion);

        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<PromotionDTO> UpdateAsync(Guid id, PromotionDTO promotionDTO)
    {
        try
        {
            var promotion = await _unitOfWork.PromotionRepository.GetAsync(p => p.Id == promotionDTO.Id);
            if(promotion is null)
                throw new NotFoundException("Promotion não localizada");

            _mapper.Map(promotionDTO, promotion);
            _unitOfWork.PromotionRepository.Update(promotion);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<PromotionDTO>(promotion);
        }
        catch (Exception e)
        {
            throw;
        }    
    }

    public async Task<PromotionDTO> GetValidPromotionAsync(Guid promotionId)
    {
        try
        {
            var promotion = await _unitOfWork.PromotionRepository.GetAsync(p =>
                p.Id == promotionId
                && p.Enable
                && p.InitialDate < DateTime.Now
                && p.FinalDate > DateTime.Now);

            if (promotion is null)
                return null;

            return _mapper.Map<PromotionDTO>(promotion);
        }
        catch (Exception e)
        {

            throw;
        }
    }

    public async Task<GamePromotionDTO> GetGamePromotionAsync(GamePromotionDTO gamePromotionDTO)
    {
        try
        {
            var gamepromotion = await _unitOfWork.GamePromotionRepository.GetAsync(g => 
                g.PromotionId == gamePromotionDTO.PromotionId && g.GameId == gamePromotionDTO.GameId);

            if (gamepromotion is null)
                return null;

            return _mapper.Map<GamePromotionDTO>(gamepromotion);
        }
        catch (Exception e)
        {

            throw;
        }
    }
}
