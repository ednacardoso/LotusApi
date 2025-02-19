using System;

public class UpdateProfileRequest
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Apelido { get; set; }
    public DateTime? DataNascimento { get; set; }
}
