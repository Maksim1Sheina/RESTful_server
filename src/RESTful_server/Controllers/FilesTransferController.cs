using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using RESTful_server.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            _service = new FilesTransferService(hostingEnvironment, new HelperService());
        }

        [HttpPost]
        public async Task<List<string>> UploadFromClient(IFormFileCollection uploads)
        {
            List<string> result;

            if (uploads.Count == 0)
                result = await _service.SaveClientFiles(HttpContext.Request.Form.Files, HttpContext);
            else
                result = await _service.SaveClientFiles(uploads, HttpContext);

            return result;
        }
    }
}
