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

        private int ObterUsuarioIdAutenticado()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id" || c.Type == "sub");
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }


        [HttpGet]
        public async Task<IActionResult> GetAgendas()
        {
            int clienteId = ObterUsuarioIdAutenticado();
            bool isAdmin = User.IsInRole("Administrador");

            var agendamentos = isAdmin
                ? await _context.Agendamentos.Where(a => a.Status == "Ativo").ToListAsync()
                : await _context.Agendamentos.Where(a => a.ClienteId == clienteId && a.Status == "Ativo").ToListAsync();

            return Ok(agendamentos);
        }



        [HttpPost]
        public async Task<IActionResult> AddAgendamento([FromBody] Agendamentos novoAgendamento)
        {
            if (novoAgendamento == null)
            {
                return BadRequest("O objeto enviado está vazio.");
            }

            // Verify if cliente exists
            var cliente = await _context.Clientes.FindAsync(novoAgendamento.ClienteId);
            // Verify if funcionario exists
            var funcionario = await _context.Funcionarios.FindAsync(novoAgendamento.FuncionarioId);

            if (cliente == null)
            {
                return BadRequest("Cliente não encontrado no sistema.");
            }

            if (funcionario == null)
            {
                return BadRequest("Funcionário não encontrado no sistema.");
            }

            // If both exist, proceed with creating the appointment
            novoAgendamento.Cliente = cliente.Nome;
            novoAgendamento.Funcionario = funcionario.Nome;

            await _context.Agendamentos.AddAsync(novoAgendamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgendas), new { id = novoAgendamento.Id }, novoAgendamento);
        }




        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> CancelarAgendamento(int id, [FromBody] CancelarAgendamentoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Motivo))
            {
                return BadRequest("O motivo do cancelamento é obrigatório.");
            }

            var clienteId = ObterUsuarioIdAutenticado();
            bool isAdmin = User.IsInRole("Administrador");
            bool isFuncionario = User.IsInRole("Funcionario");

            var agendamento = await _context.Agendamentos.FindAsync(id);

            if (agendamento == null)
            {
                return NotFound("Agendamento não encontrado.");
            }

            // Regras de cancelamento:
            if (isAdmin || agendamento.ClienteId == clienteId || (isFuncionario && agendamento.FuncionarioId == clienteId))
            {
                agendamento.Status = "Cancelado";
                agendamento.MotivoCancelamento = request.Motivo;
                await _context.SaveChangesAsync();

                return Ok(new { message = "Agendamento cancelado com sucesso." });
            }

            return Forbid("Você não tem permissão para cancelar este agendamento.");
        }


    }
}
