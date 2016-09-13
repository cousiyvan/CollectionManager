using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.Collection
{
    public class Game: CollectionElementMaster
    {
        // igdb

        #region Properties
        public List<string> Developers { get; set; }

        public Dictionary<string, Uri> PlayerPerspective { get; set; }

        public Dictionary<string, Uri> GameEngine { get; set; }

        public Dictionary<string, Uri> GameModes { get; set; }
        #endregion
    }
}
