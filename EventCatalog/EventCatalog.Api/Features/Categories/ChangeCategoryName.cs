namespace EventCatalog.Api.Features.Categories;

public sealed record ChangeCategoryName(
    Guid Id,
    string Name
) : IRequest;

internal sealed class ChangeCategoryNameHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeCategoryName>
{
    public async Task Handle(ChangeCategoryName command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (category == null)
        {
            throw new NotFoundException($"Category with id {command.Id} not found.");
        }

        category.ChangeName(command.Name);
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