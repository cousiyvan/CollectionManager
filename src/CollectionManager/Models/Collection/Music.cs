using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.Collection
{
    public class Music: CollectionElementMaster
    {
        // http://www.allmusic.com/
        #region Properties
        public TimeSpan Duration { get; set; }

        public List<string> Styles { get; set; }

        public string Band { get; set; }

        public Dictionary<string, string> Artists { get; set; }

        public Dictionary<string, string> Songs { get; set; }
        #endregion
    }
}
