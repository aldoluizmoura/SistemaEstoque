using Microsoft.EntityFrameworkCore;
using SistemaEstoque.API.Autenticação;
using SistemaEstoque.API.AutoMapper;
using SistemaEstoque.Infra.Contexto;
using SistemaEstoque.Infra.Interfaces.Repositorio;
using SistemaEstoque.Infra.Repositorios;
using SistemaEstoque.Negocio;
using SistemaEstoque.Negocio.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Banco de dados config

builder.Services.AddDbContext<DbContextEstoque>(options => 
                              options.UseSqlServer(builder.Configuration
                              .GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
                              options.UseSqlServer(builder.Configuration
                              .GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(InfraToApiMappingProfile),
                               typeof(ApiToInfraMappingProfile));

// Injenção de Dependencia
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
