﻿using System;
using System.Net;
using System.Web.Http.Results;
using PodCatcher.API.Models;
using NUnit.Framework;
using PodCatcher.API.Models.Podcasts;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Controllers
{
    [TestFixture]
    public class PodcastsControllerTest
    {
        private PodcastRepositoryStub _mPodcastRepositoryStub;
        private PodcastBuilderStub _mPodcastBuilder;
        [SetUp]
        public void Init()
        {
            _mPodcastRepositoryStub = new PodcastRepositoryStub();
            PodcastTableRepositoryFactory.SetPodcastRepository(_mPodcastRepositoryStub);
            PodcastBlobRepositoryFactory.SetPodcastRepository(_mPodcastRepositoryStub);
            _mPodcastBuilder = new PodcastBuilderStub();
            PodcastBuilderFactory.SetPodcastBuilder(_mPodcastBuilder);
        }

        [Test]
        public void Post_NewPodcast_ReturnsCreated()
        {
            // Arrange
            var httpWwwTestComFeedXml = "http://www.test.com/feed.xml";
            var podcast = new Podcast{Uri = httpWwwTestComFeedXml};
            _mPodcastBuilder.ToReturn = new PodcastFeed{Podcast = podcast};
            _mPodcastRepositoryStub.PodcastToBeReturned = podcast;
            PodcastsController controller = new PodcastsController();

            // Act
            var actionResult = controller.Post(podcast);

            // Assert
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<PodcastFeed>;

            Assert.IsNotNull(response);
            Assert.AreEqual("DefaultApi", response.RouteName);
            Assert.AreEqual(response.Content.Podcast.Id, response.RouteValues["Id"]);
        }
        [Test]
        public void Post_Null_ReturnsBadRequest()
        {
            // Arrange
            string httpWwwTestComFeedXml = null;
            Podcast podcast = new Podcast { Uri = httpWwwTestComFeedXml };
            _mPodcastRepositoryStub.PodcastToBeReturned = podcast;
            PodcastsController controller = new PodcastsController();

            // Act
            var actionResult = controller.Post(podcast);

            // Assert
            var getResponse = actionResult as StatusCodeResult;
            Assert.AreEqual(getResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test, Ignore]
        public void Post_DuplicatePodcast_ReturnsConflict()
        {
            //should return 409 or 422
        }

        [Test]
        public void Post_EmptyUrl_ReturnsBadRequest()
        {
            // Arrange
            var httpWwwTestComFeedXml = "";
            var podcast = new Podcast { Uri = httpWwwTestComFeedXml };
            _mPodcastRepositoryStub.PodcastToBeReturned = podcast;
            PodcastsController controller = new PodcastsController();

            // Act
            var actionResult = controller.Post(podcast);

            // Assert
            var getResponse = actionResult as StatusCodeResult;
            Assert.AreEqual(getResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void Post_ValidUrlButPodcastIsNull_ReturnsBadRequest()
        {
            // Arrange
            _mPodcastRepositoryStub.PodcastToBeReturned = null;
            _mPodcastRepositoryStub.ToBeThrown = new ArgumentNullException("item");
            PodcastsController controller = new PodcastsController();

            // Act
            var actionResult = controller.Post(null);

            // Assert
            var getResponse = actionResult as StatusCodeResult;
            Assert.AreEqual(getResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void Get_PodcastAfterPost_ReturnsOk()
        {
            // Arrange
            var httpWwwTestComFeedXml = "http://www.test.com/feed.xml";
            var podcast = new Podcast {Id = Guid.NewGuid(), Uri = httpWwwTestComFeedXml};
            var podcastFeed = new PodcastFeed {Podcast = podcast};
            _mPodcastRepositoryStub.PodcastToBeReturned = podcast;
            _mPodcastBuilder.ToReturn = podcastFeed;
            
            PodcastsController controller = new PodcastsController();
            var actionResult = controller.Post(podcast);
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<PodcastFeed>;

            // Act
            var getActionResult = controller.Get(Id(response));

            // Assert
            var getResponse = getActionResult as OkNegotiatedContentResult<PodcastFeed>;
            Assert.IsNotNull(getResponse);
            Assert.AreEqual(Id(response), Id(getResponse));
        }

        [Test]
        public void Get_PodcastWithAnUnknownIdIt_ReturnsNotFound()
        {
            // Arrange
            var controller = new PodcastsController();

            // Act
            var actionResult = controller.Get(new Guid("05963625-9f5c-4e66-a573-c70bb36cc225"));

            // Assert
            Assert.AreEqual(actionResult.GetType(), typeof(System.Web.Http.Results.NotFoundResult));
        }

        [Test, Ignore]
        public void Put_Podcast()
        {
            
        }

        [Test, Ignore]
        public void Delete_Podcast()
        {

        }

        private Guid Id(OkNegotiatedContentResult<PodcastFeed> response)
        {
            return response.Content.Podcast.Id;
        }

        private static Guid Id(CreatedAtRouteNegotiatedContentResult<PodcastFeed> response)
        {
            return response.Content.Podcast.Id;
        }
    }
}
