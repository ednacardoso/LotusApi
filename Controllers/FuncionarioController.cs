using Lotus.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lotus.Exceptions;
using Lotus.Models.DTOs.Requests;
using Lotus.Interfaces;

namespace Lotus.Controllers
{
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

        [Authorize(Roles = "funcionario")]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var funcionarioDto = await _funcionarioService.UpdateProfile(userId, request);
                return Ok(funcionarioDto);
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
            catch (Lotus.Exceptions.ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
