using CollectionManager.Data;
using CollectionManager.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models
{
    public static class DbInitializer
    {
        public static void Initialize(GameContext gameContext)
        {
            // gameContext.Database.EnsureDeleted();
            gameContext.Database.EnsureCreated();

            // if (gameContext.GameDbMapping.Any())
            //    gameContext.RemoveRange(gameContext.GameDbMapping.Select(x => x));

            var gameDbMappings = new GameDbMapping[]
            {
                new GameDbMapping { Collection = true, Favorite = true, GameId = 7346, UserId = "f09840b0-ef68-484e-9f94-45fce866bf7a", Wishlist = false },
                new GameDbMapping { Collection = true, Favorite = true, GameId = 7346, UserId = "73364d2e-acdb-44b9-9251-e29ea921a9fc", Wishlist = false }
            };
            foreach (GameDbMapping gameDb in gameDbMappings)
            {
                gameContext.GameDbMapping.Add(gameDb);
            }

            var miscGameInformations = new MiscGameInformation[]
            {
                new MiscGameInformation { ExternalId = 1, Type = Collection.Game.CallType.Companie, Value = "test" }
            };
            foreach (MiscGameInformation miscInfo in miscGameInformations)
            {
                gameContext.MiscGameInformation.Add(miscInfo);
            }
            gameContext.SaveChanges();
        }
    }
}
