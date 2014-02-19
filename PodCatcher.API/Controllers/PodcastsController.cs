using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
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
            IEnumerable<Podcast> podcasts = podcastTableRepository.GetAll();
            return podcasts;
        }

        public IHttpActionResult Get(Guid id)
        {
            Podcast item = podcastTableRepository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
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

        [Route("podcasts/{podcastId}/episodes")]
        public IHttpActionResult GetEpisodesByPodCast(Guid podcastId)
        {
            IEnumerable<Episode> episodes = episodeTableRepository.GetAll(podcastId);

            return Ok(episodes);
        }
    }
}
