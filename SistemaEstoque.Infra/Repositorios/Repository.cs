using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Contexto;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using System.Linq.Expressions;

namespace SistemaEstoque.Infra.Repositorios
{
    public class Repository<T> : IRepository<T> where T : Entity, new()
    {
        public readonly DbContextEstoque Db;
        public readonly DbSet<T> Dbset;

        public Repository(DbContextEstoque contexto)
        {
            Db = contexto;
            Dbset = contexto.Set<T>();
        }

        public async Task<IEnumerable<T>> Buscar(Expression<Func<T, bool>> predicate)
        {
            return await Dbset.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<T> ObterPorId(Guid id)
        {
            return await Dbset.FindAsync(id);
        }

        public async Task<List<T>> ObterTodos()
        {
            return await Dbset.ToListAsync();
        }

        public async Task Adicionar(T entity)
        {
            Dbset.Add(entity);
            await SaveChanges();
        }

        public async Task Atualizar(T entity)
        {
            Dbset.Update(entity);
            await SaveChanges();
        }

        public async Task Remover(T entity)
        {
            Dbset.Remove(entity);
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
