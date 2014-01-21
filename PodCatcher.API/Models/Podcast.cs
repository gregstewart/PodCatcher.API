using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodCatcher.API.Models
{
    public class Podcast
    {
        public Guid Id { get; set; }
        public string Title { get; set; } // title
        public string Uri { get; set; } // atom:link @href
        public string Summary { get; set; } // itunes:summary
        public string Image { get; set; } // //itunes:image @href
    }
}