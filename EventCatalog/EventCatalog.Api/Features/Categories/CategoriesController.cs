using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace EventCatalog.Api.Features.Categories;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{apiVersion}/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator mediator;

    public CategoriesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var response = await mediator.Send(new GetCategories());
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var category = await mediator.Send(new GetCategoryById(id));

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategory command)
    {
        var category = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id, apiVersion = "1.0" }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ChangeCategoryName(Guid id, [FromBody] ChangeCategoryNameRequest request)
    {
        var command = new ChangeCategoryName(id, request.NewName);
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await mediator.Send(new DeleteCategory(id));
        return NoContent();
    }

    [HttpPost("{id}/archive")]
    public async Task<IActionResult> ArchiveCategory(Guid id)
    {
        await mediator.Send(new ArchiveCategory(id));
        return NoContent();
    }
}

public record ChangeCategoryNameRequest(string NewName);