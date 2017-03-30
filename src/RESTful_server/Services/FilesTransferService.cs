using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System;
using System.IO;

namespace RESTful_server.Services
{
    public class FilesTransferService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public FilesTransferService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string VirtualUrl(string relativeUrl, HttpContext httpContext)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (!relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "/");

            var request = httpContext.Request;
            var port = request.Host.Port != 80 ? (":" + request.Host.Port) : String.Empty;

            return $"{request.Scheme}://{request.Host.Host}{port}{ relativeUrl }";
        }

        public void SaveClientFiles(IFormFileCollection UploadFiles, HttpContext httpContext)
        {
            var sPath = _hostingEnvironment.ContentRootPath + "\\FilesCache\\";

            foreach (var uploadedFile in UploadFiles)
            {
                // сохраняем файл в папку в каталоге wwwroot
                using (var fileStream = new FileStream(sPath + uploadedFile.FileName, FileMode.Create))
                {
                    var requestUrl = "parseFileContent?";
                    var apikey = "d1c4cbae-db61-48c1-928b-cbca5587edbf";
                    var fileUrl = VirtualUrl("FilesCache/" + uploadedFile.FileName, httpContext);
                    requestUrl += "apikey=" + apikey + "&fileName=" + fileUrl;

                    /*var client = new RestClient("http://api.intellexer.com");
                    var request = new RestRequest(requestUrl,Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "multipart/form-data; boundary=---011000010111000001101001");
                    request.AddParameter("multipart/form-data; boundary=---011000010111000001101001", "-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"fileName\"; filename=\"[object Object]\"\r\nContent-Type: false\r\n\r\n\r\n-----011000010111000001101001--", ParameterType.RequestBody);
                    IRestResponse response;
                    var asyncHandle = client.ExecuteAsync(request, result =>
                    {
                        response = result;
                    });*/

                    var client = new RestClient("http://api.intellexer.com/" + requestUrl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "multipart/form-data; boundary=---011000010111000001101001");
                    request.AddParameter("multipart/form-data; boundary=---011000010111000001101001", "-----011000010111000001101001\r\nContent-Disposition: form-data; name=\"fileName\"; filename=\"[object Object]\"\r\nContent-Type: false\r\n\r\n\r\n-----011000010111000001101001--", ParameterType.RequestBody);
                    IRestResponse response;
                    client.ExecuteAsync(request, result =>
                    {
                        response = result;
                    });

                    uploadedFile.CopyTo(fileStream);
                }
            }
        }
    }
}
