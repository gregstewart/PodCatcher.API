using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Episodes;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Controllers
{
    public class EpisodesController : ApiController
    {

        private static IEpisodeRepository episodeTableRepository;
        private static IPodcastRepository podcastTableRepository;

        public EpisodesController()
        {
            episodeTableRepository = EpisodeTableRepositoryFactory.Create();            
            podcastTableRepository = PodcastTableRepositoryFactory.Create();            
        }

        [Route("api/podcasts/{podcastId}/episodes")]
        [HttpGet]
        public IHttpActionResult GetEpisodesByPodCast(Guid podcastId)
        {
            Podcast podcast = podcastTableRepository.Get(podcastId);
            IEnumerable<Episode> episodes = episodeTableRepository.GetAll(podcast.Title);

            return Ok(episodes);
        }
    }
}
