using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfoAPI.Controllers
{
    [Route("api/v{version:apiVersion}/file")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
        }

        [HttpGet("{fileName}")]
        [ApiVersion(0.1,Deprecated = true)]
        public ActionResult GetFile(string fileName)
        {
            var filePath = "sample.pdf";
            if(!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            if(_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var file = System.IO.File.ReadAllBytes(filePath);
            return File(file,contentType,Path.GetFileName(filePath));

        }


        [HttpPost]
        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            if(file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("No file or invalid input was uploaded");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory() + $"uploaded_file_{Guid.NewGuid()}.pdf");

            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("Your file has been successfully uploaded.");
        }
    }
}
