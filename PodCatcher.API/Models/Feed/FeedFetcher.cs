using System.IO;
using System.Net;
using System.Text;
using System.Web.WebPages;

namespace PodCatcher.API.Models
{
    public class FeedFetcher : IFeedFetcher
    {
        
        public Feed GetFeed(string Uri)
        {
            Feed feed = new Feed();
            if (Uri.IsEmpty())
            {
                feed.StatusCode = HttpStatusCode.BadRequest;
                return feed;
            }

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Uri);
            
            try
            {
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet.IsEmpty())
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    string data = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();
                    feed.StatusCode = HttpStatusCode.OK;
                    feed.Content = data;

                    return feed;
                }
                else
                {
                    feed.StatusCode = response.StatusCode;
                    return feed;
                }
            }
            catch (WebException e)
            {
                feed.StatusCode = ((HttpWebResponse) e.Response).StatusCode;
                return feed;
            }
            
        }
    }

    public class Feed
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
    }
}