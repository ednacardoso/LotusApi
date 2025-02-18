using Lotus.Models;
using Lotus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        // 🔹 Rota única para obter agendamentos do usuário
        [HttpGet("usuario")]
        public async Task<IActionResult> GetAgendamentosPorUsuario()
        {
            int usuarioId = ObterUsuarioIdAutenticado();
            string perfil = ObterPerfilUsuario();

            IQueryable<Agendamentos> query = _context.Agendamentos
                .Include(a => a.ClienteNavigation)  // Incluindo o cliente
                .Include(a => a.FuncionarioNavigation);  // Incluindo o funcionário

            if (perfil == "Cliente")
            {
                var clienteExistente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.UserId == usuarioId);

                if (clienteExistente == null)
                {
                    return NotFound(new
                    {
                        code = "CLIENTE_NAO_CADASTRADO",
                        message = "Cadastro completo do cliente necessário"
                    });
                }
            }

            var agendamentos = await query
                .Include(a => a.ClienteNavigation) // Garantindo que a relação Cliente seja carregada
                .Include(a => a.FuncionarioNavigation) // Garantindo que a relação Funcionário seja carregada
                .Select(a => new
                {
                    a.Id,
                    a.DataAgendamento,
                    a.Status,
                    a.Observacoes,
                    a.MotivoCancelamento,
                    a.ClienteId,
                    ClienteNome = a.ClienteNavigation != null ? a.ClienteNavigation.Nome : "Cliente não informado",
                    a.FuncionarioId,
                    FuncionarioNome = a.FuncionarioNavigation != null ? a.FuncionarioNavigation.Nome : "Funcionário não informado"
                })
                .ToListAsync();

            return Ok(new { values = agendamentos }); // Retornando no formato esperado pelo Flutter
                                                      

        }


        [HttpGet("cliente/{clienteId}")]
        public IActionResult GetAgendamentosPorCliente(int clienteId)
        {
            var agendamentos = _context.Agendamentos
                .Include(a => a.ClienteNavigation)
                .Include(a => a.FuncionarioNavigation)
                .Where(a => a.ClienteId == clienteId)
                .Select(a => new
                {
                    a.Id,
                    a.DataAgendamento,
                    a.Status,
                    a.Observacoes,
                    a.MotivoCancelamento,
                    Cliente = new { Nome = a.ClienteNavigation.Nome },
                    Funcionario = new { Nome = a.FuncionarioNavigation.Nome }
                })
                .ToList();

            return Ok(agendamentos);
        }


        [HttpGet("funcionario/{funcionarioId?}")]
        public IActionResult GetAgendamentosPorFuncionario(int? funcionarioId)
        {
            var query = _context.Agendamentos
                .Include(a => a.ClienteNavigation)
                .Include(a => a.FuncionarioNavigation)
                .AsQueryable();

            if (funcionarioId.HasValue)
            {
                query = query.Where(a => a.FuncionarioId == funcionarioId.Value);
            }

            return Ok(query.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GetAgendamentosAtivos()
        {
            int usuarioId = ObterUsuarioIdAutenticado();
            string perfil = ObterPerfilUsuario();

            IQueryable<Agendamentos> query = _context.Agendamentos
                .Include(a => a.ClienteNavigation)
                .Include(a => a.FuncionarioNavigation)
                .Where(a => a.Status == "Ativo");

            if (perfil == "Cliente")
            {
                query = query.Where(a => a.ClienteId == usuarioId);
            }
            else if (perfil == "Funcionario")
            {
                query = query.Where(a => a.FuncionarioId == usuarioId);
            }

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

            Console.WriteLine($"Recebido ClienteId: {novoAgendamento.ClienteId}");
            Console.WriteLine($"Recebido FuncionarioId: {novoAgendamento.FuncionarioId}");
            Console.WriteLine($"Recebido DataAgendamento: {novoAgendamento.DataAgendamento}");

            // Validação adicional
            if (!await _context.Clientes.AnyAsync(c => c.Id == novoAgendamento.ClienteId))
            {
                return BadRequest($"Cliente não encontrado: {novoAgendamento.ClienteId}");
            }

            if (!await _context.Funcionarios.AnyAsync(f => f.Id == novoAgendamento.FuncionarioId))
            {
                return BadRequest($"Funcionário não encontrado: {novoAgendamento.FuncionarioId}");
            }

            var agendamento = new Agendamentos
            {
                ClienteId = novoAgendamento.ClienteId,
                FuncionarioId = novoAgendamento.FuncionarioId,
                DataAgendamento = novoAgendamento.DataAgendamento,
                Status = "Ativo",
                Observacoes = novoAgendamento.Observacoes,
                MotivoCancelamento = novoAgendamento.MotivoCancelamento
            };

            await _context.Agendamentos.AddAsync(agendamento);
            await _context.SaveChangesAsync();

            var createdAgendamento = await _context.Agendamentos
                .Include(a => a.ClienteNavigation)
                .Include(a => a.FuncionarioNavigation)
                .FirstOrDefaultAsync(a => a.Id == agendamento.Id);

            return CreatedAtAction(nameof(GetAgendamentosAtivos), createdAgendamento);
        }


        [HttpPut("{id}/alterar")]
        public async Task<IActionResult> AlterarAgendamento(int id, [FromBody] AlterarAgendamentoRequest request)
        {
            var agendamento = await _context.Agendamentos
                .Include(a => a.ClienteNavigation)
                .Include(a => a.FuncionarioNavigation)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
            {
                return NotFound("Agendamento não encontrado.");
            }

            var usuarioId = ObterUsuarioIdAutenticado();
            string perfil = ObterPerfilUsuario();

            if (!(perfil == "Administrador" ||
                 (perfil == "Funcionario" && agendamento.FuncionarioId == usuarioId) ||
                 (perfil == "Cliente" && agendamento.ClienteId == usuarioId)))
            {
                return Forbid("Você não tem permissão para alterar este agendamento.");
            }

            // Atualiza os campos permitidos
            if (request.DataAgendamento.HasValue)
                agendamento.DataAgendamento = request.DataAgendamento.Value;

            if (!string.IsNullOrWhiteSpace(request.Observacoes))
                agendamento.Observacoes = request.Observacoes;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                agendamento.Id,
                agendamento.DataAgendamento,
                agendamento.Status,
                agendamento.Observacoes,
                ClienteNome = agendamento.ClienteNavigation.Nome,
                FuncionarioNome = agendamento.FuncionarioNavigation.Nome
            });
        }


        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> CancelarAgendamento(int id, [FromBody] CancelarAgendamentoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Motivo))
            {
                return BadRequest("O motivo do cancelamento é obrigatório.");
            }

            var agendamento = await _context.Agendamentos
                .Include(a => a.ClienteNavigation)
                .Include(a => a.FuncionarioNavigation)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (agendamento == null)
            {
                return NotFound("Agendamento não encontrado.");
            }

            var usuarioId = ObterUsuarioIdAutenticado();
            string perfil = ObterPerfilUsuario();

            if (perfil == "Administrador" ||
                (perfil == "Funcionario" && agendamento.FuncionarioId == usuarioId) ||
                (perfil == "Cliente" && agendamento.ClienteId == usuarioId))
            {
                agendamento.Status = "Cancelado";
                agendamento.MotivoCancelamento = request.Motivo;
                await _context.SaveChangesAsync();

                return Ok(agendamento);
            }

            return Forbid("Você não tem permissão para cancelar este agendamento.");
        }
    }
}