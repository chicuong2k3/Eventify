namespace EventCatalog.Api.Features.Categories;

public sealed record CreateCategory(string Name) : IRequest<Category>;

internal sealed class CreateCategoryHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCategory, Category>
{
    public async Task<Category> Handle(CreateCategory command, CancellationToken cancellationToken)
    {
        var category = new Category(command.Name);

        categoryRepository.Add(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return category;
    }

}


//internal class CategoryCreatedDomainEventHandler(
//IEventBus eventBus,
//        ISender sender) : DomainEventHandler<CategoryCreatedDomainEvent>
//{
//    public override Task Handle(CategoryCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
//    {
//        return Task.CompletedTask;
//    }
//}