using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PodCatcher.API.Models
{
    public interface IFeedFetcherWrapper
    {
        HttpResponseMessage GetFeed(string Uri);
    }
}
