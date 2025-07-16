using Azure.Core;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BlogPostsController : ControllerBase
{
    private readonly IBlogPostRepository blogPostRepository;
    private readonly ICategoryRepository categoryRepository;
    public BlogPostsController(IBlogPostRepository blogPostRepository, 
        ICategoryRepository categoryRepository)
    {
        this.blogPostRepository = blogPostRepository;
        this.categoryRepository = categoryRepository;
    }

    // POST: {apibaseurl}/api/blogposts
    [HttpPost]
    public async Task<IActionResult> CreateBlogPost([FromBody]CreateBlogPostRequestDto request)
    {
        // Convert Dto to Domain Model
        var blogPost = new BlogPost
        {
            Author = request.Author,
            Content = request.Content,
            FeaturedImageUrl = request.FeaturedImageUrl,
            IsVisible = request.IsVisible,
            PublishedDate = request.PublishedDate,
            ShortDescription = request.ShortDescription,
            Title = request.Title,
            UrlHandle = request.UrlHandle,
            Categories = new List<Category>()
        };

        foreach (var categoryGuid in request.Categories) {
            var existingCategory = await categoryRepository.GetById(categoryGuid);

            if (existingCategory != null) { 
                blogPost.Categories.Add(existingCategory);
            }
        }

        blogPost = await blogPostRepository.CreateAsync(blogPost);

        // Convert Domain Model to Dto
        var response = new BlogPostDto
        {
            Id = blogPost.Id,
            Author = request.Author,
            Content = request.Content,
            FeaturedImageUrl = request.FeaturedImageUrl,
            IsVisible = request.IsVisible,
            PublishedDate = request.PublishedDate,
            ShortDescription = request.ShortDescription,
            Title = request.Title,
            UrlHandle = request.UrlHandle,
            Categories = blogPost.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle
            }).ToList()
        };

        return Ok(response);

    }

    // GET: {apibaseurl}/api/blogposts
    [HttpGet]
    public async Task<IActionResult> GetAllBlogPosts()
    {
        var blogPosts = await blogPostRepository.GetAllAsync();

        // Convert Domain Model to Dto
        var reponse = new List<BlogPostDto>();
        foreach (var blogPost in blogPosts)
        {
            reponse.Add(new BlogPostDto
            {
                Id = blogPost.Id,
                Author = blogPost.Author,
                Content = blogPost.Content,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                IsVisible = blogPost.IsVisible,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                Title = blogPost.Title,
                UrlHandle = blogPost.UrlHandle,
                Categories = blogPost.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle
                }).ToList()
            });
        }
        return Ok(reponse);
    }


    // GET: {apibaseUrl}/api/blogposts/{id}
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetBlogPostById([FromRoute]Guid id)
    {
        // get the BlogPost from repo
        var blogPost = await blogPostRepository.GetByIdAsync(id);

        if(blogPost == null)
        {
            return NotFound();
        }

        // Convert Domain Model to DTO
        var response = new BlogPostDto
        {
            Id = blogPost.Id,
            Author = blogPost.Author,
            Content = blogPost.Content,
            FeaturedImageUrl = blogPost.FeaturedImageUrl,
            IsVisible = blogPost.IsVisible,
            PublishedDate = blogPost.PublishedDate,
            ShortDescription = blogPost.ShortDescription,
            Title = blogPost.Title,
            UrlHandle = blogPost.UrlHandle,
            Categories = blogPost.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle
            }).ToList()
        };

        return Ok(response);

    }


    //PUT: {apibaseUrl}/api/blogposts/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> UpdateBlogPostById([FromRoute] Guid id, UpdateBlogPostRequestDto request)
    {
        // Convert Dto to Doamin Model
        var blogPost = new BlogPost
        {
            Id = id,
            Author = request.Author,
            Content = request.Content,
            FeaturedImageUrl = request.FeaturedImageUrl,
            IsVisible = request.IsVisible,
            PublishedDate = request.PublishedDate,
            ShortDescription = request.ShortDescription,
            Title = request.Title,
            UrlHandle = request.UrlHandle,
            Categories = new List<Category>()
        };

        // Foreach
        foreach (var categoryGuid in request.Categories)
        {
            var existingCategory = await categoryRepository.GetById(categoryGuid);
            if (existingCategory != null)
            {
                blogPost.Categories.Add(existingCategory);
            }
        }

        // Call repository to update BlogPost domain model
        var updatedBlogPost = await blogPostRepository.UpdateAsync(blogPost);
        
        if(updatedBlogPost == null)
        {
            return NotFound();
        }

        // Convert domain model to dto
        var response = new BlogPostDto
        {
            Id = blogPost.Id,
            Author = blogPost.Author,
            Content = blogPost.Content,
            FeaturedImageUrl = blogPost.FeaturedImageUrl,
            IsVisible = blogPost.IsVisible,
            PublishedDate = blogPost.PublishedDate,
            ShortDescription = blogPost.ShortDescription,
            Title = blogPost.Title,
            UrlHandle = blogPost.UrlHandle,
            Categories = blogPost.Categories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                UrlHandle = x.UrlHandle
            }).ToList()
        };
        return Ok(response);
    }

    //DELETE: {apibaseUrl}/api/blogposts/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
    {
        var deletedBlogPost = await blogPostRepository.DeleteAsync(id);
        if(deletedBlogPost is null)
        {
            return NotFound();
        }

        // convert Domain Model to Dto
        var response = new BlogPostDto
        {
            Id = deletedBlogPost.Id,
            Author = deletedBlogPost.Author,
            Content = deletedBlogPost.Content,
            FeaturedImageUrl = deletedBlogPost.FeaturedImageUrl,
            IsVisible = deletedBlogPost.IsVisible,
            PublishedDate = deletedBlogPost.PublishedDate,
            ShortDescription = deletedBlogPost.ShortDescription,
            Title = deletedBlogPost.Title,
            UrlHandle = deletedBlogPost.UrlHandle
        };

        return Ok(response);
    }

}
