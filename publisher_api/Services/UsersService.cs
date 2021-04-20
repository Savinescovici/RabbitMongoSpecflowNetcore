using MongoDB.Driver;
using publisher_api.Dto;
using publisher_api.Mongo;
using publisher_api.Mongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace publisher_api.Services
{
    public interface IUsersService
    {
        Task<User> AddUser(UserDto userDto);
        Task<List<User>> GetUsers(CancellationToken token = default);
        Task<User> GetUser(Guid userId, CancellationToken token = default);
        Task Update(Guid userId, UserDto userDto);
        Task Delete(Guid userId);
    }

    public class UsersService : IUsersService
    {
        private IMongoRepository<User> _repository;
        public UsersService(IMongoService mongoService)
        {
            _repository = mongoService.GetRepository<User>();
        }

        public async Task<User> AddUser(UserDto userDto)
        {
            var user = new User
            {
                UserId = userDto.UserId,
                Email = userDto.Email,
                UserName = userDto.Name,
            };
            return await _repository.CreateAsync(user);
        }

        public async Task<List<User>> GetUsers(CancellationToken token = default)
        {
            var filter = Builders<User>.Filter.Empty;
            var sort = Builders<User>.Sort.Ascending(u => u.UserName);
            var users = await _repository.GetListAsync(filter, sort, token);
            return users;
        }

        public async Task<User> GetUser(Guid userId, CancellationToken token = default)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, userId);
            var user = await _repository.FindAsync(filter, token);
            return user;
        }

        public async Task Update(Guid userId, UserDto userDto)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, userId);
            var update = Builders<User>.Update
                .Set(x => x.Email, userDto.Email)
                .Set(x => x.UserName, userDto.Name);
            await _repository.UpdateAsync(filter, update);
        }

        public async Task Delete(Guid userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, userId);
            await _repository.DeleteAsync(filter);
        }
    }
}
