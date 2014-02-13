using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace PodCatcher.API.Models.Podcasts
{
    public class Podcast
    {

        public Podcast()
        {

        }

        public Guid Id { get; set; }
        public string Title { get; set; } // title
        public string Uri { get; set; } // atom:link @href
        public string Summary { get; set; } // itunes:summary
        public string Image { get; set; } //itunes:image @href

    }
}