using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.WebPages;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Episodes;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Controllers
{
    public class PodcastsController : ApiController
    {
        private static IPodcastRepository podcastTableRepository;
        private static IEpisodeRepository episodeTableRepository;
        private static IPodcastBuilder podcastBuilder;

        public PodcastsController()
        {
            podcastTableRepository = PodcastTableRepositoryFactory.Create();
            episodeTableRepository = EpisodeTableRepositoryFactory.Create();
            podcastBuilder = PodcastBuilderFactory.Create();
        }

        public IEnumerable<Podcast> GetAll()
        {
            Uri entryPointUri = GetEntryPointUri();
            IEnumerable<Podcast> podcasts = podcastTableRepository.GetAll();

            foreach (var podcast in podcasts)
            {
                string path = podcast.Id.ToString();
                MetaData metaData = new MetaData(entryPointUri, path);
                podcast.Metadata = metaData;
                yield return podcast;
            }
            
        }

        public IHttpActionResult Get(Guid id)
        {
            Podcast podcast = podcastTableRepository.Get(id);
            Uri entryPointUri = GetEntryPointUri();
            string path = "episodes";

            if (podcast == null)
            {
                return NotFound();
            }

            MetaData metaData = new MetaData(entryPointUri, path);
            podcast.Metadata = metaData;
            return Ok(podcast);
        }

        public IHttpActionResult Post(Podcast podcast)
        {
            if (podcast != null && !podcast.Uri.IsEmpty())
            {
                try 
                {
                    PodcastFeed builtPodcastFeed = podcastBuilder.Build(podcast);
                    Podcast builtPodcast = builtPodcastFeed.Podcast;
                    builtPodcast = podcastTableRepository.Add(builtPodcast);
                    episodeTableRepository.Add(builtPodcast, builtPodcastFeed.Episodes);
                    return CreatedAtRoute("DefaultApi", new { builtPodcast.Id }, builtPodcastFeed);
                }
                catch(Exception exception)
                {
                    return StatusCode(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }        
        }

        public StatusCodeResult Put(Podcast podcast)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        private Uri GetEntryPointUri()
        {
            return new Uri(Url.Request.RequestUri.ToString());
        }

        public StatusCodeResult Delete(Podcast podcast)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
