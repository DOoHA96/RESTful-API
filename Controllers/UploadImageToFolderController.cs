using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace StudentAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/UploadImageToFolder")]
    [ApiController]
    public class UploadImageToFolderController : ControllerBase
    {
        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile is null || imageFile.Length == 0)
                return BadRequest("No File uploaded.");

            var uploadDirectory = @"C:\MyUploads";

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filepath = Path.Combine(uploadDirectory, fileName);

            if(!Directory.Exists(uploadDirectory))
                Directory.CreateDirectory(uploadDirectory);

            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Ok(new{ filepath });
            
        }

        [HttpGet("GetImage/{FileName}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetImage(string FileName)
        {

            var uploadDirectory = @"C:\MyUploads";
            var FilePath = Path.Combine(uploadDirectory, FileName);

            if (!System.IO.File.Exists(FilePath))
                return NotFound("Image Not Found.");

            var Image = System.IO.File.OpenRead(FilePath);
            var mineType = GetMimeType(FilePath);

            return File(Image, mineType);
        }

        public static string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "imgae/gif",
                _ => "application/octet-stream"

            };
        }

    }
}
