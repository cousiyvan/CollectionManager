using CollectionManager.Models.Collection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Utils
{
    interface Mapper
    {
        CollectionElementMaster Mapping(JsonReader json);
    }
}
