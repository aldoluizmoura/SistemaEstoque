using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.Models.ListasModels
{
    public class FabricantesModel
    {
        public string Nome { get; set; }
        public int Codigo { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public bool Ativo { get; set; }

        public FabricantesModel(string nome, string numeroDocumento, int codigo, bool ativo)
        {
            Nome = nome;
            NumeroDocumento = numeroDocumento;
            TipoDocumento = Documento.DefinirTipoDocumento(numeroDocumento).ToString();
            Codigo = codigo;
            Ativo = ativo;
        }
    }
}
