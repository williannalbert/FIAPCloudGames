using Application.DTOs.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameDTO>> GetAllAsync();
    //Task<PageList<GameDTO>> GetAllPaged(ProprietarioParameters proprietarioParameters);
    Task<GameDTO> GetAsync(Guid id);
    Task<GameDTO> GetByNameAsync(string name);
    Task<GameDTO> CreateAsync(CreateGameDTO createGameDTO);
    Task<GameDTO> UpdateAsync(Guid id, UpdateGameDTO updateGameDTO);
    Task<bool> DeleteAsync(Guid id);
}
