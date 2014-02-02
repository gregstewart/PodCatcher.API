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
        private static IPodcastRepository repository;
        private static IPodcastBuilder podcastBuilder;

        public PodcastsController()
        {
            repository = PodcastRepositoryFactory.Create();
            podcastBuilder = PodcastBuilderFactory.Create();
        }

        public IEnumerable<Podcast> GetAll()
        {
            return repository.GetAll();
        }

        public IHttpActionResult Get(Guid id)
        {
            Podcast item = repository.Get(id);
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
                    builtPodcast = repository.Add(builtPodcast);                    
                    return CreatedAtRoute("DefaultApi", new {builtPodcast.Id}, builtPodcast);
                }
                catch(Exception)
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
