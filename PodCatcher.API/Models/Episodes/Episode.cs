using System;

namespace PodCatcher.API.Models
{
    public class Episode
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Comments { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PermaLink { get; set; }
        public string Description { get; set; }
        public string Subtitle { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public bool Explicit { get; set; }
        public string Duration { get; set; }
        public string MediaLink { get; set; }
        public int MediaDuration { get; set; }
        public string MediaType { get; set; }
    }
}