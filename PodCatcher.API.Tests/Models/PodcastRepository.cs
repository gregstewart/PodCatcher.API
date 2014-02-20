using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Podcasts;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class PodcastRepositoryTest
    {
        private IPodcastRepository _mPodcastRepository;
        
        [SetUp]
        public void Init()
        {
            _mPodcastRepository = new PodcastRepositoryMemory();
        }

        [Test]
        public void Add_WithPodcast_IsSuccessful()
        {
            Podcast podcast = new Podcast {Uri = "http://rubyrogues.com/feed/"};
            _mPodcastRepository.Add(podcast);

            var enumerable = _mPodcastRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 1, "Failed to add new PodcastFeed");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_WithNull_ShouldNotAddToCollection()
        {
            _mPodcastRepository.Add(null);

            var enumerable = _mPodcastRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 0, "Failed to add new PodcastFeed");
        }
    }

}
