
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Dai.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly GoogleCloudStorageService _storageService;

        public PhotosController(GoogleCloudStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto()
        {
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                using var stream = file.OpenReadStream();
                await _storageService.UploadFileAsync(file.FileName, stream);
                return Ok(new { Message = "File uploaded successfully!" });
            }
            return BadRequest("No file uploaded.");
        }

        [HttpGet("download/{objectName}")]
        public async Task<IActionResult> DownloadPhoto(string objectName)
        {
            var stream = await _storageService.DownloadFileAsync(objectName);
            return File(stream, "image/jpeg");
        }
    }

}
