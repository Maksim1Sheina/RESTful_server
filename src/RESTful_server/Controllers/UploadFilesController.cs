using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTful_server.Controllers
{
    [EnableCors("TestPolicy")]
    [Route("api/[controller]")]
    public class UploadFilesController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadFilesController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public string AddFile(IFormFileCollection uploads)
        {
            // Здесь будут лежать полученные файлы.
            IFormFileCollection UploadFiles;

            if (uploads.Count == 0)
                UploadFiles = HttpContext.Request.Form.Files;
            else
                UploadFiles = uploads;

            var wPath = _hostingEnvironment.WebRootPath + "\\FilesCache\\";

            foreach (var uploadedFile in UploadFiles)
            {
                // сохраняем файл в папку в каталоге wwwroot
                using (var fileStream = new FileStream(wPath + uploadedFile.FileName, FileMode.Create))
                {
                    uploadedFile.CopyTo(fileStream);
                }
            }

            return "OK";
        }
    }
}
