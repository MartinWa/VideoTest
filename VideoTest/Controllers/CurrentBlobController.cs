using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.WindowsAzure.Storage;
using VideoTest.Infrastructure;

namespace VideoTest.Controllers
{
    public class CurrentBlobController : ApiController
    {
        public IHttpActionResult Get()
        {
            var storageAccount = CloudStorageAccount.Parse(Constants.ConnectionString);
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference(Constants.ContainerName.Trim('/'));
            var blob = container.GetBlockBlobReference(Constants.FileName);
            if (!blob.Exists())
            {
                return NotFound();
            }
            if (blob.Properties.ETag == Request.Headers.IfNoneMatch.ToString())
            {
                return new StatusCodeResult(HttpStatusCode.NotModified, Request);
            }
            return new ContentResult(Request, blob.OpenRead(), blob.Properties.ContentType, blob.Properties.ETag);
        }


    }
}
