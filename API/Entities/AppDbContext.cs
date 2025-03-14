using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Entities
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<TB_USUARIO> USUARIO { get; set; }
        public DbSet<TB_Exercicios> EXERCICIOS { get; set; }
        public DbSet<TB_CATEGORIA> CATEGORIAS { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Verifica o provedor de banco de dados configurado no appsettings.json
            var databaseProvider = _configuration["DatabaseProvider"];

            if (!optionsBuilder.IsConfigured)
            {
                #region Define o tipo de banco de dados
                switch (databaseProvider)
                {
                    case "":
                    case "InMemory":
                        optionsBuilder.UseInMemoryDatabase("InMemoryDb");
                        break;
                    case "Sqlite":
                        string DbPath = System.IO.Path.Join(AppDomain.CurrentDomain.BaseDirectory, "Database.db");
                        optionsBuilder.UseSqlite($"Data Source={DbPath}");
                        break;
                    case "MySQL":
                        var connectionString = _configuration.GetConnectionString("DefaultConnection");
                        optionsBuilder.UseMySQL(connectionString);
                        break;
                }
                #endregion
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações de relacionamentos
            modelBuilder.Entity<TB_Exercicios>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Exercicios)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TB_Exercicios>()
                .HasOne(e => e.Categoria)
                .WithMany(c => c.Exercicios)
                .HasForeignKey(e => e.CategoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            modelBuilder.Entity<TB_Exercicios>()
                .HasIndex(e => e.Exercicio);

            modelBuilder.Entity<TB_USUARIO>()
                .HasIndex(u => u.Nome)
                .IsUnique();

            // Seed de dados
            modelBuilder.Entity<TB_USUARIO>().HasData(
                new TB_USUARIO
                {
                    Id = 1,
                    Nome = "Admin",
                    Senha = "Admin@123",
                    Status = TB_USUARIO.EStatus.Ativo,
                    Created = DateTime.Now
                },
                new TB_USUARIO
                {
                    Id = 2,
                    Nome = "João",
                    Senha = "Joao@123",
                    Status = TB_USUARIO.EStatus.Ativo,
                    Created = DateTime.Now
                }
            );

            modelBuilder.Entity<TB_CATEGORIA>().HasData(
                new TB_CATEGORIA { Id = 1, Nome = "Musculação" },
                new TB_CATEGORIA { Id = 2, Nome = "Cardio" },
                new TB_CATEGORIA { Id = 3, Nome = "Flexibilidade" }
            );

            modelBuilder.Entity<TB_Exercicios>().HasData(
                new TB_Exercicios
                {
                    Id = 1,
                    Exercicio = "Supino",
                    Serie = 4,
                    Repeticoes = 12,
                    Tempo = 0,
                    CategoriaId = 1,
                    UsuarioId = 1,
                    DataCriacao = DateTime.Now
                },
                new TB_Exercicios
                {
                    Id = 2,
                    Exercicio = "Corrida",
                    Serie = 1,
                    Repeticoes = 0,
                    Tempo = 30,
                    CategoriaId = 2,
                    UsuarioId = 1,
                    DataCriacao = DateTime.Now
                },
                new TB_Exercicios
                {
                    Id = 3,
                    Exercicio = "Alongamento",
                    Serie = 2,
                    Repeticoes = 10,
                    Tempo = 15,
                    CategoriaId = 3,
                    UsuarioId = 2,
                    DataCriacao = DateTime.Now
                }
            );
        }
    }
}
