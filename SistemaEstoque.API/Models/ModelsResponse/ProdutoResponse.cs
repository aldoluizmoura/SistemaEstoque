using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.Models.ModelsResponse
{
    public class ProdutoResponse
    {
        public string Descricao { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public int Codigo { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataVencimento { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string NomeCategoria { get; set; }
        public int QuantidadeEstoque { get; set; }


        public ProdutoResponse(string descricao, int codigo, bool ativo, DateTime? dataVencimento, string marca, string modelo, string nomeCategoria, int quantidadeEstoque, Guid id)
        {
            Descricao = descricao;
            Codigo = codigo;
            Ativo = ativo;
            DataVencimento = dataVencimento;
            Marca = marca;
            Modelo = modelo;
            NomeCategoria = nomeCategoria;
            QuantidadeEstoque = quantidadeEstoque;
            Id = id;
        }
    }
}
