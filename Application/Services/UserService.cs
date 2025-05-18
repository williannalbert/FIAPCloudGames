using Application.DTOs.User;
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

public class UserService(IMapper _mapper, IUnitOfWork _unitOfWork) : IUserService
{
    public async Task<bool> DeleteAsync(Guid userId)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
            if (user is null)
                throw new NotFoundException("Usuário não localizado");

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        try
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync(u => u.Library, u => u.Wallet);
            if (users is null)
                return new List<UserDTO>();

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<UserDTO> GetAsync(Guid userId)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId, u => u.Library, u => u.Wallet);
            if (user is null)
                return null;

            return _mapper.Map<UserDTO>(user);
        }
        catch (Exception e)
        {
            throw;
        }    
    }

    public async Task<UserDTO> UpdateAsync(UpdateUserDTO updateUserDTO)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == updateUserDTO.Id, u => u.Library, u => u.Wallet);
            if (user is null)
                throw new NotFoundException("Usuário não localizado");

            _mapper.Map(updateUserDTO, user);
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<UserDTO>(user);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
