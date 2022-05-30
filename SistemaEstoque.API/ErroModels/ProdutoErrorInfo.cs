namespace SistemaEstoque.API.ErroModels
{
    public class ProdutoErrorInfo
    {
        public int QuantidadeEstoque { get; set; }
        public int QuantidadeSolicitada { get; set; }

        public ProdutoErrorInfo(int quantidadeEstoque, int quantidadeSolicitada)
        {
            QuantidadeEstoque = quantidadeEstoque;
            QuantidadeSolicitada = quantidadeSolicitada;
        }
    }
}
