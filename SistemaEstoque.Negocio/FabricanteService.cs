using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using SistemaEstoque.Negocio.Notificacões;

namespace SistemaEstoque.Negocio
{
    public class FabricanteService : IFabricanteService
    {
        public IFabricanteRepository _fabricanteRepository;
        private readonly INotificador _notificador;

        public FabricanteService(IFabricanteRepository fabricanteRepository, INotificador notificador)
        {
            _fabricanteRepository = fabricanteRepository;
            _notificador = notificador;
        }

        public async Task AdicionarFabricante(Fabricante fabricante)
        {
            try
            {
                await _fabricanteRepository.Adicionar(fabricante);
            }
            catch (Exception ex)
            {
                _notificador.AdicionarNotificacao(new Notificacao($"Não foi possivel cadastrar documento! {ex.Message}"));
            }
        }

        public async Task AtualizarFabricante(Fabricante fabricante)
        {
            try
            {
                await _fabricanteRepository.Atualizar(fabricante);
            }
            catch (Exception ex)
            {
                _notificador.AdicionarNotificacao(new Notificacao($"Não foi possível atualizar {ex.Message}"));
            }
        }

        public async Task MudarStatusFabricante(Fabricante fabricante)
        {
            fabricante.Ativo = fabricante.Ativo ? fabricante.Desativar() : fabricante.Ativar();
           
            await AtualizarFabricante(fabricante);
        }

        public async Task AlterarDocumentoFabricante(Guid fabricanteId, Guid documentoId)
        {
            var fabricante = await _fabricanteRepository.ObterPorId(fabricanteId);
            fabricante.TrocarDocumentoFabricante(documentoId);
        }
    }
}
