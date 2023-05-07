using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Contexto;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;

namespace SistemaEstoque.Infra.Repositorios
{
    public class FabricanteRepository : Repository<Fabricante>, IFabricanteRepository
    {
        public FabricanteRepository(DbContextEstoque contexto) : base(contexto) { }
        public async Task<IEnumerable<Fabricante>> ObterFabricantes() => await Db.Fabricantes.Include(f=>f.Documento).AsNoTracking().ToListAsync();
        public async Task<Fabricante> ObterPorNome(string nomeFabricante) => await Db.Fabricantes.AsNoTracking().Where(c => c.Nome == nomeFabricante).FirstOrDefaultAsync();
        public async Task<Fabricante> ObterPorId(Guid fabricanteId) => await Db.Fabricantes.Include(f => f.Documento).AsNoTracking().Where(c => c.Id == fabricanteId).FirstOrDefaultAsync();
    }
}
