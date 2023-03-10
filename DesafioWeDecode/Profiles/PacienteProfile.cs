using AutoMapper;
using DesafioWeDecode.Data.PacienteDTO;
using DesafioWeDecode.Model;

namespace DesafioWeDecode.Profiles
{
    public class PacienteProfile : Profile
    {
        public PacienteProfile()
        {
            CreateMap<PacienteDTO, Paciente>();
            CreateMap<UpdatePacienteDTO, Paciente>();
            CreateMap<Paciente, PacienteDTO>()
                .ForMember(dest => dest.Medicamentos,
                m => m.MapFrom(src => src.PacienteMedicamentos.Select(pm => pm.Medicamento.Nome)));
        }
    }
}
