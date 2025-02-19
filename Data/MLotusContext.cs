using Microsoft.EntityFrameworkCore;
using Lotus.Models;

namespace Lotus.Data
{
    public class MLotusContext : DbContext
    {
        public MLotusContext(DbContextOptions<MLotusContext> options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Funcionarios> Funcionarios { get; set; }
        public DbSet<Agendamentos> Agendamentos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Cliente)
                    .HasForeignKey<Cliente>(e => e.UserId);
                entity.HasMany(e => e.Agendamentos)
                    .WithOne(a => a.ClienteNavigation)
                    .HasForeignKey(a => a.ClienteId);
            });

            modelBuilder.Entity<Funcionarios>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                    .WithOne(u => u.Funcionario)
                    .HasForeignKey<Funcionarios>(e => e.UserId);
                entity.HasMany(e => e.Agendamentos)
                    .WithOne(a => a.FuncionarioNavigation)
                    .HasForeignKey(a => a.FuncionarioId);
            });

            modelBuilder.Entity<Agendamentos>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.DataAgendamento).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.SenhaHash).IsRequired();
                entity.Property(e => e.Tipo).IsRequired();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Token);
                entity.Property(e => e.UserEmail).IsRequired();
                entity.Property(e => e.Expiration).IsRequired();
            });
        }
    }
}
