using System.Collections.Generic;
using System.Threading.Tasks;

public class FuncionarioService : IFuncionarioService
{
    private readonly MLotusContext _context;
    private readonly IMapper _mapper;

    public FuncionarioService(MLotusContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FuncionarioDto>> GetAllFuncionarios()
    {
        var funcionarios = await _context.Funcionarios
            .Select(f => new FuncionarioDto
            {
                Id = f.Id,
                Nome = f.Nome,
                Especialidade = f.Especialidade ?? "Sem especialidade",
                Telefone = f.Telefone ?? "Sem telefone",
                Email = f.Email ?? "Sem email",
                Apelido = f.Apelido ?? "Sem apelido"
            })
            .ToListAsync();

        return funcionarios;
    }

    public async Task<FuncionarioDto> GetFuncionarioByUserId(int userId)
    {
        var funcionario = await _context.Funcionarios
            .Include(f => f.User)
            .FirstOrDefaultAsync(f => f.UserId == userId);

        return _mapper.Map<FuncionarioDto>(funcionario);
    }

    public async Task<FuncionarioDto> AddFuncionario(Funcionarios funcionario)
    {
        funcionario.DataNascimento = funcionario.DataNascimento?.ToUniversalTime();
        await _context.Funcionarios.AddAsync(funcionario);
        await _context.SaveChangesAsync();

        return _mapper.Map<FuncionarioDto>(funcionario);
    }
}
