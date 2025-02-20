namespace EventCatalog.Api.Features.Categories;

public record GetCategories : IRequest<IReadOnlyCollection<Category>>;

internal sealed class GetCategoriesHandler(
    ICategoryRepository categoryRepository)
    : IRequestHandler<GetCategories, IReadOnlyCollection<Category>>
{
    public async Task<IReadOnlyCollection<Category>> Handle(GetCategories query, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllAsync(cancellationToken);

        return categories;
    }
}
