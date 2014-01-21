using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PodCatcher.API.Models;

namespace PodCatcher.API.Controllers
{
    public class PodcastsController : ApiController
    {
        private static readonly IPodcastRepository repository = new PodcastRepository();

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
            Podcast podcast = new Podcast {Uri = Uri};
            podcast = repository.Add(podcast);
            return CreatedAtRoute("DefaultApi", new { podcast.Id }, podcast);
        }

        public void Put(Guid id, Podcast item)
        {
            item.Id = id;
            if (!repository.Update(item))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void Delete(Guid id)
        {
            Podcast item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            repository.Remove(id);
        }
    }
}
