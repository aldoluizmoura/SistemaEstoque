using SistemaEstoque.Infra.Entidades;
using System.ComponentModel.DataAnnotations;

namespace SistemaEstoque.API.Models
{
    public class DocumentoDTO
    {
        [Required(ErrorMessage = "o campo {0} é obrigatorio")]
        public string Numero { get; set; }

        public DocumentoDTO(string numero)
        {
           Numero = numero;           
        }
    }
}
