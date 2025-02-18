using Lotus.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Basic mappings
        CreateMap<Cliente, ClienteDto>();
        CreateMap<Funcionarios, FuncionarioDto>();
        CreateMap<Agendamentos, AgendamentoDto>();
        CreateMap<User, UserDto>();

        // Custom mappings with specific configurations
        CreateMap<Cliente, ClienteDto>()
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.Telefone ?? "Sem telefone"))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? "Sem email"))
            .ForMember(dest => dest.Apelido, opt => opt.MapFrom(src => src.Apelido ?? "Sem apelido"));

        CreateMap<Agendamentos, AgendamentoDto>()
            .ForMember(dest => dest.ClienteNome, opt => opt.MapFrom(src => src.ClienteNavigation.Nome))
            .ForMember(dest => dest.FuncionarioNome, opt => opt.MapFrom(src => src.FuncionarioNavigation.Nome));

        // Reverse mappings when needed
        CreateMap<ClienteDto, Cliente>();
        CreateMap<FuncionarioDto, Funcionarios>();
    }
}
