using AutoMapper;
using SistemaEstoque.API.DTOs;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.AutoMapper
{
    public class InfraToApiMappingProfile : Profile
    {
        public InfraToApiMappingProfile()
        {
            CreateMap<Produto, ProdutoDto>();
            CreateMap<Categoria, CategoriaDto>();
        }
    }
}
