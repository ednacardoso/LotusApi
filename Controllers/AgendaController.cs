using Lotus.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AgendamentosController : ControllerBase
{
    private readonly IAgendamentoService _agendamentoService;
    private readonly ILogger<AgendamentosController> _logger;

    public AgendamentosController(IAgendamentoService agendamentoService, ILogger<AgendamentosController> logger)
    {
        _agendamentoService = agendamentoService;
        _logger = logger;
    }

    [HttpGet("usuario")]
    public async Task<IActionResult> GetAgendamentosPorUsuario()
    {
        try
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var agendamentos = await _agendamentoService.GetAgendamentosByUser(userId, userRole);
            return Ok(new { values = agendamentos });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAgendamento([FromBody] Agendamentos agendamento)
    {
        try
        {
            var result = await _agendamentoService.CreateAgendamento(agendamento);
            return CreatedAtAction(nameof(GetAgendamentosPorUsuario), result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}/alterar")]
    public async Task<IActionResult> UpdateAgendamento(int id, [FromBody] AlterarAgendamentoRequest request)
    {
        try
        {
            var result = await _agendamentoService.UpdateAgendamento(id, request);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/cancelar")]
    public async Task<IActionResult> CancelarAgendamento(int id, [FromBody] CancelarAgendamentoRequest request)
    {
        try
        {
            var result = await _agendamentoService.CancelarAgendamento(id, request.Motivo);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
