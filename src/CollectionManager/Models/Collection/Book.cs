using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.Collection
{
    public class Book: CollectionElementMaster
    {
        // goodreads
        #region Properties
        public List<string> Authors { get; set; }

        public string ISBN { get; set; }

        public List<string> OtherEditions { get; set; }

        public string EditionLanguage { get; set; }

        #endregion
    }
}
