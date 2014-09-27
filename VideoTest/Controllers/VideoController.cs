using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VideoTest.Infrastructure;

namespace VideoTest.Controllers
{
    public class VideoController : ApiController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            var path = HttpContext.Current.Server.MapPath("~/App_Data/" + "BigBuckBunny_640x360.mp4");
            if (!File.Exists(path))
            {
                //  return NotFound();
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            var video = new VideoStream(path);
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent((Func<Stream, HttpContent, TransportContext, Task>)video.WriteToStream, new MediaTypeHeaderValue("video/mp4"));
            return response;
        }
    }
}