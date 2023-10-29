namespace PWManager.Domain.Common
{
    public abstract class Entity
    {
        public string Id { get; init; }
        public DateTimeOffset Created { get; init; }
        public DateTimeOffset Updated { get; set; }

        public Entity() {
            Id = Guid.NewGuid().ToString();
            Created = DateTimeOffset.Now;
            Updated = DateTimeOffset.Now;
        }

        public Entity(string id, DateTimeOffset created, DateTimeOffset updated) {
            Id = id;
            Created = created;
            Updated = updated;
        }
    }
}
