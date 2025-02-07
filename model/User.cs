using System;
using System.Text.Json.Serialization;

namespace Lotus.Models

{
    public class User
    {
        public int UserId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string SenhaHash { get; set; }  // A senha será armazenada como hash
        public string Tipo { get; set; }  // Pode ser "cliente", "funcionario" ou "administrador"
    }

}
