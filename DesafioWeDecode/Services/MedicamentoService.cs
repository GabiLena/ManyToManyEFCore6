using AutoMapper;
using DesafioWeDecode.Data;
using DesafioWeDecode.Data.MedicamentoDTO;
using DesafioWeDecode.Model;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> AdicionaMedicamentoAsync(MedicamentoDTO medicamentoDto)
        {
            var medicamento = _mapper.Map<Medicamento>(medicamentoDto);
            await _context.Medicamentos.AddAsync(medicamento);
            return await _context.SaveChangesAsync() > 0;// save changes retorna um int de entidades modificadas, se for maior que zero significa que foi modificada
        }

        public IEnumerable<Medicamento> SelecionaMedicamentos(int skip, int take) => _context.Medicamentos.Skip(skip).Take(take);

        public Medicamento RecuperaMedicamentoPorId(int id)
        {
            var medicamento = _context.Medicamentos
                 .FirstOrDefault(m => m.Id == id);
            return medicamento;
        }

        public async Task<bool> AtualizaMedicamentoAsync(int id, MedicamentoDTO medicamentoDto)
        {
            var medicamento = await _context.Medicamentos.FirstOrDefaultAsync(m => m.Id == id);
            if (medicamento is null)
                return false;

            _mapper.Map(medicamentoDto, medicamento);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> MedicamentoExisteAsync(int id) => await _context.Medicamentos.FindAsync(id) is not null;
    }

    public interface IMedicamentoService
    {
        Task<bool> AdicionaMedicamentoAsync(MedicamentoDTO medicamentoDto);
        IEnumerable<Medicamento> SelecionaMedicamentos(int skip, int take);
        Medicamento RecuperaMedicamentoPorId(int id);
        Task<bool> AtualizaMedicamentoAsync(int id, MedicamentoDTO medicamentoDto);
        Task<bool> MedicamentoExisteAsync(int id);
    }

}
