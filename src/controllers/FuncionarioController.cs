using Lotus.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class FuncionariosController : ControllerBase
{
    private readonly IFuncionarioService _funcionarioService;
    private readonly ILogger<FuncionariosController> _logger;

    public FuncionariosController(IFuncionarioService funcionarioService, ILogger<FuncionariosController> logger)
    {
        _funcionarioService = funcionarioService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetFuncionarios()
    {
        var funcionarios = await _funcionarioService.GetAllFuncionarios();
        return Ok(funcionarios);
    }

    [HttpGet("funcionario/{userId}")]
    public async Task<IActionResult> GetFuncionarioByUserId(int userId)
    {
        try
        {
            var funcionario = await _funcionarioService.GetFuncionarioByUserId(userId);
            return Ok(funcionario);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddFuncionario([FromBody] Funcionarios novoFuncionario)
    {
        try
        {
            var funcionarioDto = await _funcionarioService.AddFuncionario(novoFuncionario);
            return CreatedAtAction(nameof(GetFuncionarios), new { id = funcionarioDto.Id }, funcionarioDto);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
