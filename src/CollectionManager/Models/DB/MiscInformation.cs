using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.DB
{
    public class MiscInformation
    {
        #region Properties
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Value { get; set; }
        #endregion
    }
}
