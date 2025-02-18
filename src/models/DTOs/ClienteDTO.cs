using System;

public class ClienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string? Apelido { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public DateTime? DataNascimento { get; set; }
    public string Descricao { get; set; } = string.Empty;
}
