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
        }
    }
}
