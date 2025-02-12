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

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegistrationRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "E-mail já cadastrado." });
            }

            var user = new User
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
                Tipo = request.Tipo
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            switch (request.Tipo.ToLower())
            {
                case "cliente":
                    var cliente = new Cliente
                    {
                        Nome = request.Nome,
                        Email = request.Email,
                        UserId = user.UserId,
                        Cpf = request.Cpf,
                        Apelido = request.Apelido,
                        Telefone = request.Telefone,
                        DataNascimento = request.DataNascimento
                    };
                    _context.Clientes.Add(cliente);
                    break;

                case "funcionario":
                    var funcionario = new Funcionarios
                    {
                        Nome = request.Nome,
                        Email = request.Email,
                        UserId = user.UserId,
                        Cpf = request.Cpf,
                        Apelido = request.Apelido,
                        Telefone = request.Telefone,
                        Especialidade = request.Especialidade,
                        DataNascimento = request.DataNascimento
                    };
                    _context.Funcionarios.Add(funcionario);
                    break;

                case "administrador":
                    var administrador = new Administrador
                    {
                        Nome = request.Nome,
                        Email = request.Email,
                        UserId = user.UserId,
                        Cpf = request.Cpf,
                        Apelido = request.Apelido,
                        Telefone = request.Telefone,
                        Especialidade = request.Especialidade,
                        DataNascimento = request.DataNascimento
                    };
                    _context.Administradores.Add(administrador);
                    break;
            }

            await _context.SaveChangesAsync();
            return Ok(new { userId = user.UserId, message = "Usuário criado com sucesso." });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

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
