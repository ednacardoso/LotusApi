using Lotus.Models;
using Lotus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lotus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendaController : ControllerBase
    {
        private readonly MLotusContext _context;

        public AgendaController(MLotusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAgendas()
        {
            var agendamentos = await _context.Agendamentos.ToListAsync();
            return Ok(agendamentos);
        }

        [HttpPost]
        public async Task<IActionResult> AddAgendamento([FromBody] Agendamentos novoAgendamento)
        {
            // Lógica para converter a data de nascimento para UTC (se necessário)
            novoAgendamento.DataAgendamento = novoAgendamento.DataAgendamento?.ToUniversalTime();

            _context.Agendamentos.Add(novoAgendamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgendas), new { id = novoAgendamento.Id }, novoAgendamento);            
            
        }
    }
}
