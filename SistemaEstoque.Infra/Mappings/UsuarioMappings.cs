using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Mappings
{
    public class UsuarioMappings : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(x => x.UsuarioId);
            builder.Property(x => x.Nome).IsRequired().HasColumnType("varchar(100)");            
            builder.Property(x => x.Telefone).HasColumnType("varchar(20)");

            builder.HasMany(u => u.Produtos)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);            

            builder.HasMany(u => u.Enderecos)
              .WithOne(e => e.Usuario)
              .HasForeignKey(e => e.UsuarioId)
              .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(u => u.Documento)
              .WithOne(d => d.Usuario)
              .HasForeignKey<Usuario>(u => u.DocumentoId);

          

            builder.ToTable("Usuario");
        }
    }
}
