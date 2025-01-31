using System;
using System.Text.Json.Serialization;

namespace Lotus.Models
{    
    public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);
    


    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string? Apelido { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;        
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? DataNascimento { get; set; }
        public string Descricao { get; set; } = string.Empty;

        public void SetDataNascimento(DateTime? dataNascimento)
        {
            DataNascimento = dataNascimento?.ToUniversalTime();
        }
    }
}
