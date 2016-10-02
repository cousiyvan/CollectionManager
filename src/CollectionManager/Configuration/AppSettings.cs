using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Configuration
{
    public class AppSettings
    {
        public ServicesSettings ServicesSettings { get; set; }
        public ApiKey ApiKey { get; set; }
    }
}
