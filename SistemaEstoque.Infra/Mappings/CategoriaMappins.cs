using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Mappings
{
    public class CategoriaMappins : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(c=>c.Nome).IsRequired().HasColumnType("varchar(200)");
            builder.Property(c => c.Codigo).IsRequired();

            builder.HasMany(c => c.Produtos)
                .WithOne(p => p.Categoria)
                .HasForeignKey(p=>p.CategoriaId);

            builder.ToTable("Categoria");
        }
    }
}
