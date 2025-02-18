public class RegistrationRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Apelido { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string? Especialidade { get; set; }
    public DateTime? DataNascimento { get; set; }
}
