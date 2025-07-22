using CodePulse.API.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    // POST: {apibaseUrl}/api/images
    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file,
        [FromForm] string fileName, [FromForm] string title)
    {
        ValidateFileUpload(file);

        if (ModelState.IsValid)
        {
            // File Upload
            var blogImage = new BlogImage
            {
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                FileName = fileName,
                Title = title,
                DateCreated = DateTime.Now
            };

        }
    }

    private void ValidateFileUpload(IFormFile file) {
        var alowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

        if(!alowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower())) { 
            ModelState.AddModelError("file", "Unsupported file format")
        }

        if(file.Length > 10485760)
        {
            ModelState.AddModelError("file", "File size cannot be more than 10MB");
        }
    }
}
