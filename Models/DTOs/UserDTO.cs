using System;


    namespace Lotus.Models.DTOs
    {
        public class UserDto
        {
            public int UserId { get; set; }
            public string Nome { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;
            public DateTime DataCriacao { get; set; }
            public DateTime? UltimoLogin { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Funcionarios Funcionario { get; set; }
    }
    }


