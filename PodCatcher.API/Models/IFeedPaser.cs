using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodCatcher.API.Models
{
    public interface IFeedParser
    {
        Podcast Parse(Podcast podcast, string xml);
    }
}
