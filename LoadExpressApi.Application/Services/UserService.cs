using LoadExpressApi.Application.Abstraction.Repositories;
using LoadExpressApi.Application.Auditing;
using LoadExpressApi.Application.Common.Models;
using LoadExpressApi.Application.Common.Request;
using LoadExpressApi.Application.Common.Response;
using LoadExpressApi.Application.Interfaces;
using LoadExpressApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoadExpressApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryAsync<User> _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IRepositoryAsync<User> userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IResult> CreateUserAsync(UserRequest request)
        {
            try
            {
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    UserName = request.Email,
                    PhoneNumber = request.PhoneNumber
                };
                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();
                _logger.LogInformation("User created successfully: {Email}", request.Email);
                return await Result.SuccessAsync("User created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return await Result.FailAsync("Failed to create user");
            }
        }

        public async Task<IResult> UpdateUserAsync(string email, UserRequest request)
        {
            try
            {
                var user = await _userRepository.GetAsync(u => u.Email == email);
                if (user == null) return await Result.FailAsync("User not found");

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.PhoneNumber = request.PhoneNumber;

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
                return await Result.SuccessAsync("User updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return await Result.FailAsync("Failed to update user");
            }
        }

        public async Task<IResult<UserResponse>> GetUserAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetAsync(u => u.Email == email);
                if (user == null) return await Result<UserResponse>.FailAsync("User not found");

                var result = new UserResponse
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                return await Result<UserResponse>.SuccessAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user");
                return await Result<UserResponse>.FailAsync("Failed to fetch user");
            }
        }

        public async Task<IResult<List<UserResponse>>> GetUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetUnpaginatedListAsync();
                var response = users.Select(u => new UserResponse
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber
                }).ToList();

                return await Result<List<UserResponse>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return await Result<List<UserResponse>>.FailAsync("Failed to fetch users");
            }
        }

        public async Task<IResult> DeleteUserAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetAsync(u => u.Email == email);
                if (user == null) return await Result.FailAsync("User not found");

                user.IsDeleted = true;
                user.DeletedOn = DateTime.UtcNow;

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();
                return await Result.SuccessAsync("User deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return await Result.FailAsync("Failed to delete user");
            }
        }

        public Task AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(List<User> entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(Expression<Func<User, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUnpaginatedListAsync(Expression<Func<User, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRanges(List<User> entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exist(Expression<Func<User, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }

}
