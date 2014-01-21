using System;
using System.Web.Http.Results;
using PodCatcher.API.Models;
using NUnit.Framework;

namespace PodCatcher.API.Controllers
{
    [TestFixture]
    public class PodcastsControllerTest
    {
        [Test]
        public void PostNewPodcastReturnsCreated()
        {
            // Arrange
            PodcastsController controller = new PodcastsController();

            // Act
            var httpWwwTestComFeedXml = "http://www.test.com/feed.xml";
            var actionResult = controller.Post(httpWwwTestComFeedXml);

            // Assert
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<Podcast>;

            Assert.IsNotNull(response);
            Assert.AreEqual("DefaultApi", response.RouteName);
            Assert.AreEqual(response.Content.Id, response.RouteValues["Id"]);
        }

        [Test, Ignore]
        public void PostDuplicatePodcastReturnsConflict()
        {
            //should return 409 or 422
        }

        [Test, Ignore]
        public void PostEmptyUrlReturnsBadRequest()
        {
            // should return 400;
        }

        [Test]
        public void GetPodcastAfterPostNewFeedShouldReturnOk()
        {
            // Arrange
            PodcastsController controller = new PodcastsController();
            var httpWwwTestComFeedXml = "http://www.test.com/feed.xml";
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
