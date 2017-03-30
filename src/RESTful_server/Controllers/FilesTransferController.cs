using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using RESTful_server.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTful_server.Controllers
{
    [EnableCors("TestPolicy")]
    [Route("api/[controller]")]
    public class FilesTransferController : Controller
    {
        private readonly FilesTransferService _service;

        public FilesTransferController(IHostingEnvironment hostingEnvironment)
        {
            _service = new FilesTransferService(hostingEnvironment);
        }

        [HttpPost]
        public string UploadFromClient(IFormFileCollection uploads)
        {
            if (uploads.Count == 0)
                _service.SaveClientFiles(HttpContext.Request.Form.Files, HttpContext);
            else
                _service.SaveClientFiles(uploads, HttpContext);

            return "OK";
        }
    }
}
