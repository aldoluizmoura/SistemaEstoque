using SistemaEstoque.Infra.Entidades.Validações;
using SistemaEstoque.Infra.Exceptions;
using SistemaEstoque.Infra.Interfaces;

namespace SistemaEstoque.Infra.Entidades
{
    public class Produto : Entity, IAggregateRoot
    {   
        public string Descricao { get; private set; }
        public double Preco { get; private set; }
        public int QuantidadeEstoque { get; private set; }
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public string? Imagem { get; private set; }
        public DateTime? DataVencimento { get; private set; }
        public DateTime DataCadastro { get; private set; }

        //ER Relations
        public Fabricante Fabricante { get; private set; }
        public Guid FabricanteId { get; private set; }
        public Categoria Categoria { get; private set; }
        public Guid CategoriaId { get; private set; }
        public Usuario Usuario { get; private set; }
        public Guid UsuarioId { get; private set; }
        public bool Ativo { get; private set; }

        public Produto(){}
        public Produto(string descricao, double preco, int quantidadeEstoque, string marca, string modelo,
                       Guid fabricanteId, Guid categoriaId, DateTime? dataVencimento, 
                       string imagem, Guid usuarioId, bool activo)
        {
            Descricao = descricao;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
            Marca = marca;
            Modelo = modelo;
            FabricanteId = fabricanteId;
            CategoriaId = categoriaId;
            Imagem = imagem;            
            DataVencimento = dataVencimento;
            UsuarioId = usuarioId;
            Ativo = activo;
        }

        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
        }

        public void AlterarDescricao(string descricao)
        {
            Validacoes.ValidarSeVazio(descricao, "O campo Descrição não pode estar vazio");
            Descricao = descricao;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;

            if (!PossuiEstoque(quantidade)) throw new EntidadeExcepetions("Estoque insuficiente");
            QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoque(int quantidade)
        {
            QuantidadeEstoque += quantidade;
        }

        public bool PossuiEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }
    }
}
