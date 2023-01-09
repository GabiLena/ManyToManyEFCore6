using AutoMapper;
using DesafioWeDecode.Data;

namespace DesafioWeDecode.Services
{
    public class PacienteMedicamentoService : IPacienteMedicamentoService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public PacienteMedicamentoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> PacienteExisteEmMedicamentoAsync(int id)
        {
            var paciente = _context.Medicamentos
            return await _context.Medicamentos.Where(m => m.PacienteMedicamentos.);
        }
    }

    public interface IPacienteMedicamentoService
    {
    }
}
