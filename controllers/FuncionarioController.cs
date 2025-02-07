using Lotus.Models;
using Lotus.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.EntityFrameworkCore;

namespace Lotus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionariosController : ControllerBase
    {
        private readonly MLotusContext _context;

        public FuncionariosController(MLotusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFuncionarios()
        {
            var funcionarios = await _context.Funcionarios
                .Select(f => new
                {
                    id = f.Id,
                    nome = f.Nome,
                    especialidade = f.Especialidade ?? "Sem especialidade",
                    Telefone = f.Telefone ?? "Sem telefone",
                    email = f.Email ?? "Sem email",
                    apelido = f.Apelido ?? "Sem apelido",
                })
                .ToListAsync();

            return Ok(funcionarios);
        }

        [HttpGet("funcionario/{userId}")]
        public async Task<IActionResult> GetFuncionarioByUserId(int userId)
        {
            var funcionario = await _context.Funcionarios
                .Include(f => f.User) // Inclui os dados do User
                .FirstOrDefaultAsync(f => f.UserId == userId);

            if (funcionario == null)
            {
                return NotFound(new { message = "Funcionário não encontrado" });
            }

            return Ok(funcionario);
        }


        [HttpPost]
        public async Task<IActionResult> AddFuncionario([FromBody] Funcionarios novoFuncionario)
        {
            if (novoFuncionario == null)
            {
                return BadRequest("Funcionário inválido");
            }

            // Lógica para converter a data de nascimento para UTC (se necessário)
            novoFuncionario.DataNascimento = novoFuncionario.DataNascimento?.ToUniversalTime();

            // Adiciona o cliente ao banco de dados
            _context.Funcionarios.Add(novoFuncionario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFuncionarios), new { id = novoFuncionario.Id }, novoFuncionario);
        }
    }
}