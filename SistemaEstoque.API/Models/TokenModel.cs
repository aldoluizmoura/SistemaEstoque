namespace SistemaEstoque.API.Models
{
    public class TokenModel
    {
        public string Token { get; set; }        
        public DateTime DataExpiracao { get; set; }

        public TokenModel(string token, DateTime dataExpiracao)
        {
            Token = token;            
            DataExpiracao = dataExpiracao;
        }
    }
}
