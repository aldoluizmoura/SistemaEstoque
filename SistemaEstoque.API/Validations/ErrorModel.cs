using SistemaEstoque.Negocio.Notificacões;

namespace SistemaEstoque.API.Validations
{
    public class ErrorModel
    {
        public ErrorModel(IList<Notificacao> notificacoes)
        {
            ErrosNotificacoes = notificacoes;
        }

        public IList<Notificacao> ErrosNotificacoes { get; set; }   
    }
}

