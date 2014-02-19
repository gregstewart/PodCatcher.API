using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using NUnit.Framework;
using PodCatcher.API.Controllers;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Episodes;
using PodCatcher.API.Models.Podcasts;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Controllers
{
    class EpisodesControllerTests
    {
        private EpisodeRepositoryStub _mEpisodeRepositoryStub;
        private PodcastRepositoryStub _mPodcastRepositoryStub;
        private List<Episode> episodes;
        private Episode episode1;
        private Episode episode2;

        [SetUp]
        public void Init()
        {
            _mEpisodeRepositoryStub = new EpisodeRepositoryStub();
            _mPodcastRepositoryStub = new PodcastRepositoryStub();
            EpisodeTableRepositoryFactory.SetEpisodeRepository(_mEpisodeRepositoryStub);
            PodcastTableRepositoryFactory.SetPodcastRepository(_mPodcastRepositoryStub);
            episodes = new List<Episode>();
            episode1 = new Episode { Title = "Test 1" };
            episode2 = new Episode { Title = "Test 2" };
            episodes.Add(episode1);
            episodes.Add(episode2);
        }

        [Test]
        public void GetAll_WithValidGuid_ReturnsOK()
        {
            // Arrange
            EpisodesController controller = new EpisodesController();
            Guid podcastGuid = GetPodcastGuid();
            Podcast podcast = new Podcast {Title = "Convert to Raid: The podcast for raiders in World of Warcraft"};
            _mEpisodeRepositoryStub.EpisodesToBeReturned = episodes;
            _mPodcastRepositoryStub.PodcastToBeReturned = podcast;
            
            // Act
            var actionResult = controller.GetEpisodesByPodCast(podcastGuid);

            // Assert
            var getResponse = actionResult as OkNegotiatedContentResult<IEnumerable<Episode>>;
            Assert.IsNotNull(getResponse);

            foreach (var content in getResponse.Content)
            {
                Assert.AreEqual(content.GetType(), typeof(Episode));
            }

        }

        private Guid GetPodcastGuid()
        {
            var RowKey = new Guid("8740c4dc-fde7-480b-9e81-889672dc9c44");
            return RowKey;
        }
    }
}
