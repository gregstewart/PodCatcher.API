using System;

namespace PodCatcher.API.Models
{
    public class MetaData
    {
        protected Uri entryPointUri;
        protected string path = "";

        public MetaData()
        {
            
        }
        public MetaData(Uri entryPointUri, String path)
        {
            this.entryPointUri = entryPointUri;
            this.path = path;
        }

        public Uri Link
        {
            get
            {
                var constructedUri = FixTrailingSlash(this.entryPointUri.ToString());
                constructedUri += this.path;
                return new Uri(constructedUri);
            }
        }

        protected string FixTrailingSlash(string Uri)
        {
            if (!Uri.EndsWith("/"))
            {
                return Uri + "/";
            }

            return Uri;
        }
    }
}