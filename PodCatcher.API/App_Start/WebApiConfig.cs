using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using PodCatcher.API.Models;

namespace PodCatcher.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //CORS
            var cors = new EnableCorsAttribute("*", "*", "*"); //podcatcher.azurewebsites.net
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Web API configuration and services
            var jsonFormatter = new JsonMediaTypeFormatter();
            //optional: set serializer settings here
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

        }
    }
}
