using AutoMapper;
using DesafioWeDecode.Data.MedicamentoDTO;
using DesafioWeDecode.Model;

namespace DesafioWeDecode.Profiles
{
    public class MedicacaoProfile : Profile
    {
        public MedicacaoProfile()
        {
            CreateMap<MedicamentoDTO, Medicamento>();
            CreateMap<UpdateMedicamentoDTO, Medicamento>();
            CreateMap<Medicamento, ReadMedicamentoDTO>()
                .ForMember(dest => dest.Pacientes,
                m => m.MapFrom(src => src.PacienteMedicamentos.Select(pm => pm.Paciente.Nome)));
        }
    }
}
