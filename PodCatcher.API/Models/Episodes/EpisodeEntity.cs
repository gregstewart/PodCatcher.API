using System;
using Microsoft.WindowsAzure.Storage.Table;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Models.Episodes
{
    public class EpisodeEntity : TableEntity
    {
        public EpisodeEntity(Podcast podcast, Episode episode)
        {
            this.PartitionKey = podcast.Title;
            this.RowKey = episode.Id.ToString();
            Id = episode.Id;
            Title = episode.Title;
            Link = episode.Link;
            Comments = episode.Comments;
            PublicationDate = episode.PublicationDate;
            PermaLink = episode.PermaLink;
            Description = episode.Description;
            Subtitle = episode.Subtitle;
            Summary = episode.Summary;
            Author = episode.Author;
            Explicit = episode.Explicit;
            Duration = episode.Duration;
            MediaLink = episode.MediaLink;
            MediaDuration = episode.MediaDuration;
            MediaType = episode.MediaType;
        }

        public EpisodeEntity()
        {

        }

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