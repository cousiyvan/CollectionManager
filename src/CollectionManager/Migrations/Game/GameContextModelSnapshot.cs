using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CollectionManager.Data;

namespace CollectionManager.Migrations.Game
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CollectionManager.Models.DB.GameDbMapping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Collection");

                    b.Property<bool>("Favorite");

                    b.Property<int>("GameId");

                    b.Property<string>("UserId");

                    b.Property<bool>("Wishlist");

                    b.HasKey("Id");

                    b.ToTable("GameDbMapping");
                });
        }
    }
}
