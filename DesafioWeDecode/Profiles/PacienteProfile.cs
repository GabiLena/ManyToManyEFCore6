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
        }
    }
}
