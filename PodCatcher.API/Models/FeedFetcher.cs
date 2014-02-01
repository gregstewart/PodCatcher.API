using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.WebPages;

namespace PodCatcher.API.Models
{
    public class FeedFetcher : IFeedFetcher
    {
        
        public HttpResponseMessage GetFeed(string Uri)
        {
            if (Uri.IsEmpty())
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Uri);
            try
            {
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    string data = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();
                    var result = new HttpResponseMessage(response.StatusCode);
                    result.Content = new StringContent(data);

                    return result;
                }
                else
                {
                    return new HttpResponseMessage(response.StatusCode);
                }
            }
            catch (WebException e)
            {
                return new HttpResponseMessage(((HttpWebResponse) e.Response).StatusCode);
            }
            
        }
    }
}