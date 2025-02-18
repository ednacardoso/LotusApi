using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lotus.Models
{
    public class Agendamentos
    {
        public int Id { get; set; }
        public DateTime? DataAgendamento { get; set; }
        public string Status { get; set; } = "Ativo";
        public string Observacoes { get; set; } = string.Empty;
        public string? MotivoCancelamento { get; set; }

        // 🔹 Chaves estrangeiras
        public int ClienteId { get; set; }
        public int FuncionarioId { get; set; }

        // 🔹 Relacionamentos (Foreign Keys)
        [ForeignKey("ClienteId")]
        [JsonIgnore] // 🔥 Evita loop de serialização
        public Cliente? ClienteNavigation { get; set; }

        [ForeignKey("FuncionarioId")]
        [JsonIgnore]
        public Funcionarios? FuncionarioNavigation { get; set; }
    }
}
