using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTTT.Model;
using TTTT.Persistent;

namespace TTTT.Persistent.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> ListAsync();
        Task<EntityEntry<User>> AddAsync(User a_user);
    }
    public class UserRepository : BaseRepository, IUserRepository
    {

        public UserRepository(DirectoryDbContext a_dbContext) : base(a_dbContext)
        {
        }

        public async Task<EntityEntry<User>> AddAsync(User a_user)
        {
            var v = await dbContext.Users.AddAsync(a_user);
            await dbContext.SaveChangesAsync();
            return v;
        }

        public async Task<IEnumerable<User>> ListAsync()
        {
            return await dbContext.Users.ToListAsync();
        }
    }
}
