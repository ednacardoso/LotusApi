using Lotus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
    {
        _clienteService = clienteService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetClientes()
    {
        var clientes = await _clienteService.GetAllClientes();
        return Ok(clientes);
    }

    [HttpGet("cliente/{userId}")]
    public async Task<IActionResult> GetClienteByUserId(int userId)
    {
        try
        {
            var cliente = await _clienteService.GetClienteByUserId(userId);
            return Ok(cliente);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddCliente([FromBody] Cliente novoCliente)
    {
        try
        {
            var clienteDto = await _clienteService.AddCliente(novoCliente);
            return CreatedAtAction(nameof(GetClientes), new { id = clienteDto.Id }, clienteDto);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCliente(int id, [FromBody] Cliente clienteAtualizado)
    {
        try
        {
            var clienteDto = await _clienteService.UpdateCliente(id, clienteAtualizado);
            return Ok(clienteDto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "cliente")]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var clienteDto = await _clienteService.UpdateProfile(userId, request);
            return Ok(clienteDto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(int id)
    {
        try
        {
            await _clienteService.DeleteCliente(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
