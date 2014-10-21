using System.Net.Http.Formatting;
using System.Web.Http;

namespace VideoTest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.Formatters.Clear();  // Remove default formatters (XML) because we dont test them
            config.Formatters.Add(new JsonMediaTypeFormatter()); // Add JSON formatter
        }
    }
}