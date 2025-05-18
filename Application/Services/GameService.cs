using Application.DTOs.Game;
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

public class GameService(IMapper _mapper, IUnitOfWork _unitOfWork) : IGameService
{
    public async Task<GameDTO> CreateAsync(CreateGameDTO createGameDTO)
    {
        try
        {
            var gameExists = await _unitOfWork.GameRepository.GetAsync(g => g.Name.ToLower().Trim() == createGameDTO.Name.ToLower().Trim());
            if (gameExists is not null)
                throw new BusinessException($"Já consta game cadastrado com esse nome: {createGameDTO.Name}");

            var game = _mapper.Map<Game>(createGameDTO);

            var newGame = await _unitOfWork.GameRepository.CreateAsync(game);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<GameDTO>(newGame);
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
            var game = await _unitOfWork.GameRepository.GetAsync(g => g.Id == id);
            if (game is null)
                return false;
            
            _unitOfWork.GameRepository.Delete(game);
            await _unitOfWork.CommitAsync();

            return true;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<GameDTO> GetAsync(Guid id)
    {
        try
        {
            var game = await _unitOfWork.GameRepository.GetCompleteAsync(id);
            if (game is null)
                return null;

            return _mapper.Map<GameDTO>(game); 
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<IEnumerable<GameDTO>> GetAllAsync()
    {
        var listGames = await _unitOfWork.GameRepository.GetAllCompleteAsync();
        if(listGames is null)
            return new List<GameDTO>();

        return _mapper.Map<IEnumerable<GameDTO>>(listGames);
    }

    public async Task<GameDTO> UpdateAsync(Guid id, UpdateGameDTO updateGameDTO)
    {
        try
        {
            var game = await _unitOfWork.GameRepository.GetAsync(g => g.Id == updateGameDTO.Id);
            if (game is null)
                throw new NotFoundException("Game não localizado");

            var gameNameExists = await _unitOfWork.GameRepository.GetAsync(g => 
                g.Name.ToLower().Trim() == updateGameDTO.Name.ToLower().Trim()
                && g.Id != updateGameDTO.Id
            );
            if (gameNameExists is not null)
                throw new BusinessException($"Já consta game cadastrado com esse nome: {updateGameDTO.Name}");

            _mapper.Map(updateGameDTO, game);
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<GameDTO>(game);
        }
        catch (Exception e)
        {
            throw;
        }
        
    }

    public async Task<GameDTO> GetByNameAsync(string name)
    {
        var game = await _unitOfWork
            .GameRepository
            .GetAsync(g => 
                g.Name.Trim().ToLower().Equals(name.ToLower().Trim()));

        if (game is null)
            return null;

        return _mapper.Map<GameDTO>(game);
    }
}
