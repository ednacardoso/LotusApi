using System;

namespace Lotus.Models.DTOs.Requests
{
    public class AlterarAgendamentoRequest
    {
        public DateTime NovaData { get; set; }
        public string NovoStatus { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public int FuncionarioId { get; set; }
    }
}
