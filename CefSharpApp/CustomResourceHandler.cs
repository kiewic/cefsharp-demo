using System.Diagnostics;
using System.IO;
using CefSharp;
using CefSharp.Callback;

namespace CefSharpApp
{
    internal class CustomResourceHandler : IResourceHandler
    {
        private FileStream fileStream;

        public void Cancel()
        {
            // TODO: Why Cancel is being called when we successfully serve response?
            Debug.WriteLine("Cancel");
        }

        public void Dispose()
        {
            if (this.fileStream != null)
            {
                this.fileStream.Dispose();
            }
        }

        public bool Open(IRequest request, out bool handleRequest, ICallback callback)
        {
            handleRequest = true;
            return true;
        }

        public void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            this.fileStream = File.Open("index.html", FileMode.Open, FileAccess.Read);
            responseLength = this.fileStream.Length;
            redirectUrl = null;

            response.MimeType = "text/html";
            response.StatusCode = 200;
            response.StatusText = "OK";
        }

        public bool ProcessRequest(IRequest request, ICallback callback)
        {
            throw new System.NotImplementedException();
        }

        public bool Read(Stream dataOut, out int bytesRead, IResourceReadCallback callback)
        {
            if (this.fileStream.Position < this.fileStream.Length)
            {
                Debug.Assert(fileStream.Length <= int.MaxValue, "File stream length has to be smaller or equal to int max value");
                fileStream.CopyTo(dataOut);
                bytesRead = (int)fileStream.Length;
                return true;
            }
            bytesRead = 0;
            return false;
        }

        public bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            throw new System.NotImplementedException();
        }

        public bool Skip(long bytesToSkip, out long bytesSkipped, IResourceSkipCallback callback)
        {
            throw new System.NotImplementedException();
        }
    }
}