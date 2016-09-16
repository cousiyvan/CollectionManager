using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.Collection
{
    public class Movie: CollectionElementMaster
    {
        // tmdb: https://www.themoviedb.org/documentation/api
        #region Properties
        public TimeSpan Runtime { get; set; }

        public string Network { get; set; }

        public int Seasons { get; set; }

        public Dictionary<int, string> Episodes { get; set; }

        public Dictionary<string, string> Team { get; set; }

        public Dictionary<string, string> Actors { get; set; }

        public int Budget { get; set; }

        public int Benefits { get; set; }
        #endregion
    }
}
