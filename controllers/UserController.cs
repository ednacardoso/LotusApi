using Lotus.Data;
using Lotus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Lotus.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MLotusContext _context;

        public UserController(MLotusContext context)
        {
            _context = context;
        }

        // 🔹 LISTAR TODOS OS USUÁRIOS
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // 🔹 OBTER UM USUÁRIO POR ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }
            return Ok(user);
        }

        // 🔹 ATUALIZAR DADOS DO USUÁRIO
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            user.Nome = updatedUser.Nome;
            user.Email = updatedUser.Email;
            user.Tipo = updatedUser.Tipo;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário atualizado com sucesso!" });
        }

        // 🔹 DELETAR UM USUÁRIO
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário deletado com sucesso!" });
        }
    }
}
