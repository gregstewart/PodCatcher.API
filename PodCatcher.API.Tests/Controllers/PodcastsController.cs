using System;
using System.Net;
using System.Web.Http.Results;
using PodCatcher.API.Models;
using NUnit.Framework;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Controllers
{
    [TestFixture]
    public class PodcastsControllerTest
    {
        private PodcastRepositoryStub _mPodcastRepository;
        [SetUp]
        public void Init()
        {
            _mPodcastRepository = new PodcastRepositoryStub();
            PodcastRepositoryFactory.SetPodcastRepository(_mPodcastRepository);
        }

        [Test]
        public void PostNewPodcastReturnsCreated()
        {
            // Arrange
            var httpWwwTestComFeedXml = "http://www.test.com/feed.xml";
            _mPodcastRepository.PodcastToBeReturned = new Podcast{Uri = httpWwwTestComFeedXml};
            PodcastsController controller = new PodcastsController();

            // Act
            var actionResult = controller.Post(httpWwwTestComFeedXml);

            // Assert
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<Podcast>;

            Assert.IsNotNull(response);
            Assert.AreEqual("DefaultApi", response.RouteName);
            Assert.AreEqual(response.Content.Id, response.RouteValues["Id"]);
        }
        [Test]
        public void PostNullReturnsBadRequest()
        {
            // Arrange
            string httpWwwTestComFeedXml = null;
            _mPodcastRepository.PodcastToBeReturned = new Podcast();
            PodcastsController controller = new PodcastsController();

            // Act
            var actionResult = controller.Post(httpWwwTestComFeedXml);

            // Assert
            var getResponse = actionResult as StatusCodeResult;
            Assert.AreEqual(getResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test, Ignore]
        public void PostDuplicatePodcastReturnsConflict()
        {
            //should return 409 or 422
        }

        [Test]
        public void PostEmptyUrlReturnsBadRequest()
        {
            // Arrange
            var httpWwwTestComFeedXml = "";
            _mPodcastRepository.PodcastToBeReturned = new Podcast();
            PodcastsController controller = new PodcastsController();

            // Act
            var actionResult = controller.Post(httpWwwTestComFeedXml);

            // Assert
            var getResponse = actionResult as StatusCodeResult;
            Assert.AreEqual(getResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void PostValidUrlButPodcastIsNullReturnsBadRequest()
        {
            // Arrange
            var httpWwwTestComFeedXml = "http://someurl.com";
            _mPodcastRepository.PodcastToBeReturned = null;
            PodcastsController controller = new PodcastsController();

            // Act
            var actionResult = controller.Post(httpWwwTestComFeedXml);

            // Assert
            var getResponse = actionResult as StatusCodeResult;
            Assert.AreEqual(getResponse.StatusCode, HttpStatusCode.BadRequest);
        }
        [Test]
        public void GetPodcastAfterPostShouldReturnOk()
        {
            // Arrange
            var httpWwwTestComFeedXml = "http://www.test.com/feed.xml";
            _mPodcastRepository.PodcastToBeReturned = new Podcast { Id = Guid.NewGuid(), Uri = httpWwwTestComFeedXml };
            PodcastsController controller = new PodcastsController();
            var actionResult = controller.Post(httpWwwTestComFeedXml);
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<Podcast>;

            // Act
            var getActionResult = controller.Get(Id(response));
            // Assert
            var getResponse = getActionResult as OkNegotiatedContentResult<Podcast>;
            Assert.IsNotNull(getResponse);
            Assert.AreEqual(Id(response), Id(getResponse));
        }

        [Test]
        public void GetPodcastWithAnUnknownIdItShouldReturnNotFound()
        {
            // Arrange
            var controller = new PodcastsController();

            // Act
            var actionResult = controller.Get(new Guid("05963625-9f5c-4e66-a573-c70bb36cc225"));

            // Assert
            Assert.AreEqual(actionResult.GetType(), typeof(System.Web.Http.Results.NotFoundResult));
        }

        [Test, Ignore]
        public void PutPodcast()
        {
            
        }

        [Test, Ignore]
        public void DeletePodcast()
        {

        }

        private Guid Id(OkNegotiatedContentResult<Podcast> response)
        {
            return response.Content.Id;
        }

        private static Guid Id(CreatedAtRouteNegotiatedContentResult<Podcast> response)
        {
            return response.Content.Id;
        }
    }
}
