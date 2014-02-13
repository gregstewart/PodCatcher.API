using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models
{
    public class PodcastEntity : TableEntity
    {
        public PodcastEntity(Podcast podcast)
        {
            this.PartitionKey = podcast.Title;
            this.RowKey = podcast.Id.ToString();
            Id = podcast.Id;
            Title = podcast.Title;
            Uri = podcast.Uri;
            Summary = podcast.Summary;
            Image = podcast.Image;
        }

        public PodcastEntity()
        {
            
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; } // atom:link @href
        public string Summary { get; set; } // itunes:summary
        public string Image { get; set; } //itunes:image @href
        
    }
}