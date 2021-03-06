﻿using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

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
            var response = Request.CreateResponse(HttpStatusCode.OK);
            using (var video = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var buffer = new byte[video.Length];
                video.Read(buffer, 0, buffer.Length);
                response.Content = new ByteArrayContent(buffer);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
            }
            return response;
        }
    }
}