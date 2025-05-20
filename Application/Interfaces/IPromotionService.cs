using Application.DTOs.Game;
using Application.DTOs.GamePromotion;
using Application.DTOs.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IPromotionService
{
    Task<IEnumerable<PromotionDTO>> GetAllAsync();
    Task<PromotionDTO> GetAsync(Guid id);
    Task<PromotionDTO> GetValidPromotionAsync(Guid id);
    Task<GamePromotionDTO> GetGamePromotionAsync(GamePromotionDTO gamePromotionDTO);
    Task<PromotionDTO> CreateAsync(CreatePromotionDTO createPromotionDTO);
    Task<PromotionDTO> UpdateAsync(Guid id, PromotionDTO promotionDTO);
    Task<bool> DeleteAsync(Guid id);
    Task<GameDTO> AddGamePromotionAsync(GamePromotionDTO gamePromotionDTO);
    Task<bool> DeleteGamePromotionAsync(GamePromotionDTO gamePromotionDTO);
}
