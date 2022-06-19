using AutoMapper;
using SistemaEstoque.API.DTOs;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.AutoMapper
{
    public class ApiToInfraMappingProfile : Profile
    {
        public ApiToInfraMappingProfile()
        {
            CreateMap<ProdutoDto, Produto>()
                .ConstructUsing(p =>
                new Produto(p.Descricao, p.Preco, p.QuantidadeEstoque, p.Marca,
                p.Modelo, p.FabricanteId, p.CategoriaId, p.DataVencimento,
                p.Imagem, p.UsuarioId, p.Ativo));

            CreateMap<CategoriaDto, Categoria>()
                .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));

            CreateMap<DocumentoDTO, Documento>()
               .ConstructUsing(d => new Documento(d.Numero));

            CreateMap<RegistroUsuarioDTO, Usuario>()
               .ConstructUsing(u => new Usuario(u.Nome, new Documento(u.Documento.Numero), u.DataNascimento, u.Ativo, u.Telefone, u.Email));
        }

    }
}
