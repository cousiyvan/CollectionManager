using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollectionManager.Models.Collection
{
    public abstract class CollectionElementMaster
    {
        #region Properties
        public int Id { get; set; }

        public List<Uri> Websites { get; set; }

        public Dictionary<string, DateTime> ReleaseDate { get; set; }

        public List<string> Publishers { get; set; }

        public List<Uri> Videos { get; set; }

        public List<Uri> Pictures { get; set; }

        public List<string> Recommandations { get; set; }

        public List<string> Similar { get; set; }

        public Uri Poster { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string OriginalTitle { get; set; }

        public List<string> Keywords { get; set; }

        public Dictionary<string, Uri> Series { get; set; }

        public Dictionary<string, Uri> Quicklinks { get; set; }

        public List<string> Genres { get; set; }

        public Dictionary<string, Uri> Themes { get; set; }

        public Dictionary<string, Uri> OnlineStores { get; set; }

        public List<string> Formats { get; set; }

        ///TODO
        /// public List<Review> Reviews { get; set; }
        /// public List<Rating> Ratings { get; set; }
        #endregion
    }
}
