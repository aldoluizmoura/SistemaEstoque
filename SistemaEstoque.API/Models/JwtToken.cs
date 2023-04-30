namespace SistemaEstoque.API.Models
{
    public class JwtToken
    {
        public string Token { get; set; }        
        public DateTime DataExpiracao { get; set; }

        public JwtToken(string token, DateTime dataExpiracao)
        {
            Token = token;
            DataExpiracao = dataExpiracao;
        }
    }
}
