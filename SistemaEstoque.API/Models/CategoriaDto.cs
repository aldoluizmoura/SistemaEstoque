using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SistemaEstoque.API.Models
{
    public class CategoriaDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public int Codigo { get; set; }

        public CategoriaDto()
        {
            Nome = CultureInfo.GetCultureInfo("pt-BR").TextInfo.ToTitleCase(Nome); ;
        }
    }
}
