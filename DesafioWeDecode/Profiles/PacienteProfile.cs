using AutoMapper;
using DesafioWeDecode.Data.PacienteDTO;
using DesafioWeDecode.Model;

namespace DesafioWeDecode.Profiles
{
    public class PacienteProfile : Profile
    {
        public PacienteProfile()
        {
            CreateMap<CreatePacienteDTO, Paciente>();
            CreateMap<UpdatePacienteDTO, Paciente>();
            CreateMap<ReadPacienteDTO, Paciente>();
        }
    }
}
