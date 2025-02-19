namespace Lotus.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoLogin { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Funcionarios Funcionario { get; set; }
    }
}
