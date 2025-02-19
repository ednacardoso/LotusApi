public class Agendamentos
{
    public int Id { get; set; }
    public DateTime DataAgendamento { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public string? MotivoCancelamento { get; set; }
    public int ClienteId { get; set; }
    public virtual Cliente ClienteNavigation { get; set; }
    public int FuncionarioId { get; set; }
    public virtual Funcionarios FuncionarioNavigation { get; set; }
}
