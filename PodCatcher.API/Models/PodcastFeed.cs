using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public class PodcastFeed
    {
        
        public PodcastFeed()
        {
            
        }

        public Podcast Podcast { get; set; }
        public IEnumerable<Episode> Episodes { get; set; }

    }
}