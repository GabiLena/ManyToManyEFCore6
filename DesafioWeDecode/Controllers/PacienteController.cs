using AutoMapper;
using DesafioWeDecode.Data;
using DesafioWeDecode.Data.PacienteDTO;
using DesafioWeDecode.Model;
using Microsoft.AspNetCore.Mvc;

namespace DesafioWeDecode.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PacienteController : ControllerBase
    {

        private AppDbContext _context;
        private IMapper _mapper;

        public PacienteController(AppDbContext context, IMapper mapper)//acesso a DB e mapeamento
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionaPaciente([FromBody] CreatePacienteDTO pacienteDto) 
        {
            var paciente = _mapper.Map<Paciente>(pacienteDto);
            _context.Pacientes.Add(paciente);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperaPacientePorId),
                new { id = paciente.Id }, paciente);// entender melhor o pq deste retorno 
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaPacientePorId(int id) 
        {
            Paciente paciente = _context.Pacientes.FirstOrDefault(p => p.Id == id);
            if(paciente == null) return NotFound();
            return Ok(paciente);
        }
    }
}

