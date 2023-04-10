using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.Models.ListasModels
{
    public class FabricantesModel
    {
        public string Nome { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }

        public FabricantesModel(string nome, string numeroDocumento)
        {
            Nome = nome;
            NumeroDocumento = numeroDocumento;
            TipoDocumento = Documento.DefinirTipoDocumento(numeroDocumento).ToString();
        }
    }
}
