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

        public async Task<bool> AdicionarFabricante(Fabricante fabricante)
        {
            try
            {
                await _fabricanteRepository.Adicionar(fabricante);
                return true;
            }
            catch (Exception)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Não foi possivel cadastrar documento!"));
                return false;
            }
        }

        public async Task<bool> AtualizarFabricante(Fabricante fabricante)
        {
            try
            {
                await _fabricanteRepository.Atualizar(fabricante);
                return true;
            }
            catch (Exception)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Não foi possível atualizar"));
                return false;
            }
        }

        public async Task MudarStatusFabricante(Fabricante fabricante)
        {

            if (fabricante.Ativo)
            {
                fabricante.Desativar();               
            }
            else
            {
                fabricante.Ativar();
            }
           
            await AtualizarFabricante(fabricante);
        }

        public async Task AlterarDocumentoFabricante(Guid fabricanteId, Guid documentoId)
        {
            var fabricante = await _fabricanteRepository.ObterPorId(fabricanteId);
            fabricante.TrocarDocumentoFabricante(documentoId);
        }
    }
}
