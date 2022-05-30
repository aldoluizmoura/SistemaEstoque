using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Contexto;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;

namespace SistemaEstoque.Infra.Repositorios
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(DbContextEstoque contexto) : base(contexto) {}

        public async Task<IEnumerable<Categoria>> ObterCategorias() => await Db.Categorias.AsNoTracking().ToListAsync();
        public async Task<Categoria> ObterPorNome(string nomeCategoria) => await Db.Categorias.AsNoTracking().Where(c=>c.Nome == nomeCategoria).FirstOrDefaultAsync();
    }
}
