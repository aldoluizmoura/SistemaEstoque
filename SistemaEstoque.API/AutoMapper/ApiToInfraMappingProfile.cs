using AutoMapper;
using SistemaEstoque.API.Models;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.AutoMapper
{
    public class ApiToInfraMappingProfile : Profile
    {
        public ApiToInfraMappingProfile()
        {
            CreateMap<ProdutoDto, Produto>()
                .ConstructUsing(p =>
                new Produto(p.Descricao, p.Codigo, p.Preco, p.QuantidadeEstoque, p.Marca,
                p.Modelo, p.FabricanteId, p.CategoriaId, p.DataVencimento,
                p.Imagem, p.UsuarioId));

            CreateMap<CategoriaDto, Categoria>()
                .ConstructUsing(c => new Categoria(c.Nome, c.Codigo));

            CreateMap<DocumentoDTO, Documento>()
               .ConstructUsing(d => new Documento(d.NumeroDocumento));

            CreateMap<RegistroUsuarioDTO, Usuario>()
               .ConstructUsing(u => new Usuario(u.Nome, u.Matricula, new Documento(u.Documento.NumeroDocumento), u.DataNascimento, u.Telefone, u.Email));

            CreateMap<FabricanteDTO, Fabricante>()
              .ConstructUsing(f => new Fabricante(f.Nome, f.Codigo, new Documento(f.NumeroDocumento), f.UsuarioId));
        }
    }
}
