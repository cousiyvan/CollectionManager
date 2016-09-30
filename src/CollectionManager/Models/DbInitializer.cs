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
            gameContext.Database.EnsureCreated();

            if (gameContext.GameDbMapping.Any())
                return;

            var gameDbMappings = new GameDbMapping[]
            {
                new GameDbMapping { Collection = true, Favorite = true, Id = 1, GameId = -1, UserId = 1, Wishlist = false }
            };
            foreach (GameDbMapping gameDb in gameDbMappings)
            {
                gameContext.GameDbMapping.Add(gameDb);
            }
            gameContext.SaveChanges();
        }
    }
}
