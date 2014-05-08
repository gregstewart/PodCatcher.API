using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PodCatcher.API.Models;
using PodCatcher.API.Models.Episodes;
using PodCatcher.API.Models.Podcasts;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Models
{
    public class SyncPodcastsTests
    {
        private PodcastRepositoryStub _mPodcastRepositoryStub;
        private EpisodeRepositoryStub _mEpisodeRepositoryStub;
        private static PodcastBuilderStub _mPodcastBuilderStub;
        private LoggerStub _mlogger;

        [SetUp]
        public void Init()
        {
            _mPodcastRepositoryStub = new PodcastRepositoryStub();
            _mEpisodeRepositoryStub = new EpisodeRepositoryStub();
            _mlogger = new LoggerStub();

            PodcastTableRepositoryFactory.SetPodcastRepository(_mPodcastRepositoryStub);
            EpisodeTableRepositoryFactory.SetEpisodeRepository(_mEpisodeRepositoryStub);
            LoggerFactory.SetLogger(_mlogger);
        }

        [Test]
        public void GetStoredPodcasts_WhenRepositoryHasData_ReturnsACollectionOfPodcasts()
        {
            // Arrange
            SetUpPodcastsRepository();
            SyncPodcasts sync = new SyncPodcasts();

            // Act
            IEnumerable<Podcast> podcasts = sync.GetStoredPodcasts();

            // Assert
            Assert.IsTrue(podcasts.Any(), "Does no return a collection of stored podcasts");
        }

        [Test]
        public void GetStoredEpisodes_WhenRepositoryHasData_ReturnsEpisodeCollection()
        {
            // Arrange 
            var podcast = new Podcast { Id = new Guid(), Uri = "http://some.uri/" };
            SetUpEpisodeRepository();
            SyncPodcasts sync = new SyncPodcasts();

            // Act
            IEnumerable<Episode> episodes = sync.GetStoredEpisodes(podcast);

            Assert.IsTrue(episodes.Any(), "Does not return a collection");
        }

        [Test]
        public void GetLatestStoredEpisode_WhenCollectionHasItems_ReturnsTheNewestEpisodeInTheCollection()
        {
            // Arrange
            var podcast = new Podcast { Id = new Guid(), Uri = "http://some.uri/" };
            SetUpEpisodeRepository();
            SyncPodcasts sync = new SyncPodcasts();
            IEnumerable<Episode> episodes = sync.GetStoredEpisodes(podcast);
            
            // Act
            var episode = sync.GetLatestStoredEpisode(episodes);
            var latestEpisode = _mEpisodeRepositoryStub.EpisodesToBeReturned[0];

            Assert.IsTrue(episode == latestEpisode);   
        }

        [Test]
        public void GetLatestStoredEpisode_WhenCollectionIsEmpty_ReturnsNull()
        {
            // Arrange
            var podcast = new Podcast { Id = new Guid(), Uri = "http://some.uri/" };
            SetUpEpisodeRepository();
            SyncPodcasts sync = new SyncPodcasts();
            IEnumerable<Episode> episodes = sync.GetStoredEpisodes(podcast);

            // Act
            var episode = sync.GetLatestStoredEpisode(Enumerable.Empty<Episode>());
            
            Assert.IsTrue(episode == null);
        }

        [Test]
        public void GetFeed_WithResult_ReturnsAParsedPodcastAndEpisodes()
        {
            // Arrange
            var podcast = SetUpPodcastBuilder();
            SyncPodcasts sync = new SyncPodcasts();
            
            // Act
            PodcastFeed podcastFeed = sync.GetFeed(podcast);

            Assert.IsTrue(podcastFeed.Podcast == podcast);
        }

        [Test]
        public void GetDelta_GivenTheLatestEpisodeAndAnUpdatedCollection_ReturnsTheDelta()
        {
            // Arrange
            var podcastEpisode = new Episode { PublicationDate = new DateTime(2014, 1, 1)};
            var podcastEpisodes = new List<Episode>();
            SyncPodcasts sync = new SyncPodcasts();

            podcastEpisodes.Add(new Episode { PublicationDate = new DateTime(2014, 1, 2) });
            podcastEpisodes.Add(new Episode { PublicationDate = new DateTime(2014, 2, 2) });
            podcastEpisodes.Add(new Episode { PublicationDate = new DateTime(2014, 4, 2) });
            podcastEpisodes.Add(new Episode { PublicationDate = new DateTime(2013, 1, 2) });

            // Act
            var episodesToSync = sync.GetDelta(podcastEpisode, podcastEpisodes);

            // Assert
            Assert.IsTrue(episodesToSync.Count().Equals(3), "Does not return a collection of three episodes");
        }

        [Test]
        public void GetDelta_GivenTheLatestEpisodeIsNullAndAnUpdatedCollection_ReturnsAll()
        {
            // Arrange
            var podcastEpisodes = new List<Episode>();
            SyncPodcasts sync = new SyncPodcasts();

            podcastEpisodes.Add(new Episode { PublicationDate = new DateTime(2014, 1, 2) });
            podcastEpisodes.Add(new Episode { PublicationDate = new DateTime(2014, 2, 2) });
            podcastEpisodes.Add(new Episode { PublicationDate = new DateTime(2014, 4, 2) });
            podcastEpisodes.Add(new Episode { PublicationDate = new DateTime(2013, 1, 2) });

            // Act
            var episodesToSync = sync.GetDelta(null, podcastEpisodes);

            // Assert
            Assert.IsTrue(episodesToSync.Count().Equals(4), "Does not return a collection of 4 episodes");
        }

        [Test]
        public void Sync_WithData_CallsAllTheMethods()
        {
            // Arrange
            var podcast = SetUpPodcastBuilder();
            SetUpPodcastsRepository();
            SetUpEpisodeRepository();
            
            var syncPodcastsMock = new Mock<SyncPodcasts>();
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetStoredPodcasts()).Returns(_mPodcastRepositoryStub.PodcastsToBeReturned);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetStoredEpisodes(It.IsAny<Podcast>())).Returns(_mEpisodeRepositoryStub.EpisodesToBeReturned);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetLatestStoredEpisode(It.IsAny<List<Episode>>())).Returns(_mEpisodeRepositoryStub.EpisodesToBeReturned[0]);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetDelta(It.IsAny<Episode>(), It.IsAny<List<Episode>>())).Returns(_mEpisodeRepositoryStub.EpisodesToBeReturned);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetFeed(It.IsAny<Podcast>())).Returns(_mPodcastBuilderStub.ToReturn);
            
            // Act
            syncPodcastsMock.Object.Sync();

            // Assert
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredPodcasts(), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredEpisodes(It.IsAny<Podcast>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetLatestStoredEpisode(It.IsAny<List<Episode>>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetFeed(It.IsAny<Podcast>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetDelta(It.IsAny<Episode>(), It.IsAny<List<Episode>>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.UpdateEpisodes(It.IsAny<Podcast>(), It.IsAny<List<Episode>>()), Times.AtLeastOnce());
            
        }

        [Test]
        public void Sync_WithNoPodcasts_OnlyCallsGetStoredPodcasts()
        {
            // Arrange
            var podcast = SetUpPodcastBuilder();
            SetUpPodcastsRepository();
            SetUpEpisodeRepository();
            
            var syncPodcastsMock = new Mock<SyncPodcasts>();
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetStoredPodcasts()).Returns(Enumerable.Empty<Podcast>());
            
            // Act
            syncPodcastsMock.Object.Sync();

            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredPodcasts(), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredEpisodes(It.IsAny<Podcast>()), Times.Never);
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetLatestStoredEpisode(It.IsAny<List<Episode>>()), Times.Never);
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetFeed(It.IsAny<Podcast>()), Times.Never);
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetDelta(It.IsAny<Episode>(), It.IsAny<List<Episode>>()), Times.Never);
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.UpdateEpisodes(It.IsAny<Podcast>(), It.IsAny<List<Episode>>()), Times.Never);
        }

        [Test]
        public void Sync_WithNoEpisodes_CallsGetStoredPodcastsAndGetStoredEpisodes()
        {
            // Arrange
            var podcast = SetUpPodcastBuilder();
            SetUpPodcastsRepository();
            SetUpEpisodeRepository();

            var syncPodcastsMock = new Mock<SyncPodcasts>();
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetStoredPodcasts()).Returns(_mPodcastRepositoryStub.PodcastsToBeReturned);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetStoredEpisodes(It.IsAny<Podcast>())).Returns(Enumerable.Empty<Episode>());
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetLatestStoredEpisode(It.IsAny<List<Episode>>())).Returns((Episode) null);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetDelta(It.IsAny<Episode>(), It.IsAny<List<Episode>>())).Returns(_mEpisodeRepositoryStub.EpisodesToBeReturned);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetFeed(It.IsAny<Podcast>())).Returns(_mPodcastBuilderStub.ToReturn);

            // Act
            syncPodcastsMock.Object.Sync();

            // Assert
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredPodcasts(), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredEpisodes(It.IsAny<Podcast>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetLatestStoredEpisode(It.IsAny<List<Episode>>()), Times.Never);
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetFeed(It.IsAny<Podcast>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetDelta(It.IsAny<Episode>(), It.IsAny<List<Episode>>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.UpdateEpisodes(It.IsAny<Podcast>(), It.IsAny<List<Episode>>()), Times.AtLeastOnce());
        }
        
        [Test]
        public void Sync_WithNoUpdatedPodcastFeed_DoesNotCallGetDelta()
        {
            // Arrange
            var podcast = SetUpPodcastBuilder();
            SetUpPodcastsRepository();
            SetUpEpisodeRepository();
            _mPodcastBuilderStub.ToReturn.Episodes = Enumerable.Empty<Episode>();

            var syncPodcastsMock = new Mock<SyncPodcasts>();
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetStoredPodcasts()).Returns(_mPodcastRepositoryStub.PodcastsToBeReturned);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetStoredEpisodes(It.IsAny<Podcast>())).Returns(Enumerable.Empty<Episode>());
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetLatestStoredEpisode(It.IsAny<List<Episode>>())).Returns((Episode)null);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetDelta(It.IsAny<Episode>(), It.IsAny<List<Episode>>())).Returns(_mEpisodeRepositoryStub.EpisodesToBeReturned);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetFeed(It.IsAny<Podcast>())).Returns(_mPodcastBuilderStub.ToReturn);

            // Act
            syncPodcastsMock.Object.Sync();

            // Assert
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredPodcasts(), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredEpisodes(It.IsAny<Podcast>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetLatestStoredEpisode(It.IsAny<List<Episode>>()), Times.Never);
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetFeed(It.IsAny<Podcast>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetDelta(It.IsAny<Episode>(), It.IsAny<List<Episode>>()), Times.Never);
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.UpdateEpisodes(It.IsAny<Podcast>(), It.IsAny<List<Episode>>()), Times.Never);
        }

        [Test]
        public void Sync_WithNoNewPodcasts_DoesNotCallUpdatePodcasts()
        {
            // Arrange
            var podcast = SetUpPodcastBuilder();
            SetUpPodcastsRepository();
            SetUpEpisodeRepository();

            var syncPodcastsMock = new Mock<SyncPodcasts>();
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetStoredPodcasts()).Returns(_mPodcastRepositoryStub.PodcastsToBeReturned);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetStoredEpisodes(It.IsAny<Podcast>())).Returns(Enumerable.Empty<Episode>());
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetLatestStoredEpisode(It.IsAny<List<Episode>>())).Returns((Episode)null);
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetDelta(It.IsAny<Episode>(), It.IsAny<List<Episode>>())).Returns(new List<Episode>());
            syncPodcastsMock.Setup(syncPodcasts => syncPodcasts.GetFeed(It.IsAny<Podcast>())).Returns(_mPodcastBuilderStub.ToReturn);

            // Act
            syncPodcastsMock.Object.Sync();

            // Assert
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredPodcasts(), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetStoredEpisodes(It.IsAny<Podcast>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetLatestStoredEpisode(It.IsAny<List<Episode>>()), Times.Never);
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetFeed(It.IsAny<Podcast>()), Times.AtLeastOnce());
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.GetDelta(It.IsAny<Episode>(), It.IsAny<List<Episode>>()), Times.AtLeastOnce);
            syncPodcastsMock.Verify(syncPodcasts => syncPodcasts.UpdateEpisodes(It.IsAny<Podcast>(), It.IsAny<List<Episode>>()), Times.Never);
        }

        private void SetUpPodcastsRepository()
        {
            var podcast1 = new Podcast { Id = new Guid(), Uri = "http://some.uri/" };
            var podcast2 = new Podcast { Id = new Guid(), Uri = "http://someother.uri/" };
            List<Podcast> podcastsToBeReturned = new List<Podcast>();
            podcastsToBeReturned.Add(podcast1);
            podcastsToBeReturned.Add(podcast2);
            _mPodcastRepositoryStub.PodcastsToBeReturned = podcastsToBeReturned;
        }

        private void SetUpEpisodeRepository()
        {
            var episodesToBeReturned = EpisodesToBeReturned();
            _mEpisodeRepositoryStub.EpisodesToBeReturned = episodesToBeReturned;
        }

        private static Podcast SetUpPodcastBuilder()
        {
            var podcast = new Podcast { Uri = "http://some.uri/" };
            var episodes = EpisodesToBeReturned();
            _mPodcastBuilderStub = new PodcastBuilderStub();
            PodcastBuilderFactory.SetPodcastBuilder(_mPodcastBuilderStub);
            _mPodcastBuilderStub.ToReturn = new PodcastFeed { Podcast = podcast, Episodes = episodes };
            return podcast;
        }

        
        private static List<Episode> EpisodesToBeReturned()
        {
            var episode1 = new Episode { Id = new Guid(), PublicationDate = new DateTime(2014, 1, 2) };
            var episode2 = new Episode { Id = new Guid(), PublicationDate = new DateTime(2014, 2, 2) };
            var episode3 = new Episode { Id = new Guid(), PublicationDate = new DateTime(2014, 5, 4) };
            var episode4 = new Episode { Id = new Guid(), PublicationDate = new DateTime(2014, 1, 3) };
            List<Episode> episodesToBeReturned = new List<Episode>();
            episodesToBeReturned.Add(episode1);
            episodesToBeReturned.Add(episode2);
            episodesToBeReturned.Add(episode3);
            episodesToBeReturned.Add(episode4);
            return episodesToBeReturned;
        }
    }
}
