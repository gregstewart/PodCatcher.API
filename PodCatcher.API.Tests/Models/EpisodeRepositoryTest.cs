using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Episodes;
using PodCatcher.API.Models.Podcasts;

namespace PodCatcher.API.Tests.Models
{
    [TestFixture]
    class EpisodeRepositoryTest
    {
        private IEpisodeRepository _episodeRepository;
        private List<Episode> episodes;
        private Episode episode1;
        private Episode episode2;

        [SetUp]
        public void Init()
        {
            _episodeRepository = new EpisodeRepositoryMemory();
            episodes = new List<Episode>();
            episode1 = new Episode { Title = "Test 1", PublicationDate = new DateTime(2013, 12, 12) };
            episode2 = new Episode { Title = "Test 2", PublicationDate = new DateTime(2014, 01, 12) };
            episodes.Add(episode1);
            episodes.Add(episode2);
        }

        [Test]
        public void Add_WithEpisode_IsSuccessful()
        {
            _episodeRepository.Add(episode1);

            var enumerable = _episodeRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 1, "Failed to add new Episode collection");
        }
        [Test]
        public void Add_WithEpisodesCollection_IsSuccessful()
        {
            Podcast podcast = new Podcast { Uri = "http://www.test.com/feed.xml" };
            _episodeRepository.Add(podcast, episodes);

            var enumerable = _episodeRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 2, "Failed to add new Episode collection");
        }

        [Test]
        public void Add_WithEmptyEisodeCollection_IsUnsuccessful()
        {
            Podcast podcast = new Podcast { Uri = "http://www.test.com/feed.xml" };
            _episodeRepository.Add(podcast, new List<Episode>());

            var enumerable = _episodeRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 0, "Failed to add new Episode collection");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_WithNull_ShouldNotAddToCollection()
        {
            _episodeRepository.Add(null);

            var enumerable = _episodeRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 0, "Failed to add new Episode collection");
        }

        [Test]
        public void GetAll_ReturnsItemsInTheRepository_InTheCorrectOrder()
        {
            _episodeRepository.Add(episode1);
            _episodeRepository.Add(episode2);

            var enumerable = _episodeRepository.GetAll();
            Assert.IsTrue(enumerable.Count() == 2, "Failed to get 2 items added to the collection");
            Assert.IsTrue(enumerable.ElementAt(0).Equals(episode2), "Episode 1 is not the first item in the result");
        }
    }    
}
