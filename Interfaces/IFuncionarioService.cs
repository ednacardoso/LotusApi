using System.Collections.Generic;
using System.Threading.Tasks;

public interface IFuncionarioService
{
    Task<IEnumerable<FuncionarioDto>> GetAllFuncionarios();
    Task<FuncionarioDto> GetFuncionarioByUserId(int userId);
    Task<FuncionarioDto> AddFuncionario(Funcionarios funcionario);
}
