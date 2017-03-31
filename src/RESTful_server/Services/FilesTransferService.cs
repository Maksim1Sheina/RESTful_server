using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RESTful_server.Services
{
    public class FilesTransferService
    {
        private const string FilesCachePath = "FilesCache";
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly HelperService _helperService;

        public FilesTransferService(IHostingEnvironment hostingEnvironment, HelperService helperService)
        {
            _hostingEnvironment = hostingEnvironment;
            _helperService = helperService;
        }

        public async Task<List<string>> SaveClientFiles(IFormFileCollection UploadFiles, HttpContext httpContext)
        {
            //var sPath = _hostingEnvironment.WebRootPath + "\\" + FilesCachePath + "\\";
            List<string> content = new List<string>();

            foreach (var uploadedFile in UploadFiles)
            {
                var requestUrl = "parseFileContent?";
                var apikey = "d1c4cbae-db61-48c1-928b-cbca5587edbf";
                requestUrl += "apikey=" + apikey + "&fileName=" + uploadedFile.FileName;

                var client = new RestClient("http://api.intellexer.com/" + requestUrl);
                var request = new RestRequest(Method.POST);

                request.AddFileBytes(
                    uploadedFile.Name,
                    _helperService.FileStreamToByteArray(uploadedFile.OpenReadStream(), uploadedFile.Length),
                    uploadedFile.FileName);

                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/octet-stream");

                var taskCompletionSource = new TaskCompletionSource<IRestResponse>();
                client.ExecuteAsync(request, (restResponse) =>
                {
                    if (restResponse.ErrorException != null)
                    {
                        const string message = "Error retrieving response.";
                        throw new RESTful_serverException(message, restResponse.ErrorException);
                    }
                    taskCompletionSource.SetResult(restResponse);
                });

                var response = await taskCompletionSource.Task;

                content.Add(response.Content);
            }

            return content;
        }
    }
}
