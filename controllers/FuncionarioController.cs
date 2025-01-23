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
            
            var funcionario = await _context.Funcionarios.ToListAsync();
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