using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PodCatcher.API.Models
{
    public interface IFeedFetcher
    {
        HttpResponseMessage GetFeed(string Uri);
    }
}
