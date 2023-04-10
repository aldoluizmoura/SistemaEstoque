using SistemaEstoque.Infra.Entidades;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoque.API.Models
{
    public class FabricanteDTO
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DocumentoDTO Documento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Codigo{ get; set; }

        public Guid Id { get; set; }

        public Guid UsuarioId{ get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }
    }
}
