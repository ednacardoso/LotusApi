using Microsoft.EntityFrameworkCore;
using Lotus.Models;

namespace Lotus.Data
{
    public class MLotusContext : DbContext
    {
        public MLotusContext(DbContextOptions<MLotusContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Funcionarios> Funcionarios { get; set; }

        public DbSet<Agendamentos> Agendamentos { get; set; }

        public DbSet<DescricaoPessoa> DescricaoPessoa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DescricaoPessoa>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Descricao).HasColumnName("descricao");
            });
        }



    }

}
