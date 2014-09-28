using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VideoTest.Infrastructure;

namespace VideoTest.Controllers
{
    public class InstantRangeVideoController : ApiController
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
            HttpResponseMessage response;
            VideoStream video;
            if (Request.Headers.Range != null)
            {
                var firstRange = Request.Headers.Range.Ranges.FirstOrDefault();
                var start = firstRange == null ? 0 : firstRange.From == null ? 0 : firstRange.From.Value;
                video = new VideoStream(path, start);
                response = Request.CreateResponse(HttpStatusCode.PartialContent);
            }
            else
            {
                video = new VideoStream(path);
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            response.Content = new PushStreamContent((Func<Stream, HttpContent, TransportContext, Task>)video.WriteToStream, new MediaTypeHeaderValue("video/mp4"));
            return response;
        }
    }
}