using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.EntityFrameworkCore.Extensions;

namespace API.Entities
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            if (Database.IsMySql())
                Database.SetCommandTimeout(120);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // Remover a configuração de banco de dados do OnConfiguring para evitar conflitos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Apenas se necessário
            if (!optionsBuilder.IsConfigured)
            {
                if (Database.IsMySql())
                {
                    var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var builder = new ConfigurationBuilder().SetBasePath(Path.GetDirectoryName(location))
                                                            .AddJsonFile("appsettings.json");
                    optionsBuilder.UseMySQL(builder.Build().GetConnectionString("DefaultConnection"));
                }
                else
                    optionsBuilder.UseInMemoryDatabase("InMemoryDb");
            }
        }

        public virtual DbSet<TB_USUARIO> TB_USUARIO { get; set; }
        public virtual DbSet<TB_Exercicios> TB_EXERCICIOS { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TB_USUARIO>().HasData(
            new TB_USUARIO()
            {
                Id = 1,
                Nome = "Admin",
                Senha = "Admin",
                Status = Entities.TB_USUARIO.EStatus.Ativo
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
