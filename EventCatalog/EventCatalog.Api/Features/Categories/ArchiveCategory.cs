namespace EventCatalog.Api.Features.Categories;

public sealed record ArchiveCategory(Guid Id) : IRequest;


internal sealed class ArchiveCategoryHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ArchiveCategory>
{
    public async Task Handle(ArchiveCategory command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (category == null)
        {
            throw new NotFoundException($"Category with id {command.Id} not found.");
        }

        category.Archive();
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

}


//internal class CategoryArchivedDomainEventHandler(
//IEventBus eventBus,
//        ISender sender) : DomainEventHandler<CategoryArchivedDomainEvent>
//{
//    public override Task Handle(CategoryArchivedDomainEvent domainEvent, CancellationToken cancellationToken)
//    {
//        return Task.CompletedTask;
//    }
//}