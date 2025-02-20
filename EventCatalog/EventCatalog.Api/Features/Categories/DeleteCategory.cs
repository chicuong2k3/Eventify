namespace EventCatalog.Api.Features.Categories;

public sealed record DeleteCategory(Guid Id) : IRequest;

internal sealed class DeleteCategoryHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCategory>
{
    public async Task Handle(DeleteCategory command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (category == null)
        {
            throw new NotFoundException($"Category with id {command.Id} not found.");
        }

        categoryRepository.Remove(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

}

//internal class CategoryNameChangedDomainEventHandler(
//        IEventBus eventBus,
//        ISender sender) : DomainEventHandler<CategoryNameChangedDomainEvent>
//{
//    public override Task Handle(CategoryNameChangedDomainEvent domainEvent, CancellationToken cancellationToken)
//    {
//        return Task.CompletedTask;
//    }
//}