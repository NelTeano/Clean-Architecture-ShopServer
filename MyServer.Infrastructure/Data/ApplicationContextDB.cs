using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyServer.Core.Entities;
namespace MyServer.Infrastructure.Data
{
    public class ApplicationContextDB(DbContextOptions<ApplicationContextDB> options) : DbContext(options)
    {
        public DbSet<UserEntity> User { get; set; }

    }
}
