using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lotus.Models
{
    public class Agendamentos
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Funcionario { get; set; } = string.Empty;
        public DateTime? DataAgendamento { get; set; }
        public string Status { get; set; } = "Ativo";
        public string Observacoes { get; set; } = string.Empty;       
        public string? MotivoCancelamento { get; set; }       
        public int ClienteId { get; set; }
        public int FuncionarioId { get; set; }

        public void SetDataAgendamento(DateTime? dataAgendamento)
        {
            DataAgendamento = dataAgendamento?.ToUniversalTime();
        }
    }
}