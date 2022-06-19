using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Contexto;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;

namespace SistemaEstoque.Infra.Repositorios
{
    public class DocumentoRepository : Repository<Documento>, IDocumentoRepository
    {
        public DocumentoRepository(DbContextEstoque contexto) : base(contexto) {}

        public async Task<IEnumerable<Documento>> ObterDocumentos() => await Db.Documentos.AsNoTracking().ToListAsync();
        public async Task<Documento> ObterPorNumero(string numeroDocumento) => await Db.Documentos.AsNoTracking().Where(c=>c.Numero == numeroDocumento).FirstOrDefaultAsync();
    }
}
