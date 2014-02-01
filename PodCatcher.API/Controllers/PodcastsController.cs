using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
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

        public IHttpActionResult Post(string Uri)
        {
            if (!Uri.IsEmpty())
            {
                try 
                {
                    Podcast podcast = podcastBuilder.Build(Uri);
                    podcast = repository.Add(podcast);                    
                    return CreatedAtRoute("DefaultApi", new {podcast.Id}, podcast);
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

        public void Put(Guid id, Podcast item)
        {
//            item.Id = id;
//            if (!repository.Update(item))
//            {
//                throw new HttpResponseException(HttpStatusCode.NotFound);
//            }
        }

        public void Delete(Guid id)
        {
//            Podcast item = repository.Get(id);
//            if (item == null)
//            {
//                throw new HttpResponseException(HttpStatusCode.NotFound);
//            }
//
//            repository.Remove(id);
        }
    }
}
