using System;

public class AgendamentoDto
{
    public int Id { get; set; }
    public DateTime DataAgendamento { get; set; }
    public string Status { get; set; }
    public string Observacoes { get; set; }
    public string MotivoCancelamento { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNome { get; set; }
    public int FuncionarioId { get; set; }
    public string FuncionarioNome { get; set; }
}
