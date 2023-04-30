using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.API.Autenticação
{
    public class ApplicationDbContext : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> modelBuilder) 
            : base(modelBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Usuario>().ToTable("Usuario")
                .Property(u => u.Id)
                .HasColumnName("Id");

            builder.Entity<IdentityUserRole<Guid>>().ToTable("UsuarioPapel");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("Login");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("Claims");
            builder.Entity<IdentityRole<Guid>>().ToTable("Papel");
        }
    }
}
