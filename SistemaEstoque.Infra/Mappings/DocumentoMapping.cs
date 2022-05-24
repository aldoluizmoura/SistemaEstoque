using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Mappings
{
    public class DocumentoMapping : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Numero).IsRequired().HasColumnType("varchar(14)");

            builder.ToTable("Documento");
        }
    }
}
