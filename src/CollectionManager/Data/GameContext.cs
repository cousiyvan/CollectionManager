using CollectionManager.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Data
{
    public class GameContext: DbContext
    {
        public GameContext(DbContextOptions<GameContext> options):base(options)
        {

        }

        public DbSet<GameDbMapping> GameDbMapping { get; set; }
        public DbSet<MiscGameInformation> MiscGameInformation { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameDbMapping>().ToTable("GameDbMapping");
            modelBuilder.Entity<MiscGameInformation>().ToTable("MiscGameInformation");
        }
    }
}
