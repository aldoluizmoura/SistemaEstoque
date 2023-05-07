using SistemaEstoque.Infra.Entidades.Extensions;
using SistemaEstoque.Infra.Entidades.Validações;
using SistemaEstoque.Infra.Exceptions;
using SistemaEstoque.Infra.Interfaces;

namespace SistemaEstoque.Infra.Entidades
{
    public class Produto : Entity, IAggregateRoot
    {
        private string _descricao = string.Empty;
        private string _marca = string.Empty;
        private string _modelo = string.Empty;
        private bool _ativo;

        public string Descricao
        {
            get { return _descricao; }
            set { _descricao = StringExtensions.CapitalizarString(value); }
        }

        public string Marca
        {
            get { return _marca; }
            set { _marca = StringExtensions.CapitalizarString(value); }
        }

        public string Modelo
        {
            get { return _modelo; }
            set { _modelo = StringExtensions.CapitalizarString(value); }
        }

        public int Codigo { get; private set; }
        public double Preco { get; private set; }
        public int QuantidadeEstoque { get; private set; }
       
        public string? Imagem { get; private set; }
        public DateTime? DataVencimento { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public bool Ativo
        {
            get { return _ativo; }
            set { _ativo = true; }
        }

        //ER Relations
        public Fabricante Fabricante { get; private set; }
        public Guid FabricanteId { get; private set; }
        public Categoria Categoria { get; private set; }
        public Guid CategoriaId { get; private set; }
        public Usuario Usuario { get; private set; }
        public Guid UsuarioId { get; private set; }        

        public Produto(){}
        public Produto(string descricao, int codigo, double preco, int quantidadeEstoque, string marca, string modelo,
                       Guid fabricanteId, Guid categoriaId, DateTime? dataVencimento,string imagem, Guid usuarioId)
        {
            Descricao = descricao;
            Codigo = codigo;
            Preco = preco;
            QuantidadeEstoque = quantidadeEstoque;
            Marca = marca;
            Modelo = modelo;
            FabricanteId = fabricanteId;
            CategoriaId = categoriaId;
            Imagem = imagem;            
            DataVencimento = dataVencimento;
            DataCriacao = DateTime.UtcNow;
            UsuarioId = usuarioId;
            Ativo = true;

            Validar();
        }

        public bool Ativar() => Ativo = true;

        public bool Desativar() => Ativo = false;

        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Descricao, "O campo Nome não pode ser vázio");
            Validacoes.ValidarSeVazio(Modelo, "O campo Nome não pode ser vázio");
            Validacoes.ValidarSeVazio(Marca, "O campo Nome não pode ser vázio");            
        }

        public void AlterarDescricao(string descricao)
        {
            Validacoes.ValidarSeVazio(descricao, "O campo Descrição não pode estar vazio");
            Descricao = descricao;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;

            if (!PossuiEstoque(quantidade)) 
                throw new EntidadeExcepetions("Estoque insuficiente");

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
