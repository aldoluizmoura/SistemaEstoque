﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaEstoque.Infra.Entidades;

namespace SistemaEstoque.Infra.Mappings
{
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Logradouro).IsRequired().HasColumnType("varchar(150)");
            builder.Property(e => e.Numero).IsRequired().HasColumnType("varchar(50)");
            builder.Property(e => e.Cidade).IsRequired().HasColumnType("varchar(100)");
            builder.Property(e => e.Estado).IsRequired().HasColumnType("varchar(50)");
            builder.Property(e => e.Cep).IsRequired().HasColumnType("varchar(8)");
            builder.Property(e => e.Complemento).HasColumnType("varchar(100)");

            builder.ToTable("Endereco");
        }
    }
}
