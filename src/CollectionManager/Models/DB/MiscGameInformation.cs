﻿using CollectionManager.Models.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.DB
{
    public class MiscGameInformation:MiscInformation
    {
        #region Properties
        public Game.CallType Type { get; set; }
        #endregion
    }
}
