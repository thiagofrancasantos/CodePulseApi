using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageRepository imageRepository;
    public ImagesController(IImageRepository imageRepository)
    {
        this.imageRepository = imageRepository;
    }

    // GET: {apibaseUrl}/api/images
    [HttpGet]
    public async Task<IActionResult> GetAllImages()
    {
        // call image repository to get all images
        var images = await imageRepository.GetAll();

        // Convert Domain Model to Dto
        var response = new List<BlogImageDto>();
        foreach (var image in images)
        {
            response.Add(new BlogImageDto
            {
                Id = image.Id,
                Title = image.Title,
                DateCreated = image.DateCreated,
                FileExtension = image.FileExtension,
                FileName = image.FileName,
                Url = image.Url
            });
        }

        return Ok(response);
    }

    // POST: {apibaseUrl}/api/images
    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageRequestDto request)
    {
        ValidateFileUpload(request.File);

        if (ModelState.IsValid)
        {
            var blogImage = new BlogImage
            {
                FileExtension = Path.GetExtension(request.File.FileName).ToLower(),
                FileName = request.FileName,
                Title = request.Title,
                DateCreated = DateTime.Now
            };

            blogImage = await imageRepository.Upload(request.File, blogImage);

            var response = new BlogImageDto
            {
                Id = blogImage.Id,
                Title = blogImage.Title,
                DateCreated = blogImage.DateCreated,
                FileExtension = blogImage.FileExtension,
                FileName = blogImage.FileName,
                Url = blogImage.Url
            };

            return Ok(response);
        }

        return BadRequest(ModelState);
    }


    private void ValidateFileUpload(IFormFile file) {
        var alowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

        if(!alowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower())) {
            ModelState.AddModelError("file", "Unsupported file format");
        }

        if(file.Length > 10485760)
        {
            ModelState.AddModelError("file", "File size cannot be more than 10MB");
        }
    }
}
