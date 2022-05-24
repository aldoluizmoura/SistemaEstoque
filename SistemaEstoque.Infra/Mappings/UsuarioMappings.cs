using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Mappings
{
    public class UsuarioMappings : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).IsRequired().HasColumnType("varchar(100)");            
            builder.Property(x => x.Telefone).HasColumnType("varchar(20)");

            builder.HasMany(c => c.Produtos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId);

            builder.HasOne(f => f.Documento)
              .WithOne(d => d.Usuario)
              .HasForeignKey<Usuario>(f => f.DocumentoId);

            builder.ToTable("Usuario");
        }
    }
}
