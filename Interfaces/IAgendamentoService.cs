using Lotus.Models.DTOs.Requests;

namespace Lotus.Interfaces
{
    public interface IAgendamentoService
    {
        Task<IEnumerable<AgendamentoDto>> GetAgendamentosByUser(int userId, string userRole);
        Task<AgendamentoDto> CreateAgendamento(Agendamentos agendamento);
        Task<AgendamentoDto> UpdateAgendamento(int id, AlterarAgendamentoRequest request);
        Task<AgendamentoDto> CancelarAgendamento(int id, string motivo);
        Task<IEnumerable<AgendamentoDto>> GetAllAgendamentos();
    }
}
