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
    public class MedicamentosController : ControllerBase
    {

        private readonly IMedicamentoService _service;
        private readonly IPacienteService _pacienteService;
        private readonly IMapper _mapper;

        public MedicamentosController(IMedicamentoService service, IPacienteService pacienteservice, IMapper mapper)
        {
            _service = service;
            _pacienteService = pacienteservice;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AdicionaMedicamento([FromBody] MedicamentoDTO medicamentoDto)
        {
            var foiAdicionado = await _service.AdicionaMedicamentoAsync(medicamentoDto);
            if (foiAdicionado)
                return Ok("Medicamento adicionado com sucesso");

            return StatusCode(304);
        }

        [HttpGet]
        public async Task<ActionResult<List<Medicamento>>> RecuperaMedicamentos()
        {
            var medicamentos = await _service.RecuperaMedicamentosAsync();
            if (medicamentos is null)
                return StatusCode(500, "Lista é nula.");

            if (!medicamentos.Any())
                return NotFound("Não existem medicamentos.");

            List<MedicamentoDTO> medicamentoDtos = new();
            for (int i = 0; i < medicamentos.Count; i++)
            {
                var medicamentoDto = _mapper.Map<MedicamentoDTO>(medicamentos[i]);
                medicamentoDtos.Add(medicamentoDto);
            }

            return Ok(medicamentoDtos);

        }

        [HttpGet("{id}")]
        public ActionResult<MedicamentoDTO> RecuperaMedicacaoPorId(int id) 
        {
            var medicamento = _service.RecuperaMedicamentoPorId(id);
            if (medicamento is null)
            {
                return NotFound();
            }
            var medicamentoDTO = _mapper.Map<MedicamentoDTO>(medicamento);
            return Ok(medicamentoDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaMedicamento(int id, [FromBody] UpdateMedicamentoDTO medicamentoDto)
        {
            var medicamentoFoiAtualizado = await _service.AtualizaMedicamentoAsync(id, medicamentoDto);
            if (!medicamentoFoiAtualizado)
                return BadRequest("Não foi possível atualizar dados de medicamento.");

            return Ok("Medicamento atualizado com sucesso!");
        }

        [HttpPost("{medicamentoId}")]
        public async Task<IActionResult> AdicionaPacientesNoMedicamento(int medicamentoId, [FromBody] List<int> pacientesIds)
        {
            var medicamento = _service.RecuperaMedicamentoPorId(medicamentoId);
            if (medicamento is null)
                return NotFound("Medicamento não existe.");

            if (!pacientesIds.Any())
                return BadRequest("Lista de pacientes vazia.");

            var pacienteIdsNovos = _pacienteService.ObterPacientesNovos(pacientesIds, medicamento);
            if (!pacienteIdsNovos.Any())
                return StatusCode(304,"O paciente já possui esses medicamentos.");

            List<int> idsValidos = new();
            List<int> idsInvalidos = new();

            foreach (var pacienteId in pacienteIdsNovos)
            {
                var existe = await _pacienteService.PacienteExisteAsync(pacienteId);
                if (existe)
                    idsValidos.Add(pacienteId);
                else
                    idsInvalidos.Add(pacienteId);
            }

            if (!idsValidos.Any())
                return NotFound(new { mensagem = "Estes pacientes não foram encontrados: ", idsInvalidos });

            for (int i = 0; i < idsValidos.Count; i++)
                await _service.AdicionaPacienteAMedicamentoAsync(medicamentoId, idsValidos[i]);

            if (idsInvalidos.Any())
                return Ok(new { mensagem = "Alguns pacientes não foram encontrados: ", idsInvalidos });

            return Ok("Todos os pacientes foram adicionados.");
        }
    }
}
