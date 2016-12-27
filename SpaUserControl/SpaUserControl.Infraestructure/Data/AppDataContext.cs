using SpaUserControl.Domain.Models;
using System.Data.Entity;

namespace SpaUserControl.Infraestructure.Data
{
    public class AppDataContext : DbContext
    {

        public AppDataContext():base("AppConnectionString")
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
