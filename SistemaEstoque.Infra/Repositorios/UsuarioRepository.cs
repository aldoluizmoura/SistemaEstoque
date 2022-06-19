using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Contexto;
using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;

namespace SistemaEstoque.Infra.Repositorios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public readonly DbContextEstoque Db;
        public readonly DbSet<Usuario> Dbset;

        public UsuarioRepository(DbContextEstoque contexto)
        {
            Db = contexto;
            Dbset = contexto.Set<Usuario>();
        }

        public async Task AtualizarUsuario(Usuario usuario)
        {
            Dbset.Update(usuario);
            await Db.SaveChangesAsync();
        }

        public async Task<Usuario> ObterPorDocumento(string documento)
        {
            return await Dbset.AsNoTracking().Where(u => u.Documento.Numero == documento).FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObterPorEmail(string emailUsuario)
        {
            return await Dbset.AsNoTracking().Where(u => u.Email == emailUsuario).FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObterPorId(Guid usuarioId)
        {
            return await Dbset.AsNoTracking().Where(u => u.UsuarioId == usuarioId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Usuario>> ObterPorNome(string nomeUsuario)
        {
            return await Dbset.AsNoTracking().Where(u => u.Nome == nomeUsuario).ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> ObterUsuarios()
        {
            return await Dbset.AsNoTracking().ToListAsync();
        }
    }
}
