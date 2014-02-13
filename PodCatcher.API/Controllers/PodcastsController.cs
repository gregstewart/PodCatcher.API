using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.WebPages;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Controllers
{
    public class PodcastsController : ApiController
    {
        private static IPodcastRepository podcastTableRepository;
        private static IPodcastRepository podcastBlobRepository;
        private static IPodcastBuilder podcastBuilder;

        public PodcastsController()
        {
            podcastTableRepository = PodcastTableRepositoryFactory.Create();
            podcastBlobRepository = PodcastBlobRepositoryFactory.Create();
            podcastBuilder = PodcastBuilderFactory.Create();
        }

        public IEnumerable<Podcast> GetAll()
        {
            IEnumerable<Podcast> podcasts = podcastTableRepository.GetAll();
            IEnumerable<Podcast> desiarlisedPodcasts = podcastBlobRepository.GetAll(podcasts);
            return desiarlisedPodcasts;
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
                    podcastBlobRepository.Add(builtPodcast);
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
    }
}
