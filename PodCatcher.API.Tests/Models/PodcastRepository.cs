using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    public class PodcastRepositoryTest
    {
        private PodcastRepository _mPodcastRepository;

        [SetUp]
        public void Init()
        {
            _mPodcastRepository = new PodcastRepository();
        }

        [Test]
        public void Add_WithPodcast_IsSuccessful()
        {
            _mPodcastRepository.Add(new Podcast { Uri = "http://rubyrogues.com/feed/" });

            var enumerable = _mPodcastRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 1, "Failed to add new Podcast");
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_WithNull_ShouldNotAddToCollection()
        {
            _mPodcastRepository.Add(null);

            var enumerable = _mPodcastRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 0, "Failed to add new Podcast");
        }
    }

}
