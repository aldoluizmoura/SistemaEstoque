﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaEstoque.Infra.Contexto;

#nullable disable

namespace SistemaEstoque.Infra.Migrations
{
    [DbContext(typeof(DbContextEstoque))]
    [Migration("20230408172326_Primeira")]
    partial class Primeira
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Categoria", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Codigo")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Categoria", (string)null);
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Documento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("varchar(14)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Documento", (string)null);
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Endereco", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("varchar(8)");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Complemento")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Endereco", (string)null);
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Fabricante", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DocumentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentoId")
                        .IsUnique();

                    b.ToTable("Fabricante", (string)null);
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Produto", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<Guid>("CategoriaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DataCadastro")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataVencimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<Guid>("FabricanteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Imagem")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<double>("Preco")
                        .HasColumnType("float");

                    b.Property<int>("QuantidadeEstoque")
                        .HasColumnType("int");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("FabricanteId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Produto", (string)null);
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Usuario", b =>
                {
                    b.Property<Guid>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DocumentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Id")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(100)");

                    b.HasKey("UsuarioId");

                    b.HasIndex("DocumentoId")
                        .IsUnique();

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Endereco", b =>
                {
                    b.HasOne("SistemaEstoque.Infra.Entidades.Usuario", "Usuario")
                        .WithMany("Enderecos")
                        .HasForeignKey("UsuarioId")
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Fabricante", b =>
                {
                    b.HasOne("SistemaEstoque.Infra.Entidades.Documento", "Documento")
                        .WithOne("Fabricante")
                        .HasForeignKey("SistemaEstoque.Infra.Entidades.Fabricante", "DocumentoId")
                        .IsRequired();

                    b.Navigation("Documento");
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Produto", b =>
                {
                    b.HasOne("SistemaEstoque.Infra.Entidades.Categoria", "Categoria")
                        .WithMany("Produtos")
                        .HasForeignKey("CategoriaId")
                        .IsRequired();

                    b.HasOne("SistemaEstoque.Infra.Entidades.Fabricante", "Fabricante")
                        .WithMany("Produtos")
                        .HasForeignKey("FabricanteId")
                        .IsRequired();

                    b.HasOne("SistemaEstoque.Infra.Entidades.Usuario", "Usuario")
                        .WithMany("Produtos")
                        .HasForeignKey("UsuarioId")
                        .IsRequired();

                    b.Navigation("Categoria");

                    b.Navigation("Fabricante");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Usuario", b =>
                {
                    b.HasOne("SistemaEstoque.Infra.Entidades.Documento", "Documento")
                        .WithOne("Usuario")
                        .HasForeignKey("SistemaEstoque.Infra.Entidades.Usuario", "DocumentoId")
                        .IsRequired();

                    b.Navigation("Documento");
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Categoria", b =>
                {
                    b.Navigation("Produtos");
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Documento", b =>
                {
                    b.Navigation("Fabricante")
                        .IsRequired();

                    b.Navigation("Usuario")
                        .IsRequired();
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Fabricante", b =>
                {
                    b.Navigation("Produtos");
                });

            modelBuilder.Entity("SistemaEstoque.Infra.Entidades.Usuario", b =>
                {
                    b.Navigation("Enderecos");

                    b.Navigation("Produtos");
                });
#pragma warning restore 612, 618
        }
    }
}
