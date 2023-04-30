using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.Models.ModelsResponse
{
    public class FabricantesResponse
    {
        public string Nome { get; set; }
        public Guid Id { get; set; }
        public int Codigo { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public bool Ativo { get; set; }

        public FabricantesResponse(string nome, string numeroDocumento, int codigo, bool ativo, Guid id)
        {
            Nome = nome;
            NumeroDocumento = numeroDocumento;
            TipoDocumento = Documento.DefinirTipoDocumento(numeroDocumento).ToString();
            Codigo = codigo;
            Ativo = ativo;
            Id = id;
        }
    }
}
