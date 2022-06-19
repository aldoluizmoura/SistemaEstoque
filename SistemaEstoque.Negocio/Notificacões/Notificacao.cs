namespace SistemaEstoque.Negocio.Notificacões
{
    public class Notificacao
    {
        public string Mensagem{ get; set; }
        public Notificacao(string mensagem)
        {
            Mensagem = mensagem;    
        }
    }
}
