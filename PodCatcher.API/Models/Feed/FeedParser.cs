using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace PodCatcher.API.Models
{
    public class FeedParser : IFeedParser
    {
        private static XNamespace _itunes;
        public PodcastFeed Parse(PodcastFeed podcastFeed, string xml)
        {
            XElement root = XElement.Parse(xml);
            _itunes = GetItunesNamespace(xml);
            
            podcastFeed.Podcast.Title = GetFirstElement(root, "title");

            podcastFeed.Podcast.Summary = GetFirstElement(root, "description");

            podcastFeed.Podcast.Image = GetPodcastImage(root);

            podcastFeed.Episodes = GetAllEpisodes(root);

            return podcastFeed;
        }

        private XNamespace GetItunesNamespace(string xml)
        {
            XDocument document = XDocument.Parse(xml);
            var result = document.Root.Attributes().
                    Where(attribute => attribute.IsNamespaceDeclaration).
                    GroupBy(attribute => attribute.Name.Namespace == XNamespace.None ? String.Empty : attribute.Name.LocalName,
                            attribute => XNamespace.Get(attribute.Value)).
                    ToDictionary(g => g.Key,
                                 g => g.First());

            return result["itunes"];
        }

        private string GetPodcastImage(XElement root)
        {
            var image = root.Descendants(_itunes + "image").FirstOrDefault();
            if (image != null)
            {
                return image.Attribute("href").Value;
            }
            else
            {
                image = root.Descendants("image").FirstOrDefault();
                return (string) image.Descendants("url").FirstOrDefault();
            }

        }

        private IEnumerable<Episode> GetAllEpisodes(XElement root)
        {
            List<Episode> episodes = new List<Episode>();
            IEnumerable<XElement> elements = from el in root.Descendants("item")
                select el;

            var xElements = elements as XElement[] ?? elements.ToArray();
            if (xElements.Any())
            {
                episodes.AddRange(xElements.Select(xElement => new Episode
                {
                    Title = GetFirstElement(xElement, "title"), 
                    Author = GetNamespaceFirstElement(xElement, "author"), 
                    Comments = GetFirstElement(xElement, "comments"), 
                    Description = GetFirstElement(xElement, "description"), 
                    Duration =  GetNamespaceFirstElement(xElement, "duration"),
                    Explicit = (GetNamespaceFirstElement(xElement, "explicit") != "no"), 
                    Id = Guid.NewGuid(), 
                    Link = GetFirstElement(xElement, "link"), 
                    PermaLink = GetFirstElement(xElement, "guid"),
                    PublicationDate = DateParser.ParseDate(GetFirstElement(xElement, "pubDate")), 
                    Subtitle = GetNamespaceFirstElement(xElement, "subtitle"), 
                    Summary = GetNamespaceFirstElement(xElement, "summary"),
                    MediaLink = GetFirstAttribute(xElement, "enclosure", "url"),
                    MediaDuration = Convert.ToInt32(GetFirstAttribute(xElement, "enclosure", "length")),
                    MediaType = GetFirstAttribute(xElement, "enclosure", "type")
                }));
            }

            return episodes;
        }

        private static string GetFirstAttribute(XElement root, string element, string attribute)
        {
            return (string)
                (from el in root.Descendants(element)
                    select el.Attribute(attribute)).FirstOrDefault();
        }

        private static string GetFirstElement(XElement root, string element)
        {
            return (string)
                (from el in root.Descendants(element)
                 select el).FirstOrDefault();
        }

        private static string GetNamespaceFirstElement(XElement root, string element)
        {
            return (string)
                (from el in root.Descendants(_itunes + element) 
                 select el).FirstOrDefault();
        }
    }
}