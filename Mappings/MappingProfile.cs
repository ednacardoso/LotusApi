using AutoMapper;
using Lotus.Models;
using Lotus.Models.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Cliente, ClienteDto>();
        CreateMap<Funcionarios, FuncionarioDto>();
        CreateMap<Agendamentos, AgendamentoDto>();
        CreateMap<User, UserDto>();
        
    }
}
