using SistemaEstoque.Infra.Entidades;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SistemaEstoque.API.Models
{
    public class FabricanteDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DocumentoDTO Documento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Codigo{ get; set; }

        [JsonIgnore]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid UsuarioId{ get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [JsonIgnore]
        public bool Ativo { get; set; }        
    }
}
