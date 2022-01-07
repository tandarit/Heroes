using HeroesAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HeroesAPI.Data
{
    public class HeroesContext : DbContext
    {
        public HeroesContext(DbContextOptions<HeroesContext> options)
            : base(options)
        {
        }
        public DbSet<HeroValue> HeroValue { get; set; }
    }
}
