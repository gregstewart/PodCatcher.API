using System;
using System.ComponentModel.DataAnnotations;

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

        public MetaData Metadata { get; set; }
    }
}