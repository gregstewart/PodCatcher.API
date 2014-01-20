using System;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PodCatcher.API.Models;

namespace PodCatcher.API.Controllers
{
    [TestClass]
    public class QuizControllerTest
    {
        [TestMethod]
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

        [TestMethod, Ignore]
        public void PostDuplicateFeedReturnsConflict()
        {
            //should return 409
        }

        [TestMethod]
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

        [TestMethod]
        public void GetFeedWithAnUnknownIdItShouldReturnNotFound()
        {
            // Arrange
            var controller = new FeedsController();

            // Act
            var actionResult = controller.Get(new Guid("05963625-9f5c-4e66-a573-c70bb36cc225"));

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod, Ignore]
        public void PutFeed()
        {
            
        }

        [TestMethod, Ignore]
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
