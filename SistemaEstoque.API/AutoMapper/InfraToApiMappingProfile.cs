using AutoMapper;
using SistemaEstoque.API.Models;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.AutoMapper
{
    public class InfraToApiMappingProfile : Profile
    {
        public InfraToApiMappingProfile()
        {
            CreateMap<Produto, ProdutoDto>();
            CreateMap<Categoria, CategoriaDto>();
            CreateMap<Documento, DocumentoDTO>();
            CreateMap<Usuario, RegistroUsuarioDTO>();
        }
    }
}
