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

        private string ObterPerfilUsuario()
        {
            if (User.IsInRole("Administrador")) return "Administrador";
            if (User.IsInRole("Funcionario")) return "Funcionario";
            if (User.IsInRole("Cliente")) return "Cliente";
            return "Desconhecido";
        }

        [HttpGet]
        public async Task<IActionResult> GetAgendas()
        {
            int usuarioId = ObterUsuarioIdAutenticado();
            string perfil = ObterPerfilUsuario();

            IQueryable<Agendamentos> query = _context.Agendamentos.Where(a => a.Status == "Ativo");

            if (perfil == "Cliente")
            {
                query = query.Where(a => a.ClienteId == usuarioId);
            }
            else if (perfil == "Funcionario")
            {
                query = query.Where(a => a.FuncionarioId == usuarioId);
            }
            // Administrador já tem acesso a todos os agendamentos

            var agendamentos = await query.ToListAsync();
            return Ok(agendamentos);
        }

        [HttpPost]
        public async Task<IActionResult> AddAgendamento([FromBody] Agendamentos novoAgendamento)
        {
            if (novoAgendamento == null)
            {
                return BadRequest("O objeto enviado está vazio.");
            }

            int usuarioId = ObterUsuarioIdAutenticado();
            string perfil = ObterPerfilUsuario();

            // Buscar cliente pelo ID
            var cliente = await _context.Clientes.FindAsync(novoAgendamento.ClienteId);
            if (cliente == null)
            {
                return BadRequest("Cliente não encontrado.");
            }

            // Buscar funcionário pelo ID
            var funcionario = await _context.Funcionarios.FindAsync(novoAgendamento.FuncionarioId);
            if (funcionario == null)
            {
                return BadRequest("Funcionário não encontrado.");
            }

            // Regras de negócio conforme o perfil do usuário
            if (perfil == "Cliente" && novoAgendamento.ClienteId != usuarioId)
            {
                return Forbid("Clientes só podem agendar para si mesmos.");
            }

            if (perfil == "Funcionario" && novoAgendamento.FuncionarioId != usuarioId)
            {
                return Forbid("Funcionários só podem criar agendamentos para si mesmos.");
            }

            // Preencher os nomes automaticamente antes de salvar no banco
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

            var usuarioId = ObterUsuarioIdAutenticado();
            string perfil = ObterPerfilUsuario();

            var agendamento = await _context.Agendamentos.FindAsync(id);
            if (agendamento == null)
            {
                return NotFound("Agendamento não encontrado.");
            }

            // Regras de cancelamento:
            if (perfil == "Administrador" ||
                (perfil == "Funcionario" && agendamento.FuncionarioId == usuarioId) ||
                (perfil == "Cliente" && agendamento.ClienteId == usuarioId))
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
