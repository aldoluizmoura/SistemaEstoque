using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.Autenticação
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> modelBuilder) 
            : base(modelBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>().ToTable("Usuario")
                .Property(u => u.UsuarioId)
                .HasColumnName("UsuarioId");

            builder.Entity<IdentityUserRole<string>>().ToTable("UsuarioPapel");
            builder.Entity<IdentityUserLogin<string>>().ToTable("Login");
            builder.Entity<IdentityUserClaim<string>>().ToTable("Claim");
            builder.Entity<IdentityRole>().ToTable("Papel");
        }
    }
}
