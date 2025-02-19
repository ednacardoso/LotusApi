namespace Lotus.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> GetAllClientes();
        Task<ClienteDto> GetClienteByUserId(int userId);
        Task<ClienteDto> AddCliente(Cliente cliente);
        Task<ClienteDto> UpdateCliente(int id, Cliente clienteAtualizado);
        Task DeleteCliente(int id);
        Task<ClienteDto> UpdateProfile(int userId, UpdateProfileRequest request);
    }
}
