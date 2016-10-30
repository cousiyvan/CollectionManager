using CollectionManager.Models.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {

        }

        public DbSet<MovieDBMapping> MovieDbMapping { get; set; }
        public DbSet<MiscMovieInformation> MiscGameInformation { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieDBMapping>().ToTable("MovieDbMapping");
            modelBuilder.Entity<MiscMovieInformation>().ToTable("MiscMovieInformation");
        }
    }
}
