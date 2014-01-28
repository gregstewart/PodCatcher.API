using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models;

namespace PodCatcher.API.Tests.Stubs
{
    public class FeedFetcherStub : IFeedFetcher
    {
        public HttpResponseMessage ToReturn;
        public HttpResponseMessage GetFeed(string Uri)
        {
            if (ToReturn != null)
            {
                return ToReturn;
            }

            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            return httpResponseMessage;
        }
    }
}
