namespace Lotus.Interfaces
{
    public interface IFuncionarioService
    {
        Task<IEnumerable<FuncionarioDto>> GetAllFuncionarios();
        Task<FuncionarioDto> GetFuncionarioByUserId(int userId);
        Task<FuncionarioDto> AddFuncionario(Funcionarios funcionario);
        Task<FuncionarioDto> UpdateProfile(int userId, UpdateProfileRequest request);
    }
}

