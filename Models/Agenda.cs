using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


public class Agendamentos
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int FuncionarioId { get; set; }
    public DateTime DataAgendamento { get; set; }
    public string Status { get; set; }
    public string Observacoes { get; set; }
    public string? MotivoCancelamento { get; set; }

    [JsonIgnore]
    public Cliente? ClienteNavigation { get; set; }

    [JsonIgnore]
    public Funcionarios? FuncionarioNavigation { get; set; }
}



