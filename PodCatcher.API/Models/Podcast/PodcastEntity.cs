using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace PodCatcher.API.Models
{
    public class PodcastEntity : TableEntity
    {
        public PodcastEntity(string title, Guid id)
        {
            this.PartitionKey = title;
            this.RowKey = id.ToString();
            Id = id;
            Title = title;
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