using System;
using System.Web.Http.Results;
using PodCatcher.API.Models;
using NUnit.Framework;

namespace PodCatcher.API.Controllers
{
    [TestFixture]
    public class FeedsControllerTest
    {
        [Test]
        public void PostNewFeedReturnsCreated()
        {
            // Arrange
            FeedsController controller = new FeedsController();

            // Act
            var httpWwwTestComFeedXml = "http://www.test.com/feed.xml";
            var actionResult = controller.Post(httpWwwTestComFeedXml);

            // Assert
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<Feed>;

            Assert.IsNotNull(response);
            Assert.AreEqual("DefaultApi", response.RouteName);
            Assert.AreEqual(response.Content.Id, response.RouteValues["Id"]);
        }

        [Test, Ignore]
        public void PostDuplicateFeedReturnsConflict()
        {
            //should return 409 or 422
        }

        [Test, Ignore]
        public void PostEmptyUrlReturnsBadRequest()
        {
            // should return 400;
        }

        [Test]
        public void GetFeedAfterPostNewFeedShouldReturnOk()
        {
            // Arrange
            FeedsController controller = new FeedsController();
            var httpWwwTestComFeedXml = "http://www.test.com/feed.xml";
            var actionResult = controller.Post(httpWwwTestComFeedXml);
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<Feed>;

            // Act
            var getActionResult = controller.Get(Id(response));
            // Assert
            var getResponse = getActionResult as OkNegotiatedContentResult<Feed>;
            Assert.IsNotNull(getResponse);
            Assert.AreEqual(Id(response), Id(getResponse));
        }

        [Test]
        public void GetFeedWithAnUnknownIdItShouldReturnNotFound()
        {
            // Arrange
            var controller = new FeedsController();

            // Act
            var actionResult = controller.Get(new Guid("05963625-9f5c-4e66-a573-c70bb36cc225"));

            // Assert
            Assert.AreEqual(actionResult.GetType(), typeof(System.Web.Http.Results.NotFoundResult));
        }

        [Test, Ignore]
        public void PutFeed()
        {
            
        }

        [Test, Ignore]
        public void DeleteFeed()
        {

        }

        private Guid Id(OkNegotiatedContentResult<Feed> response)
        {
            return response.Content.Id;
        }

        private static Guid Id(CreatedAtRouteNegotiatedContentResult<Feed> response)
        {
            return response.Content.Id;
        }
    }
}
