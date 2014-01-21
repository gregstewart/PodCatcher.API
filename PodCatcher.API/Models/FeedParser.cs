using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace PodCatcher.API.Models
{
    public class FeedParser
    {
        public Podcast Parse(Podcast podcast, string xml)
        {
            XElement root = XElement.Parse(xml);
            XNamespace itunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";

            podcast.Title = (string)
                (from el in root.Descendants("title")
                select el).First();

            podcast.Summary = (string)
                (from el in root.Descendants("description")
                 select el).First();
            
            podcast.Image = (string)
                (from el in root.Descendants(itunes + "image")
                 select el.Attribute("href")).First();
            return podcast;
        }
    }
}