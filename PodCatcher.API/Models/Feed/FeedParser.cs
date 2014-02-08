using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace PodCatcher.API.Models
{
    public class FeedParser : IFeedParser
    {
        public Podcast Parse(Podcast podcast, string xml)
        {
            XElement root = XElement.Parse(xml);
            XNamespace itunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";

            podcast.Title = GetFirstElement(root, "title");

            podcast.Summary = GetFirstElement(root, "description");

            podcast.Image = GetNamespaceFirstAttribute(root, itunes, "image", "href");

            podcast.Episodes = GetAllEpisodes(root);

            return podcast;
        }

        private IEnumerable<Episode> GetAllEpisodes(XElement root)
        {
            XNamespace itunes = "http://www.itunes.com/dtds/podcast-1.0.dtd";
            List<Episode> episodes = new List<Episode>();
            IEnumerable<XElement> elements = from el in root.Descendants("item")
                select el;

            var xElements = elements as XElement[] ?? elements.ToArray();
            if (xElements.Any())
            {
                episodes.AddRange(xElements.Select(xElement => new Episode
                {
                    Title = GetFirstElement(xElement, "title"), 
                    Author = GetNamespaceFirstElement(xElement, itunes, "author"), 
                    Comments = GetFirstElement(xElement, "comments"), 
                    Description = GetFirstElement(xElement, "description"), 
                    Duration = GetNamespaceFirstElement(xElement, itunes, "duration"),
                    Explicit = (GetNamespaceFirstElement(xElement, itunes, "explicit") != "no"), 
                    Id = Guid.NewGuid(), 
                    Link = GetFirstElement(xElement, "link"), 
                    PermaLink = GetFirstElement(xElement, "guid"),
                    PublicationDate = ParseDate(GetFirstElement(xElement, "pubDate")), 
                    Subtitle = GetNamespaceFirstElement(xElement, itunes, "subtitle"), 
                    Summary = GetNamespaceFirstElement(xElement, itunes, "summary"),
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

        private static string GetNamespaceFirstAttribute(XElement root, XNamespace itunes, string element, string attribute)
        {
            return (string)
                (from el in root.Descendants(itunes + element)
                 select el.Attribute(attribute)).FirstOrDefault();
        }


        private static string GetFirstElement(XElement root, string element)
        {
            return (string)
                (from el in root.Descendants(element)
                 select el).FirstOrDefault();
        }

        private static string GetNamespaceFirstElement(XElement root, XNamespace itunes, string element)
        {
            return (string)
                (from el in root.Descendants(itunes + element)
                 select el).FirstOrDefault();
        }

        private static DateTime ParseDate(string date)
        {
            try
            {
                return Convert.ToDateTime(date);
            }
            catch (Exception exception)
            {
                return NormalizeDate(date);  
            
            }
        }

        private static DateTime NormalizeDate(string date)
        {
            try
            {
                Dictionary<string, string> timeZones = new Dictionary<string, string>();
                timeZones.Add("EST", "-05:00");
                timeZones.Add("CST", "-06:00");
                timeZones.Add("MST", "-07:00");
                timeZones.Add("PST", "-08:00");

                string inputDate = date;
                string modifiedInputDate = inputDate.Substring(0, inputDate.LastIndexOf(" "));
                string timeZoneIdentifier = inputDate.Substring(inputDate.LastIndexOf(" ") + 1);
                string timeZoneOffset = timeZones[timeZoneIdentifier];
                string dateForParsing = modifiedInputDate + " " + timeZoneOffset;

                return DateTime.ParseExact(dateForParsing, "ddd, dd MMM yyyy HH:mm:ss zzz", System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                throw new DataException();
            }
        }  
    }
}