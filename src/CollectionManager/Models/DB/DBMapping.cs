﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.DB
{
    public class DBMapping
    {
        #region Properties
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool Favorite { get; set; }
        public bool Wishlist { get; set; }
        public bool Collection { get; set; }
        #endregion
    }
}
