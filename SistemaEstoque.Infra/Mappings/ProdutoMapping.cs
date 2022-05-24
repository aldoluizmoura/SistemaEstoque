using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Descricao).IsRequired().HasColumnType("varchar(250)");
            builder.Property(p => p.Marca).IsRequired().HasColumnType("varchar(250)");
            builder.Property(p => p.Modelo).IsRequired().HasColumnType("varchar(250)");
            builder.Property(p => p.Preco).IsRequired();
            builder.Property(p => p.Ativo).IsRequired();            

            builder.ToTable("Produto");
        }
    }
}
