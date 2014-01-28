﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models;

namespace PodCatcher.API.Tests.Stubs
{
    public class FeedParserStub : IFeedParser
    {
        public Podcast Parse(Podcast podcast, string xml)
        {
            podcast.Title = "Test podcast";
            podcast.Summary = "Test Summary";
            podcast.Image = "http://some.url/image.png";

            List<Episode> episodes = new List<Episode>();

            Episode episode = new Episode
            {
                Title = "Test episode Title",
                Author = "Test author",
                Comments = "Test comment",
                Description = "Test episode description",
                Duration = "1:34:45",
                Explicit = false,
                Id = Guid.NewGuid(),
                Link = "http://some.url/test/link",
                PermaLink = "some test guid",
                PublicationDate = Convert.ToDateTime("1/12/2014 17:13:23"),
                Subtitle = "Some test subtitle",
                Summary = "Some test summary",
                MediaLink = "http://some.url/file.mp3",
                MediaDuration = Convert.ToInt32("12345678"),
                MediaType = "type/media"
            };

            episodes.Add(episode);

            podcast.Episodes = episodes;

            return podcast;
        }
    }
}
