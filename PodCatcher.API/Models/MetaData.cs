using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public class MetaData
    {
        private Uri entryPointUri;
        private Podcast podcast;
        private string path = "";

        public MetaData(Podcast podcast, Uri entryPointUri)
        {
            this.podcast = podcast;
            this.entryPointUri = entryPointUri;
        }

        public MetaData(Uri entryPointUri, String path)
        {
            this.podcast = podcast;
            this.entryPointUri = entryPointUri;
            this.path = path;
        }

        public Uri Link
        {
            get
            {
                var constructedUri = FixTrailingSlash(this.entryPointUri.ToString());
                constructedUri += this.path;
                return new Uri(constructedUri);
            }
        }

        public Uri SubscribeLink
        {
            get
            {
                var constructedUri = FixTrailingSlash(this.entryPointUri.ToString());
                constructedUri += this.path + "/subscribe";
                return new Uri(constructedUri);
            }
        }

        private string FixTrailingSlash(string Uri)
        {
            if (!Uri.EndsWith("/"))
            {
                return Uri + "/";
            }

            return Uri;
        }
    }
}