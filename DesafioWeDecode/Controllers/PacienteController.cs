using AutoMapper;
using DesafioWeDecode.Data;
using DesafioWeDecode.Data.MedicamentoDTO;
using DesafioWeDecode.Data.PacienteDTO;
using DesafioWeDecode.Model;
using DesafioWeDecode.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioWeDecode.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _service;
        private readonly IMedicamentoService _medicamentoService;

        public PacienteController(IPacienteService service, IMedicamentoService medicamentoService)
        {
            _service = service;
            _medicamentoService = medicamentoService;
        }

        [HttpPost]
        public IActionResult AdicionaPaciente([FromBody] PacienteDTO pacienteDto)//FUNCIONA
        {
            _service.AdicionaPacienteAsync(pacienteDto);
            return Ok(pacienteDto); //CreatedAtAction(nameof(RecuperaPacientePorId),
                                    //new { id = paciente.Id }, paciente);// entender melhor o pq deste retorno 
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaPacientePorId(int id)
        {
            Paciente paciente = _service.RecuperaPacientePorId(id);
            if (paciente == null) return NotFound();
            return Ok(paciente);
        }//FUNCIONA

        //RECUPERA TODOS OS PACIENTES

        [HttpPost("{pacienteId}")]
        public async Task<IActionResult> AdicionaMedicamentosNoPaciente(int pacienteId, [FromBody] IEnumerable<MedicamentoDTO> medicamentos)
        {
            var paciente = _service.RecuperaPacientePorId(pacienteId);
            if (paciente is null)
                return NotFound();

            if (medicamentos.Any())
                return BadRequest();

            List<int> idsValidos = new();
            List<int> idsInvalidos = new();

            foreach (var medicamento in medicamentos)
            {
                var idMedicamento = medicamento.Id.Value;//value, pq é um int nullable

                var existe = await _medicamentoService.MedicamentoExisteAsync(idMedicamento);
                if (existe)
                    idsValidos.Add(idMedicamento);
                else
                    idsInvalidos.Add(idMedicamento);
            }

            if (idsValidos.Any())
            {
                for (int i = 0; i < idsValidos.Count; i++)
                {
                    await _service.AdicionaMedicamentoAoPacienteAsync(pacienteId, idsValidos[i]);
                }
                return Ok();
            }

            else 
            {
                return NotFound(new { mensagem = "Alguns medicamentos não foram encontrados", idsInvalidos });
            }
            
        }
    }
}

