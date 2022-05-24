namespace SistemaEstoque.Infra.Entidades
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        public Entity()
        {
            Id = new Guid();
        }
    }
}
