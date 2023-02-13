using AutoMapper;
using DesafioWeDecode.Data.MedicamentoDTO;
using DesafioWeDecode.Data.PacienteDTO;
using DesafioWeDecode.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DesafioWeDecode.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPacienteService _service;
        private readonly IMedicamentoService _medicamentoService;

        public PacientesController(IPacienteService service, IMedicamentoService medicamentoService, IMapper mapper)
        {
            _service = service;
            _medicamentoService = medicamentoService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AdicionaPaciente([FromBody] PacienteDTO pacienteDto)
        {
            bool foiAdicionado = await _service.AdicionaPacienteAsync(pacienteDto);

            if (foiAdicionado)
                return Ok("Paciente foi adicionado com sucesso.");

            return StatusCode(304, "Medicamento não foi adicionado");
        }

        [HttpGet]
        public async Task<IActionResult> RecuperaPacientes()
        {
            var pacientes = await _service.RecuperaPacientes();
            if (pacientes is null)
                return StatusCode(500, "Lista é nula");

            if (!pacientes.Any())
                return NotFound();

            List<PacienteDTO> pacienteDTOs = new();
            for (int i = 0; i < pacientes.Count; i++)
            {
                var pacienteDto = _mapper.Map<PacienteDTO>(pacientes[i]);
                pacienteDTOs.Add(pacienteDto);
            }

            return Ok(pacienteDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PacienteDTO>> RecuperaPacientePorId(int id)
        {
            var paciente = await _service.RecuperaPacientePorIdEIncluiListaMedicações(id);
            if (paciente is null)
            {
                return NotFound();
            }
            var pacienteDto = _mapper.Map<PacienteDTO>(paciente);
            return Ok(pacienteDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaPaciente(int id, [FromBody] UpdatePacienteDTO pacienteDTO)
        {
            var pacienteFoiAtualizado = await _service.AtualizaPacientesAsync(id,pacienteDTO);
            if (!pacienteFoiAtualizado)
                return BadRequest("Não foi possível atualizar dados de paciente.");

            return Ok("Paciente atualizado com sucesso!");
        }

        [HttpPost("{pacienteId}")]
        public async Task<IActionResult> AdicionaMedicamentosNoPaciente(int pacienteId, [FromBody] List<int> medicamentoIds)
        {
            var paciente = _service.RecuperaPacientePorIdEIncluiListaMedicações(pacienteId);
            if (paciente is null)
                return NotFound("Paciente não existe.");

            if (!medicamentoIds.Any())
                return BadRequest("Lista de medicamentos vazia.");

            var medicamentoIdsNovos = _medicamentoService.ObterMedicamentosNovos(medicamentoIds, paciente);
            if (!medicamentoIdsNovos.Any())
                return StatusCode(304, "O paciente já possui esses medicamentos.");

            List<int> idsValidos = new();
            List<int> idsInvalidos = new();

            foreach (var medicamentoId in medicamentoIdsNovos)
            {
                var existe = await _medicamentoService.MedicamentoExisteAsync(medicamentoId);
                if (existe)
                    idsValidos.Add(medicamentoId);
                else
                    idsInvalidos.Add(medicamentoId);
            }

            if (!idsValidos.Any())
                return NotFound(new { mensagem = "Estes medicamentos não foram encontrados: ", idsInvalidos });

            for (int i = 0; i < idsValidos.Count; i++)
                await _service.AdicionaMedicamentoAoPacienteAsync(pacienteId, idsValidos[i]);

            if (idsInvalidos.Any())
                return Ok(new { mensagem = "Alguns medicamentos não foram encontrados: ", idsInvalidos });

            return Ok("Todos os medicamentos foram adicionados."); 
        }
    }
}

