using System;
using System.Text.Json.Serialization;

namespace Lotus.Models
{
    public class Administrador
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataNascimento { get; set; }

        public string descricao { get; set; } = string.Empty;

        public ICollection<Agendamentos> Agendamentos { get; set; }

        public void SetDataNascimento(DateTime? dataNascimento)
        {
            DataNascimento = dataNascimento?.ToUniversalTime();
        }
    }
}

