using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Contexto;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;

namespace SistemaEstoque.Infra.Repositorios
{
    public class FabricanteRepository : Repository<Fabricante>, IFabricanteRepository
    {
        public FabricanteRepository(DbContextEstoque contexto) : base(contexto) { }
        public async Task<IEnumerable<Fabricante>> ObterFabricantes() => await Db.Fabricantes.AsNoTracking().ToListAsync();
        public async Task<Fabricante> ObterPorNome(string nomeFabricante) => await Db.Fabricantes.AsNoTracking().Where(c => c.Nome == nomeFabricante).FirstOrDefaultAsync();
    }
}
