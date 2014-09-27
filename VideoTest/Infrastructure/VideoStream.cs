using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace VideoTest.Infrastructure
{
    public class VideoStream
    {
        private readonly string _path;

        public VideoStream(string path)
        {
            _path = path;
        }

        public async Task WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {
                var buffer = new byte[65536];

                using (var video = File.Open(_path, FileMode.Open, FileAccess.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        length -= bytesRead;
                    }
                }
            }
            catch (HttpException)
            {
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}