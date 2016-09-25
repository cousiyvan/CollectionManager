using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.Collection
{
    public class Game: CollectionElementMaster
    {
        // igdb - API: https://market.mashape.com/igdbcom/internet-game-database
        public enum Category
        {
            MainGame = 0,
            DLC_Addon,
            Expansion,
            Bundle,
            Standalone_Expansion
        }
        public enum CallType
        {
            Genres=0,
            Companie,
            Franchise,
            Theme,
            GamePerspective,
            Platform,
            Keyword,
            Serie,
            GameModes
        }

        #region Properties
        public List<string> Developers { get; set; }

        public Dictionary<string, Uri> PlayerPerspective { get; set; }

        public Dictionary<string, Uri> GameEngine { get; set; }

        public List<string> GameModes { get; set; }
        #endregion

        #region constructors
        public Game():base()
        {
            this.Developers = new List<string>();
            this.PlayerPerspective = new Dictionary<string, Uri>();
            this.GameEngine = new Dictionary<string, Uri>();
            this.GameModes = new List<string>();
        }
        #endregion
    }
}
