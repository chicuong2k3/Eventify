namespace EventCatalog.Api.Features.Categories;

public sealed record GetCategoryById(Guid Id) : IRequest<Category>;

internal sealed class GetCategoryByIdHandler(
    ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategoryById, Category>
{
    public async Task<Category> Handle(GetCategoryById query, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(query.Id);
        if (category == null)
        {
            throw new NotFoundException($"Category with id {query.Id} not found.");
        }

        return category;
    }
}