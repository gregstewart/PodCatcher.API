﻿namespace PodCatcher.API.Models
{
    public interface IFeedFetcher
    {
        Feed GetFeed(string Uri);
    }
}
