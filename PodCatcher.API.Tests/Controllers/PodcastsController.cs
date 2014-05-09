using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using PodCatcher.API.Controllers;
using PodCatcher.API.Models;
using NUnit.Framework;
using PodCatcher.API.Models.Episodes;
using PodCatcher.API.Models.Podcasts;
using PodCatcher.API.Tests.Stubs;

namespace PodCatcher.API.Tests.Controllers
{
    [TestFixture]
    public class PodcastsControllerTest
    {
        private PodcastRepositoryStub _mPodcastRepositoryStub;
        private EpisodeRepositoryStub _mEpisodeRepositoryStub;
        private PodcastBuilderStub _mPodcastBuilder;
        private LoggerStub _mlogger;
        
        [SetUp]
        public void Init()
        {
            _mPodcastRepositoryStub = new PodcastRepositoryStub();
            _mEpisodeRepositoryStub = new EpisodeRepositoryStub();
            _mPodcastBuilder = new PodcastBuilderStub();
            _mlogger = new LoggerStub();
            PodcastTableRepositoryFactory.SetPodcastRepository(_mPodcastRepositoryStub);
            EpisodeTableRepositoryFactory.SetEpisodeRepository(_mEpisodeRepositoryStub);
            PodcastBuilderFactory.SetPodcastBuilder(_mPodcastBuilder);
            LoggerFactory.SetLogger(_mlogger);
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

        [Test]
        public void Post_DuplicatePodcast_ReturnsConflict()
        {
            //should return 409
            // Arrange
            Podcast storedPodcast = new Podcast { Title = "Test Podcast" };
            Podcast submittedPodcast = new Podcast { Uri= "http://some.uri", Title = "Test Podcast" };
            var podcastFeed = new PodcastFeed { Podcast = submittedPodcast, Episodes = new List<Episode>() };
            _mPodcastRepositoryStub.PodcastToBeReturned = storedPodcast;
            _mPodcastBuilder.ToReturn = podcastFeed;
            PodcastsController controller = new PodcastsController();
            SetupControllerForTests(controller, null);

            // Act
            var actionResult = controller.Post(submittedPodcast);

            // Assert
            var getResponse = actionResult as StatusCodeResult;
            Assert.AreEqual(HttpStatusCode.Conflict, getResponse.StatusCode);
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
            var podcastFeed = new PodcastFeed {Podcast = podcast, Episodes = new List<Episode>()};
            _mPodcastRepositoryStub.PodcastToBeReturned = podcast;
            _mPodcastBuilder.ToReturn = podcastFeed;
            
            PodcastsController controller = new PodcastsController();
            SetupControllerForTests(controller, null);
            var actionResult = controller.Post(podcast);
            var response = actionResult as CreatedAtRouteNegotiatedContentResult<PodcastFeed>;

            // Act
            var getActionResult = controller.Get(Id(response));

            // Assert
            var getResponse = getActionResult as OkNegotiatedContentResult<Podcast>;
            Assert.IsNotNull(getResponse);
            Assert.AreEqual(Id(response), Id(getResponse));
        }

        [Test]
        public void Get_PodcastWithAnUnknownIdIt_ReturnsNotFound()
        {
            // Arrange
            var controller = new PodcastsController();
            SetupControllerForTests(controller, null);
            
            // Act
            var actionResult = controller.Get(new Guid("05963625-9f5c-4e66-a573-c70bb36cc225"));

            // Assert
            Assert.AreEqual(actionResult.GetType(), typeof(System.Web.Http.Results.NotFoundResult));
        }

        [Test]
        public void Get_PodcastWithAValidId_ReturnsOK()
        {
            // Arrange
            var newGuid = Guid.NewGuid();
            var podcast = new Podcast { Id = newGuid, Uri = "http://some.uri/" };
            _mPodcastRepositoryStub.PodcastToBeReturned = podcast;
            var controller = new PodcastsController();
            SetupControllerForTests(controller, null);
            
            // Act
            var actionResult = controller.Get(newGuid);

            // Assert
            Assert.AreEqual(actionResult.GetType(), typeof(System.Web.Http.Results.OkNegotiatedContentResult<Podcast>));
        }

        [Test]
        public void Get_PodcastWithAValidId_ReturnsOKAndHasLinkMetaData()
        {
            // Arrange
            var newGuid = Guid.NewGuid();
            var podcast = new Podcast { Id = newGuid, Uri = "http://some.uri/" };
            var entryPointUri = new Uri("http://localhost:81/api/podcasts/" + newGuid.ToString());
            var metadata = new PodcastMetaData(entryPointUri, "episodes");

            _mPodcastRepositoryStub.PodcastToBeReturned = podcast;
            var controller = new PodcastsController();
            SetupControllerForTests(controller, newGuid.ToString());
            
            // Act
            var actionResult = controller.Get(newGuid);
            var getResponse = actionResult as OkNegotiatedContentResult<Podcast>;
            var getMetaData = MetaData(getResponse);
            // Assert
            Assert.AreEqual(actionResult.GetType(), typeof(System.Web.Http.Results.OkNegotiatedContentResult<Podcast>));
            Assert.AreEqual(metadata.Link, getMetaData.Link);
        }

        [Test]
        public void Get_PodcastWithAValidId_ReturnsOKAndHasSubscribeLinkMetaData()
        {
            // Arrange
            var newGuid = Guid.NewGuid();
            var podcast = new Podcast { Id = newGuid, Uri = "http://some.uri/" };
            var entryPointUri = new Uri("http://localhost:81/api/podcasts/" + newGuid.ToString());
            var metadata = new PodcastMetaData(entryPointUri, "episodes");

            _mPodcastRepositoryStub.PodcastToBeReturned = podcast;
            var controller = new PodcastsController();
            SetupControllerForTests(controller, newGuid.ToString());

            // Act
            var actionResult = controller.Get(newGuid);
            var getResponse = actionResult as OkNegotiatedContentResult<Podcast>;
            var getMetaData = MetaData(getResponse);
            // Assert
            Assert.AreEqual(actionResult.GetType(), typeof(System.Web.Http.Results.OkNegotiatedContentResult<Podcast>));
            Assert.AreEqual(metadata.SubscribeLink, getMetaData.SubscribeLink);
        }

        [Test]
        public void Put_Podcast_ReturnsNotImplemented()
        {
            // Arrange
            var controller = new PodcastsController();
                      
            // Act
            var actionResult = controller.Put(new Podcast());
            
            // Assert
            Assert.AreEqual(actionResult.StatusCode, HttpStatusCode.NotImplemented);

        }

        [Test]
        public void Delete_Podcast_ReturnsNotImplemented()
        {
            // Arrange
            var controller = new PodcastsController();

            // Act
            var actionResult = controller.Delete(new Podcast());

            // Assert
            Assert.AreEqual(actionResult.StatusCode, HttpStatusCode.NotImplemented);
        }

        [Test]
        public void Post_PodcastSubscribe_ReturnsNotImplemented()
        {
            // Arrange
            var controller = new PodcastsController();
            Guid podcastGuid = new Guid("8740c4dc-fde7-480b-9e81-889672dc9c44");

            // Act
            var actionResult = controller.SubscribeByPodcastId(podcastGuid);

            // Assert
            Assert.AreEqual(actionResult.StatusCode, HttpStatusCode.NotImplemented);
        }

        private Guid Id(OkNegotiatedContentResult<Podcast> response)
        {
            return response.Content.Id;
        }

        private static Guid Id(CreatedAtRouteNegotiatedContentResult<PodcastFeed> response)
        {
            return response.Content.Podcast.Id;
        }

        private PodcastMetaData MetaData(OkNegotiatedContentResult<Podcast> response)
        {
            return response.Content.Metadata;
        }

        private static void SetupControllerForTests(ApiController controller, string path)
        {
            var config = new HttpConfiguration();
            var requestedUri = "http://localhost:81/api/podcasts/" + path;
            var request = new HttpRequestMessage(HttpMethod.Get, requestedUri);
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary
                {
                    {"id", Guid.Empty},
                    {"controller", "podcasts"}
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
