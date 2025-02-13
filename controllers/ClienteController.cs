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

        [HttpGet("cliente/{userId}")]
        public async Task<IActionResult> GetClienteByUserId(int userId)
        {
            var cliente = await _context.Clientes
                .Include(c => c.User) // Inclui os dados do User
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cliente == null)
            {
                return NotFound(new { message = "Cliente não encontrado" });
            }

            return Ok(cliente);
        }

        private int ObterUsuarioIdAutenticado()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id" || c.Type == "sub");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }


        // Método para adicionar um cliente
        [HttpPost]
        public async Task<IActionResult> AddCliente([FromBody] Cliente novoCliente)
        {
            if (novoCliente == null)
            {
                return BadRequest("Cliente inválido");
            }

            // Obtém o ID do usuário autenticado
            int usuarioId = ObterUsuarioIdAutenticado();

            // Verifica se o cliente já possui um usuário vinculado
            if (usuarioId == 0)
            {
                return BadRequest("Usuário não autenticado");
            }

            novoCliente.UserId = usuarioId;  // 🔹 Agora o usuário está vinculado corretamente
            novoCliente.DataNascimento = novoCliente.DataNascimento?.ToUniversalTime();

            _context.Clientes.Add(novoCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClientes), new { id = novoCliente.Id }, novoCliente);
        }


    }
}
