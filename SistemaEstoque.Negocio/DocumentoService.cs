using SistemaEstoque.Infra.Entidades;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Negocio.Interfaces;
using SistemaEstoque.Negocio.Notificacões;

namespace SistemaEstoque.Negocio
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IDocumentoRepository _documentoRepository;
        private readonly INotificador _notificador;

        public DocumentoService(IDocumentoRepository documentoRepository,
                                INotificador notificador)
        {
            _documentoRepository = documentoRepository;
            _notificador = notificador;
        }

        public async Task AdicionarDocumento(Documento documento)
        {
            try
            {
                await _documentoRepository.Adicionar(documento);               
            }
            catch (Exception)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Não foi possivel cadastrar documento!"));                
            }
        }

        public async Task<bool> AtualizarDocumento(Documento documento)
        {
            //TODO: verificar se usuario é adm;

            try
            {
                await _documentoRepository.Atualizar(documento);
                return true;
            }
            catch (Exception)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Não foi possível atualizar"));
                return false;
            }
        }

        public async Task ExcluirDocumento(Documento documento)
        {
            try
            {
                await _documentoRepository.Remover(documento);            
            }
            catch (Exception)
            {
                _notificador.AdicionarNotificacao(new Notificacao("Não foi possível excluir"));                
            }
        }

        public async Task<bool> VerificarDisponibilidadeDocumento(string numeroDocumento)
        {
            var documento = await _documentoRepository.ObterPorNumero(numeroDocumento);

            if (documento is not null) 
            {
                _notificador.AdicionarNotificacao(new Notificacao("Documento já está em uso!"));
                return false;
            }

            return true;
        }       
    }
}
