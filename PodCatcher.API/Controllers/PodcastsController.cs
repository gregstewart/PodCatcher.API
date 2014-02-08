using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.WebPages;
using PodCatcher.API.Models;

namespace PodCatcher.API.Controllers
{
    public class PodcastsController : ApiController
    {
        private static IPodcastRepository podcastTableRepository;
        private static PodcastBlobRepository _podcastBlobRepository;
        private static IPodcastBuilder podcastBuilder;

        public PodcastsController()
        {
            podcastTableRepository = PodcastRepositoryFactory.Create();
            _podcastBlobRepository = new PodcastBlobRepository();
            podcastBuilder = PodcastBuilderFactory.Create();
        }

        public IEnumerable<Podcast> GetAll()
        {
            IEnumerable<Podcast> podcasts = podcastTableRepository.GetAll();
            IEnumerable<Podcast> desiarlisedPodcasts =_podcastBlobRepository.GetAll(podcasts);
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
                    Podcast builtPodcast = podcastBuilder.Build(podcast.Uri);
                    _podcastBlobRepository.Add(builtPodcast);
                    builtPodcast = podcastTableRepository.Add(builtPodcast);                    
                    return CreatedAtRoute("DefaultApi", new {builtPodcast.Id}, builtPodcast);
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
