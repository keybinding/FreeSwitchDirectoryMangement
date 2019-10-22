using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTTT.Model;

namespace TTTT.Persistent
{
    public class DirectoryDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DirectoryDbContext(DbContextOptions<DirectoryDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity
                .HasIndex(u => u.Number)
                .IsUnique();
            });
        }
    }
}
