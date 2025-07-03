using CodePulse.API.Data;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
  private readonly ICategoryRepository categoryRepository;

  public CategoriesController(ICategoryRepository categoryRepository)
  {
    this.categoryRepository = categoryRepository;
  }

  //
  [HttpPost]
  public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
  {
    // Map DTO to Domain Model

    var category = new Category
    {
      Name = request.Name,
      UrlHandle = request.UrlHandle
    };

    await categoryRepository.CreateAsync(category);


    //Domain Model to DTo
    var response = new CategoryDto
    {
      Id = category.Id,
      Name = category.Name,
      UrlHandle = category.UrlHandle
    };

    return Ok(response);

  }

    // GET: http://localhost:5261/api/Categories
    [HttpGet]
  public async Task<IActionResult> GetAllCategory(){
    var categories = await categoryRepository.GetAllAsync();

        // Map domain model to DTO

    var response = new List<CategoryDto>();
    foreach (var category in categories) {
            response.Add(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            });  
    }

    return Ok(response);
        

  }

    //GET: http://localhost:5261/api/categories/{id}
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetCategoryById([FromRoute]Guid id)    {
        var existingCategory = await categoryRepository.GetById(id);

        if (existingCategory is null) {
            return NotFound();
        }

        var response = new CategoryDto
        {
            Id = existingCategory.Id,
            Name = existingCategory.Name,
            UrlHandle = existingCategory.UrlHandle
        };

        return Ok(response);
    }

    //PUT: http://localhost:5261/api/categories/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> EditCategory([FromRoute]Guid id, UpdateCategoryRequestDto request)
    {

        // Convert Dto to Domain Model
        var category = new Category
        {
            Id = id,
            Name = request.Name,
            UrlHandle = request.UrlHandle
        };

        category = await categoryRepository.UpdateAsync(category);

        if(category == null)
        {
            return NotFound();
        }

        // Convert Domain Model to Dto 
        var response = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle
        };

        return Ok(response);

    }

    // DELETE: http://localhost:5261/api/categories/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteCategory([FromRoute]Guid id)
    {
        var category = await categoryRepository.DeleteAsync(id);

        if(category is null)
        {
            return NotFound();
        }

        // Convert Domain Model to DTO
        var response = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            UrlHandle = category.UrlHandle
        };

        return Ok(response);
    }
}
