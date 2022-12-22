using AutoMapper;
using DesafioWeDecode.Data;
using DesafioWeDecode.Data.PacienteDTO;
using DesafioWeDecode.Model;
using Microsoft.EntityFrameworkCore;

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
        public Paciente RecuperaPacientePorId(int id) 
        {
            var paciente = _context.Pacientes
                .FirstOrDefault(p => p.Id == id);
            return paciente;
        }

    }

    public interface IPacienteService
    {
        Task<bool> AdicionaPacienteAsync(PacienteDTO pacienteDto);
        Paciente RecuperaPacientePorId(int id);
        Task AdicionaMedicamentoAoPacienteAsync(int id, int medicamentoId);
    }
}