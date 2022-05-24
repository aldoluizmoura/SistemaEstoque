namespace SistemaEstoque.Infra.Exceptions
{
    public class EntidadeExcepetions : Exception
    {
        public EntidadeExcepetions()
        {}

        public EntidadeExcepetions(string mensagem) : base(mensagem)
        {}

        public EntidadeExcepetions(string mensagem, Exception innerException) 
                                   : base(mensagem, innerException)
        {}
    }
}
