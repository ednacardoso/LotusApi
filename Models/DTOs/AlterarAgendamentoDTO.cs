using System;

namespace Lotus.Models.DTOs.Requests
{
    public class AlterarAgendamentoRequest    {
       
        public DateTime? DataAgendamento { get; set; }
        public string? Observacoes { get; set; }
        public string NovoStatus { get; set; } = string.Empty;       
        public int FuncionarioId { get; set; }
    }
}
