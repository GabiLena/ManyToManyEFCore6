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
        public async Task<bool> PacienteExisteAsync(int id) => await _context.Pacientes.FindAsync(id) is not null;



        public async Task AdicionaMedicamentoAoPacienteAsync(int id, int medicamentoId)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == id);
            paciente!.PacienteMedicamentos.Add(new() { MedicamentoId = medicamentoId, PacienteId = id });
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> AdicionaPacienteAsync(PacienteDTO pacienteDto)
        {
            var paciente = _mapper.Map<Paciente>(pacienteDto);
            await _context.Pacientes.AddAsync(paciente);
            return await _context.SaveChangesAsync() > 0;
        }
        public Paciente? RecuperaPacientePorId(int id)
        {
            var paciente = _context.Pacientes
                .Include(p => p.PacienteMedicamentos)//para retornar informar informação, tipo lazy property
                    .ThenInclude(pm => pm.Medicamento)
                .FirstOrDefault(p => p.Id == id);

            return paciente;
        }
        public async Task<List<Paciente>> RecuperaPacientes(int skip, int take)
        {
            return await _context.Pacientes
            .Include(p => p.PacienteMedicamentos)
            .ThenInclude(pm => pm.Medicamento).Skip(skip).Take(10).ToListAsync();
        }

        public async Task<bool> PossuiMedicamentoAsync(int idPaciente, int medicamentoId)
        {
            var paciente = await _context.Pacientes
                .Include(p => p.PacienteMedicamentos)
                .FirstOrDefaultAsync(p => p.Id == idPaciente && p.PacienteMedicamentos.Any(pm => pm.MedicamentoId == medicamentoId));

            return paciente != null;
        }
    }

    public interface IPacienteService
    {
        Task<bool> PacienteExisteAsync(int id);
        Task<bool> AdicionaPacienteAsync(PacienteDTO pacienteDto);
        Task<List<Paciente>> RecuperaPacientes(int skip, int take);
        Paciente? RecuperaPacientePorId(int id);
        Task AdicionaMedicamentoAoPacienteAsync(int id, int medicamentoId);
        Task<bool> PossuiMedicamentoAsync(int idPaciente, int medicamentoId);
    }
}