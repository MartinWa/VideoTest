using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace VideoTest.Infrastructure
{
    public class ContentResult : IHttpActionResult
    {
        readonly HttpRequestMessage _request;
        private readonly Stream _content;
        private readonly string _contentType;
        private readonly string _etag;

        public ContentResult(HttpRequestMessage request, Stream content, string contentType, string etag = null)
        {
            _request = request;
            _content = content;
            _contentType = contentType;
            _etag = etag;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(_content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(_contentType);
            if (!string.IsNullOrEmpty(_etag))
            {
                response.Headers.ETag = new EntityTagHeaderValue(_etag);
            }
            return Task.FromResult(response);
        }
    }
}