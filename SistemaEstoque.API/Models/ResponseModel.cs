namespace SistemaEstoque.API.Models
{
    public class ResponseModel
    {
        public string Token { get; set; }        
        public object Objeto { get; set; }
        public DateTime DataDeCriacao { get; set; }

        public ResponseModel(string token, object objeto)
        {
            Token = token;
            Objeto = objeto;
            DataDeCriacao = DateTime.UtcNow;
        }
    }
}
