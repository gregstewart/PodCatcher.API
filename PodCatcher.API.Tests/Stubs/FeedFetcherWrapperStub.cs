using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PodCatcher.API.Models;

namespace PodCatcher.API.Tests.Stubs
{
    public class FeedFetcherWrapperStub : IFeedFetcherWrapper
    {
        public HttpResponseMessage ToReturn;
        public HttpResponseMessage GetFeed(string Uri)
        {
            if (ToReturn != null)
            {
                return ToReturn;
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
