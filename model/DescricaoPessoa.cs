using System;
using System.Text.Json.Serialization;

namespace Lotus.Models
{
    
        
        public class DescricaoPessoa
        {
            public int Id { get; set; }  // Changed from lowercase 'id' to uppercase 'Id'
            public string Descricao { get; set; } = string.Empty;
        }


    }