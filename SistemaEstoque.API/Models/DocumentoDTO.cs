using System.ComponentModel.DataAnnotations;

namespace SistemaEstoque.API.Models
{
    public class DocumentoDTO
    {
        [Required(ErrorMessage = "o campo {0} é obrigatorio")]
        public string NumeroDocumento { get; set; }

        public DocumentoDTO(string numeroDocumento)
        {
           NumeroDocumento = numeroDocumento;           
        }
    }
}
