using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using VideoTest.Infrastructure;

namespace VideoTest.Controllers
{
    public class SasController : ApiController
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
            var sasConstraints = new SharedAccessBlobPolicy
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(4),
                Permissions = SharedAccessBlobPermissions.Read
            };

            var sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);
            var uri = blob.Uri.AbsoluteUri + sasBlobToken;
            return Redirect(uri);
        }

    }
}
