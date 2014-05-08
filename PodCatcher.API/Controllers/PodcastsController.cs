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
        private static ILogger logger; 

        public PodcastsController()
        {
            podcastTableRepository = PodcastTableRepositoryFactory.Create();
            episodeTableRepository = EpisodeTableRepositoryFactory.Create();
            logger = LoggerFactory.Create();
            podcastBuilder = PodcastBuilderFactory.Create();
        }

        public IEnumerable<Podcast> GetAll()
        {

            logger.Debug("Get all podcasts");
            Uri entryPointUri = GetEntryPointUri();
            IEnumerable<Podcast> podcasts = podcastTableRepository.GetAll();

            foreach (var podcast in podcasts)
            {
                string path = podcast.Id.ToString();
                PodcastMetaData podcastMetaData = new PodcastMetaData(entryPointUri, path);
                podcast.Metadata = podcastMetaData;
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

            PodcastMetaData podcastMetaData = new PodcastMetaData(entryPointUri, path);
            podcast.Metadata = podcastMetaData;
            return Ok(podcast);
        }

        public IHttpActionResult Post(Podcast podcast)
        {
            logger.Debug("Debugging message for creating a new podcast");
            if (podcast != null && !podcast.Uri.IsEmpty())
            {
                try 
                {
                    PodcastFeed builtPodcastFeed = podcastBuilder.Build(podcast);
                    Podcast builtPodcast = builtPodcastFeed.Podcast;
                    builtPodcast = podcastTableRepository.Add(builtPodcast);
                    episodeTableRepository.Add(builtPodcast, builtPodcastFeed.Episodes);
                    logger.Info("Successfully created podcast entry for: " + builtPodcast.Title);
                    return CreatedAtRoute("DefaultApi", new { builtPodcast.Id }, builtPodcastFeed);
                }
                catch(Exception exception)
                {
                    logger.Error("Bad Request: " + exception.Message);
                    logger.Error(exception.InnerException.ToString());
                    logger.Error(exception.StackTrace);
                    return StatusCode(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                logger.Error("Bad Request no URi provided");
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

        [Route("api/podcasts/{podcastId}/subscribe", Name = "SubscribeByPodcastId")]
        [HttpPost]
        public StatusCodeResult SubscribeByPodcastId(Guid podcastId)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        [Route("api/podcasts/sync", Name = "SyncPodcasts")]
        [HttpGet]
        public IHttpActionResult SyncPodcasts()
        {
            SyncPodcasts syncPodcasts = new SyncPodcasts();
            syncPodcasts.Sync();

            return StatusCode(HttpStatusCode.OK);
        }

    }
}
