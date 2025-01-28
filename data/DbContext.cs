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



    }

}
