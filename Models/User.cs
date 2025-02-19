public class User
{
    public int UserId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public virtual Cliente Cliente { get; set; }
    public virtual Funcionarios Funcionario { get; set; }
}
