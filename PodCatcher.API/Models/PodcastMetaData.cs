using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public class PodcastMetaData : MetaData
    {
        private Podcast podcast;
        
        public PodcastMetaData(Podcast podcast, Uri entryPointUri)
        {
            this.podcast = podcast;
            this.entryPointUri = entryPointUri;
        }

        public PodcastMetaData(Uri entryPointUri, String path)
        {
            this.entryPointUri = entryPointUri;
            this.path = path;
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

    }
}