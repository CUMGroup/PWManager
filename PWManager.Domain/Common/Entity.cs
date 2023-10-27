namespace PWManager.Domain.Common
{
    public abstract class Entity
    {
        public string Id { get; init; }
        public DateTimeOffset Created { get; init; }
        public DateTimeOffset Updated { get; set; }

    }
}
