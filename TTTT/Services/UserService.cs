using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTTT.Model;
using TTTT.Persistent.Repositories;

namespace TTTT.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> ListAsync();
        Task<EntityEntry<User>> AddAsync(User a_user);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository a_userRepository)
        {
            this.userRepository = a_userRepository;
        }

        public async Task<EntityEntry<User>> AddAsync(User a_user)
        {
            return await userRepository.AddAsync(a_user);
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await userRepository.ListAsync();
        }
    }
}
