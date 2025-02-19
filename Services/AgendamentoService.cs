using Lotus.Data;
using Lotus.Exceptions;
using AutoMapper;
using Lotus.Interfaces;
using Lotus.Models.DTOs.Requests;
using Microsoft.EntityFrameworkCore;


public class AgendamentoService : IAgendamentoService
{
    private readonly MLotusContext _context;
    private readonly IMapper _mapper;

    public AgendamentoService(MLotusContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AgendamentoDto>> GetAllAgendamentos()
    {
        var agendamentos = await _context.Agendamentos
            .Include(a => a.ClienteNavigation)
            .Include(a => a.FuncionarioNavigation)
            .ToListAsync();

        return _mapper.Map<IEnumerable<AgendamentoDto>>(agendamentos);
    }

    public async Task<IEnumerable<AgendamentoDto>> GetAgendamentosByUser(int userId, string userRole)
    {
        IQueryable<Agendamentos> query = _context.Agendamentos
            .Include(a => a.ClienteNavigation)
            .Include(a => a.FuncionarioNavigation);

        if (userRole == "Cliente")
        {
            var clienteExistente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (clienteExistente == null)
                throw new NotFoundException("Cliente não cadastrado");

            query = query.Where(a => a.ClienteId == clienteExistente.Id);
        }

        var agendamentos = await query.Select(a => new AgendamentoDto
        {
            Id = a.Id,
            DataAgendamento = a.DataAgendamento,
            Status = a.Status,
            Observacoes = a.Observacoes,
            MotivoCancelamento = a.MotivoCancelamento,
            ClienteId = a.ClienteId,
            ClienteNome = a.ClienteNavigation.Nome,
            FuncionarioId = a.FuncionarioId,
            FuncionarioNome = a.FuncionarioNavigation.Nome
        }).ToListAsync();

        return agendamentos;
    }

    public async Task<AgendamentoDto> CreateAgendamento(Agendamentos agendamento)
    {
        await ValidateAgendamento(agendamento);

        agendamento.Status = "Ativo";
        await _context.Agendamentos.AddAsync(agendamento);
        await _context.SaveChangesAsync();

        return await GetAgendamentoById(agendamento.Id);
    }

    public async Task<AgendamentoDto> UpdateAgendamento(int id, AlterarAgendamentoRequest request)
    {
        var agendamento = await _context.Agendamentos
            .Include(a => a.ClienteNavigation)
            .Include(a => a.FuncionarioNavigation)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (agendamento == null)
            throw new NotFoundException("Agendamento não encontrado");

        if (request.DataAgendamento.HasValue)
            agendamento.DataAgendamento = request.DataAgendamento.Value;

        if (!string.IsNullOrWhiteSpace(request.Observacoes))
            agendamento.Observacoes = request.Observacoes;

        await _context.SaveChangesAsync();
        return await GetAgendamentoById(id);
    }

    public async Task<AgendamentoDto> CancelarAgendamento(int id, string motivo)
    {
        var agendamento = await _context.Agendamentos.FindAsync(id);

        if (agendamento == null)
            throw new NotFoundException("Agendamento não encontrado");

        agendamento.Status = "Cancelado";
        agendamento.MotivoCancelamento = motivo;

        await _context.SaveChangesAsync();
        return await GetAgendamentoById(id);
    }

    private async Task<AgendamentoDto> GetAgendamentoById(int id)
    {
        return await _context.Agendamentos
            .Include(a => a.ClienteNavigation)
            .Include(a => a.FuncionarioNavigation)
            .Where(a => a.Id == id)
            .Select(a => new AgendamentoDto
            {
                Id = a.Id,
                DataAgendamento = a.DataAgendamento,
                Status = a.Status,
                Observacoes = a.Observacoes,
                MotivoCancelamento = a.MotivoCancelamento,
                ClienteId = a.ClienteId,
                ClienteNome = a.ClienteNavigation.Nome,
                FuncionarioId = a.FuncionarioId,
                FuncionarioNome = a.FuncionarioNavigation.Nome
            })
            .FirstOrDefaultAsync();
    }

    private async Task ValidateAgendamento(Agendamentos agendamento)
    {
        if (!await _context.Clientes.AnyAsync(c => c.Id == agendamento.ClienteId))
            throw new ValidationException($"Cliente não encontrado: {agendamento.ClienteId}");

        if (!await _context.Funcionarios.AnyAsync(f => f.Id == agendamento.FuncionarioId))
            throw new ValidationException($"Funcionário não encontrado: {agendamento.FuncionarioId}");
    }
}
