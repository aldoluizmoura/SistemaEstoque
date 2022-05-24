using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Mappings
{
    public class FabricanteMappings : IEntityTypeConfiguration<Fabricante>
    {
        public void Configure(EntityTypeBuilder<Fabricante> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Nome).IsRequired().HasColumnType("varchar(200)");

            builder.HasMany(f => f.Produtos)
                .WithOne(f => f.Fabricante)
                .HasForeignKey(f => f.FabricanteId);

            builder.HasOne(f => f.Documento)
              .WithOne(d => d.Fabricante)
              .HasForeignKey<Fabricante>(f => f.DocumentoId);

            builder.ToTable("Fabricante");

        }
    }
}
