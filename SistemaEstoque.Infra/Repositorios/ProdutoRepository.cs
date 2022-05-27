using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Contexto;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;

namespace SistemaEstoque.Infra.Repositorios
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(DbContextEstoque contexto) : base(contexto) {}

        public async Task<IEnumerable<Produto>> ObterProdutos() => await Db.Produtos.AsNoTracking().Where(p=>p.Ativo == true).ToListAsync();

        public async Task<IEnumerable<Produto>> ObterProdutosPorCategorias(Guid categoriaId) => await Db.Produtos.AsNoTracking().Where(p => p.CategoriaId == categoriaId).ToListAsync();
    }
}
