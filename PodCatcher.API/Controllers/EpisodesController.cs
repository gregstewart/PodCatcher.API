using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Episodes;

namespace PodCatcher.API.Controllers
{
    public class EpisodesController : ApiController
    {

        private static IEpisodeRepository episodeTableRepository;

        public EpisodesController()
        {
            episodeTableRepository = EpisodeTableRepositoryFactory.Create();            
        }

        public IHttpActionResult Get(Guid podcastId)
        {
            IEnumerable<Episode> episodes = episodeTableRepository.GetAll(podcastId);
            
            return Ok(episodes);
        }
    }
}
