using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;

namespace SistemaEstoque.Negocio
{
    public class CategoriaService : ICategoriaService
    {
        public ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<bool> AdicionarCategoria(Categoria categoria)
        {
            try
            {
                await _categoriaRepository.Adicionar(categoria);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> AtualizarCategoria(Categoria categoria)
        {
            try
            {
                if (await _categoriaRepository.ObterPorId(categoria.Id) is not null)
                {
                    await _categoriaRepository.Atualizar(categoria);
                    return true;
                }

                return false;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
