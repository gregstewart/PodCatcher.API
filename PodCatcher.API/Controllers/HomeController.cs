using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using PodCatcher.API.Models;

namespace PodCatcher.API.Controllers
{
    public class HomeController : ApiController
    {
        public IHttpActionResult Get()
        {
            Uri entryPointUri = GetEntryPointUri();
            const string path = "podcasts";

            var metaData = new MetaData(entryPointUri, path);
            return Ok(metaData);
        }
        // duplicated in podcast controller....
        private Uri GetEntryPointUri()
        {
            return new Uri(Url.Request.RequestUri.ToString());
        }

    }
}
