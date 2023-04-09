using System;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoque.API.Models
{
    public class ProdutoDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid FabricanteId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid CategoriaId{ get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public double Preco { get; set; }
        public string Imagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public bool Ativo { get; set; }
        public DateTime DataVencimento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime DataCadastro { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int QuantidadeEstoque { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Modelo { get; set; }
    }
}
