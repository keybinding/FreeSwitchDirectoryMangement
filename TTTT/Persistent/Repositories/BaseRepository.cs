using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTTT.Model;
using TTTT.Persistent;

namespace TTTT.Persistent.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly DirectoryDbContext dbContext;

        public BaseRepository(DirectoryDbContext a_dbContext)
        {
            dbContext = a_dbContext;
        }
    }
}
