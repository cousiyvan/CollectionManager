using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CollectionManager.Data;

namespace CollectionManager.Migrations.Game
{
    [DbContext(typeof(GameContext))]
    [Migration("20161011174601_CollectionManager")]
    partial class CollectionManager
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("CollectionManager.Models.DB.MiscGameInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ExternalId");

                    b.Property<int>("Type");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("MiscGameInformation");
                });
        }
    }
}
