using System.Collections.Generic;
using System.Threading.Tasks;

public interface IClienteService
{
    Task<IEnumerable<ClienteDto>> GetAllClientes();
    Task<ClienteDto> GetClienteByUserId(int userId);
    Task<ClienteDto> AddCliente(Cliente cliente);
}
