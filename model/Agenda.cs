using System;
using System.Text.Json.Serialization;

namespace Lotus.Models
{
    public class Agendamentos
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Funcionario { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Observacoes { get; set; } = string.Empty;       
        public DateTime? DataAgendamento { get; set; }

        public void SetDataAgendamento(DateTime? dataAgendamento)
        {
            DataAgendamento = dataAgendamento?.ToUniversalTime();
        }
    }
}