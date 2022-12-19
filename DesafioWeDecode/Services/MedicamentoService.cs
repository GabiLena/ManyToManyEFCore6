using AutoMapper;
using DesafioWeDecode.Data;
using DesafioWeDecode.Data.MedicamentoDTO;
using DesafioWeDecode.Model;

namespace DesafioWeDecode.Services
{
    public class MedicamentoService : IMedicamentoService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public MedicamentoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AdicionaMedicamentoAsync(CreateMedicamentoDTO medicamentoDto)
        {
            var medicamento = _mapper.Map<Medicamento>(medicamentoDto);
            await _context.Medicamentos.AddAsync(medicamento);
            return await _context.SaveChangesAsync() > 0;
        }

        public IEnumerable<Medicamento> SelecionaMedicamentos(int skip, int take) => _context.Medicamentos.Skip(skip).Take(take);
    }

    public interface IMedicamentoService
    {
        Task<bool> AdicionaMedicamentoAsync(CreateMedicamentoDTO medicamentoDto);
        IEnumerable<Medicamento> SelecionaMedicamentos(int skip, int take);
    }
}
