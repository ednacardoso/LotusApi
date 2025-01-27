using Lotus.Models;
using Lotus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lotus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DescricaoPessoaController : ControllerBase
    {
        private readonly MLotusContext _context;

        public DescricaoPessoaController(MLotusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDescricaoPessoa()
        {
            var descricaoPessoa = await _context.DescricaoPessoa.ToListAsync();
            return Ok(descricaoPessoa);
        }

        [HttpPost]
        public async Task<IActionResult> AddDescricaoPessoa([FromBody] DescricaoPessoa novoDescricaoPessoa)
        {
            _context.DescricaoPessoa.Add(novoDescricaoPessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDescricaoPessoa), new { id = novoDescricaoPessoa.Id }, novoDescricaoPessoa);
        }
    }
}

