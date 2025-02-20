namespace EventCatalog.Domain.CategoryAggregate;

public sealed class Category : Entity
{
    private Category()
    {

    }

    public string Name { get; private set; }
    public bool IsArchived { get; private set; }

    public Category(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ValidationException("Category name is required.");

        Id = Guid.NewGuid();
        Name = name;
        IsArchived = false;

        Raise(new CategoryCreated(Id));
    }
    public void Archive()
    {
        if (!IsArchived)
        {
            IsArchived = true;

            Raise(new CategoryArchived(Id));
        }
    }

    public void ChangeName(string name)
    {
        if (Name != name)
        {
            Name = name;

            Raise(new CategoryNameChanged(Id, Name));
        }
    }
}
