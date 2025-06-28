using LoadExpressApi.Application.Abstraction.Repositories;
using LoadExpressApi.Application.Common.Models;
using LoadExpressApi.Application.Common.Request;
using LoadExpressApi.Application.Common.Response;
using LoadExpressApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadExpressApi.Application.Interfaces
{
    public interface IUserService : IRepositoryAsync<User>
    {
        Task<IResult> CreateUserAsync(UserRequest request);
        Task<IResult> UpdateUserAsync(string id, UserRequest request);
        Task<IResult<UserResponse>> GetUserAsync(string id);
        Task<IResult<List<UserResponse>>> GetUsersAsync();
        Task<IResult> DeleteUserAsync(string id);
    }

}
