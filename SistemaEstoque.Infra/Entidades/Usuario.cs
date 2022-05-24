using SistemaEstoque.Infra.Entidades.Validações;

namespace SistemaEstoque.Infra.Entidades
{
    public class Usuario : Entity
    {
        public string Nome { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Telefone { get; private set; }
        public Endereco Endereco { get; private set; }
        public bool Ativo { get; set; }
        public ICollection<Produto> Produtos { get; set; }

        //EF Relations
        public Documento Documento { get; private set; }
        public Guid DocumentoId { get; private set; }

        public bool Ativar() => Ativo = true;
        public bool Desativar() => Ativo = false;
        public Usuario() { }
        public Usuario(string nome, Documento documento, 
                        DateTime dataNascimento, bool ativo,
                        string telefone, Endereco endereco)
        {
            Nome = nome;
            Documento = documento;
            DataNascimento = dataNascimento;
            Ativo =  ativo;            
            Telefone = telefone;
            Endereco = endereco;

            Validar();
        }

        private void Validar()
        {
            Validacoes.ValidarSeVazio(Nome, "O Nome não pode ser vázio");
            Validacoes.ValidarSeNulo(DataNascimento, "A DataNascimento não pode ser vázio");
            Validacoes.ValidarSeVazio(Telefone, "O Telefone não pode ser vázio");
            Validacoes.ValidarSeFalso(Ativo, "Usuário não pode ser criado como inativo!");
        }
    }
}
