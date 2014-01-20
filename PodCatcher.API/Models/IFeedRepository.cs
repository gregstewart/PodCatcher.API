using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PodCatcher.API.Models
{
    interface IFeedRepository
    {
        IEnumerable<Feed> GetAll();
        Feed Get(Guid Id);
        Feed Add(Feed item);
        void Remove(Guid Id);
        bool Update(Feed item);
    }
}
