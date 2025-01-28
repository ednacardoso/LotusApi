using System;
using System.Text.Json.Serialization;

namespace Lotus.Models
{
    public class Funcionarios
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataNascimento { get; set; }

        public string descricao { get; set; } = string.Empty;

        public void SetDataNascimento(DateTime? dataNascimento)
        {
            DataNascimento = dataNascimento?.ToUniversalTime();
        }
    }
}
