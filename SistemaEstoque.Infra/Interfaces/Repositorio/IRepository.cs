using SistemaEstoque.Infra.Entidades;
using System.Linq.Expressions;

namespace SistemaEstoque.Infra.Interfaces.Repositorio
{
    public interface IRepository<T> : IDisposable where T : Entity 
    {
        Task Adicionar(T entity);
        Task<T> ObterPorId(Guid id);
        Task<List<T>> ObterTodos();
        Task Atualizar(T entity);
        Task Remover(T entity);
        Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate);
    }
}
