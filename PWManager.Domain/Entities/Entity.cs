namespace PWManager.Domain.Entities {
    public abstract class Entity {
        public string Id { get; init; }
        DateTimeOffset Created { get; init; }
        DateTimeOffset Updated { get; set; }
        
    }
}
