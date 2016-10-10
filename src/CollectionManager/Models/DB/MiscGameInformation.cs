using CollectionManager.Models.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.DB
{
    public class MiscGameInformation
    {
        #region Properties
        public int Id { get; set; }
        public Game.CallType Type { get; set; }
        public int ExternalId { get; set; }
        public string Value { get; set; }
        #endregion
    }
}
