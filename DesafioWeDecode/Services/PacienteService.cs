using AutoMapper;
using DesafioWeDecode.Data;
using DesafioWeDecode.Data.PacienteDTO;
using DesafioWeDecode.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DesafioWeDecode.Services
{
    public class PacienteService : IPacienteService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public PacienteService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> AdicionaPacienteAsync(PacienteDTO pacienteDto)
        {
            var paciente = _mapper.Map<Paciente>(pacienteDto);
            await _context.Pacientes.AddAsync(paciente);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> PacienteExisteAsync(int id) => await _context.Pacientes.FindAsync(id) is not null;
        public async Task<List<Paciente>> RecuperaPacientes()
        {
            return await _context.Pacientes
            .Include(p => p.PacienteMedicamentos)
            .ThenInclude(pm => pm.Medicamento).ToListAsync();
        }
        public async Task<Paciente?> RecuperaPacientePorIdEIncluiListaMedicações(int id)
        {
            return await _context.Pacientes
                .Include(p => p.PacienteMedicamentos)
                    .ThenInclude(pm => pm.Medicamento)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<bool> PossuiMedicamentoAsync(int idPaciente, int medicamentoId)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.PacienteMedicamentos)
                .FirstOrDefaultAsync(p => p.Id == idPaciente && p.PacienteMedicamentos.Any(pm => pm.MedicamentoId == medicamentoId));

            return paciente != null;
        }
        public async Task AdicionaMedicamentoAoPacienteAsync(int id, int medicamentoId)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == id);
            paciente!.PacienteMedicamentos.Add(new() { MedicamentoId = medicamentoId, PacienteId = id });
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();
        }
        public IEnumerable<int> ObterPacientesNovos(List<int> pacienteIds, Medicamento? medicamento) => pacienteIds.Where(pacienteId => !medicamento.PacienteMedicamentos.Any(pm => pacienteId == pm.PacienteId));
        public async Task<bool> AtualizaPacientesAsync(int idPaciente, UpdatePacienteDTO pacienteDTO)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == idPaciente);
            if (paciente is null)
                return false;

            var pacienteAtualizado = _mapper.Map(pacienteDTO, paciente);
            paciente = pacienteAtualizado;
            _context.Update(paciente);

            return await _context.SaveChangesAsync() > 0;
        }
    }

    public interface IPacienteService
    {
        Task<bool> AdicionaPacienteAsync(PacienteDTO pacienteDto);
        Task<bool> PacienteExisteAsync(int id);
        Task<List<Paciente>> RecuperaPacientes();
        Task<Paciente?> RecuperaPacientePorIdEIncluiListaMedicações(int id);
        Task<bool> PossuiMedicamentoAsync(int idPaciente, int medicamentoId);
        Task AdicionaMedicamentoAoPacienteAsync(int id, int medicamentoId);
        IEnumerable<int> ObterPacientesNovos(List<int> pacienteIds, Medicamento? medicamento);
        Task<bool> AtualizaPacientesAsync(int idPaciente, UpdatePacienteDTO pacienteDTO);
    }
}