using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PodCatcher.API.Models;

namespace PodCatcher.API.Controllers
{
    public class FeedsController : ApiController
    {
        private static readonly IFeedRepository repository = new FeedRepository();

        public IEnumerable<Feed> GetAll()
        {
            return repository.GetAll();
        }

        public IHttpActionResult Get(Guid id)
        {
            Feed item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        public IHttpActionResult Post(string Uri)
        {
            Feed feed = new Feed {Uri = Uri};
            feed = repository.Add(feed);
            return CreatedAtRoute("DefaultApi", new { feed.Id }, feed);
        }

        public void Put(Guid id, Feed item)
        {
            item.Id = id;
            if (!repository.Update(item))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void Delete(Guid id)
        {
            Feed item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            repository.Remove(id);
        }
    }
}
