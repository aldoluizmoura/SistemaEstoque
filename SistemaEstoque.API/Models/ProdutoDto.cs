using Newtonsoft.Json.Linq;
using SistemaEstoque.Infra.Entidades;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace SistemaEstoque.API.Models
{
    public class ProdutoDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid FabricanteId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Codigo { get; set; }

        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]        
        public Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid CategoriaId{ get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public double Preco { get; set; }
        public string Imagem { get; set; } //ajustar isso

        [JsonIgnore]
        public bool Ativo { get; set; }
        public DateTime DataVencimento { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} precisa ter o valor mínimo de {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int QuantidadeEstoque { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Modelo { get; set; } = string.Empty;        
    }
}
