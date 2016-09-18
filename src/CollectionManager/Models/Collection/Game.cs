using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.Collection
{
    public enum Category
    {
        MainGame=0,
        DLC_Addon,
        Expansion,
        Bundle,
        Standalone_Expansion
    }

    public class Game: CollectionElementMaster
    {
        // igdb - API: https://market.mashape.com/igdbcom/internet-game-database

        #region Properties
        public List<string> Developers { get; set; }

        public Dictionary<string, Uri> PlayerPerspective { get; set; }

        public Dictionary<string, Uri> GameEngine { get; set; }

        public Dictionary<string, Uri> GameModes { get; set; }
        #endregion
    }
}
