using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using NUnit.Framework;
using PodCatcher.API.Controllers;
using PodCatcher.API.Models;

namespace PodCatcher.API.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Get_Index_ReturnsNotFound()
        {
            // Arrange 
            var entryPointUri = new Uri("http://localhost:81/api/");
            HomeController controller = new HomeController();
            SetupControllerForTests(controller, "");
            var metadata = new MetaData(entryPointUri, "podcasts");

            // Act
            var actionResult = controller.Get();
            var getResponse = actionResult as OkNegotiatedContentResult<MetaData>;
            var getMetaData = MetaData(getResponse);
            
            // Assert
            Assert.AreEqual(actionResult.GetType(), typeof(System.Web.Http.Results.OkNegotiatedContentResult<MetaData>));
            Assert.AreEqual(metadata.Link, getMetaData.Link);

        }

        private MetaData MetaData(OkNegotiatedContentResult<MetaData> response)
        {
            return response.Content;
        }


        private static void SetupControllerForTests(ApiController controller, string path)
        {
            var config = new HttpConfiguration();
            var requestedUri = "http://localhost:81/api/" + path;
            var request = new HttpRequestMessage(HttpMethod.Get, requestedUri);
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary
                {
                    {"id", Guid.Empty},
                    {"controller", "home"}
                });
            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            UrlHelper urlHelper = new UrlHelper(request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            /// inject a fake helper
            controller.Url = urlHelper;
        }
    }
}
