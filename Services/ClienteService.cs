using System.Collections.Generic;
using System.Threading.Tasks;
using Lotus.Models;
using Lotus.Validators;
using Lotus.Exceptions;
using Lotus.Data;
using AutoMapper;
using Lotus.Interfaces;
using Microsoft.EntityFrameworkCore;




public class ClienteService : IClienteService
{
    private readonly MLotusContext _context;
    private readonly IMapper _mapper;

    public ClienteService(MLotusContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClienteDto> UpdateProfile(int userId, UpdateProfileRequest request)
    {
        var cliente = await _context.Clientes
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cliente == null)
            throw new NotFoundException("Cliente não encontrado");

        cliente.Nome = request.Nome;
        cliente.Email = request.Email;
        cliente.Telefone = request.Telefone;
        cliente.Apelido = request.Apelido;
        cliente.DataNascimento = request.DataNascimento;

        await _context.SaveChangesAsync();

        return _mapper.Map<ClienteDto>(cliente);
    }



    public async Task<IEnumerable<ClienteDto>> GetAllClientes()
    {
        var clientes = await _context.Clientes
            .Select(c => new ClienteDto
            {
                Id = c.Id,
                Nome = c.Nome,
                Telefone = c.Telefone ?? "Sem telefone",
                Email = c.Email ?? "Sem email",
                Apelido = c.Apelido ?? "Sem apelido",
                DataNascimento = c.DataNascimento
            })
            .ToListAsync();

        return clientes;
    }

    public async Task<ClienteDto> GetClienteByUserId(int userId)
    {
        var cliente = await _context.Clientes
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cliente == null)
            throw new NotFoundException("Cliente não encontrado");

        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> AddCliente(Cliente cliente)
    {
        if (await _context.Clientes.AnyAsync(c => c.Email == cliente.Email))
            throw new ValidationException("Email já cadastrado");

        cliente.DataNascimento = cliente.DataNascimento?.ToUniversalTime();

        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();

        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task<ClienteDto> UpdateCliente(int id, Cliente clienteAtualizado)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            throw new NotFoundException("Cliente não encontrado");

        cliente.Nome = clienteAtualizado.Nome;
        cliente.Email = clienteAtualizado.Email;
        cliente.Telefone = clienteAtualizado.Telefone;
        cliente.Apelido = clienteAtualizado.Apelido;
        cliente.DataNascimento = clienteAtualizado.DataNascimento?.ToUniversalTime();

        await _context.SaveChangesAsync();
        return _mapper.Map<ClienteDto>(cliente);
    }

    public async Task DeleteCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            throw new NotFoundException("Cliente não encontrado");

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
    }
}
