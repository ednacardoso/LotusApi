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

        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Administrador> Administradores { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamento Cliente - User (Já existente)
            modelBuilder.Entity<Cliente>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Cliente>(c => c.UserId);

            // Relacionamento Funcionários - User (Já existente)
            modelBuilder.Entity<Funcionarios>()
                .HasOne(f => f.User)
                .WithOne()
                .HasForeignKey<Funcionarios>(f => f.UserId);

            // 🔹 Relacionamento Agendamentos - Cliente
            modelBuilder.Entity<Agendamentos>()
                .HasOne(a => a.ClienteNavigation)
                .WithMany(c => c.Agendamentos)  // Um cliente pode ter vários agendamentos
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);  // Se o cliente for deletado, deleta os agendamentos também (opcional)

            // 🔹 Relacionamento Agendamentos - Funcionários
            modelBuilder.Entity<Agendamentos>()
                .HasOne(a => a.FuncionarioNavigation)
                .WithMany(f => f.Agendamentos)  // Um funcionário pode ter vários agendamentos
                .HasForeignKey(a => a.FuncionarioId)
                .OnDelete(DeleteBehavior.Restrict); // Impede a exclusão se houver agendamentos pendentes
        }



    }

}


