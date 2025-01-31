using Lotus.Models;
using Lotus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lotus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly MLotusContext _context;

        public ClientesController(MLotusContext context)
        {
            _context = context;
        }

        // Método para obter todos os clientes
        [HttpGet]
        public async Task<IActionResult> GetClientes()
        {
            var clientes = await _context.Clientes
               .Select(f => new
               {
                   id = f.Id,
                   nome = f.Nome,
                   Telefone = f.Telefone ?? "Sem telefone",
                   email = f.Email ?? "Sem email",
                   apelido = f.Apelido ?? "Sem apelido",
               })
               .ToListAsync();
            return Ok(clientes);
        }       

        // Método para adicionar um cliente
        [HttpPost]
        public async Task<IActionResult> AddCliente([FromBody] Cliente novoCliente)
        {
            if (novoCliente == null)
            {
                return BadRequest("Cliente inválido");
            }

            // Lógica para converter a data de nascimento para UTC (se necessário)
            novoCliente.DataNascimento = novoCliente.DataNascimento?.ToUniversalTime();

            // Adiciona o cliente ao banco de dados
            _context.Clientes.Add(novoCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClientes), new { id = novoCliente.Id }, novoCliente);
        }
    }
}
