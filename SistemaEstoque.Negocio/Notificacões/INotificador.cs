namespace SistemaEstoque.Negocio.Notificacões
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void AdicionarNotificacao(Notificacao notificacao);
    }
}