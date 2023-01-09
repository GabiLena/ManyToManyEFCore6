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
        private readonly IMapper _mapper;
        private readonly IPacienteService _service;
        private readonly IMedicamentoService _medicamentoService;

        public PacienteController(IPacienteService service, IMedicamentoService medicamentoService, IMapper mapper)
        {
            _service = service;
            _medicamentoService = medicamentoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona um paciente no banco de dados
        /// </summary>
        /// <param name="pacienteDto">Objeto com os campos necessários para a criação de um paciente</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AdicionaPaciente([FromBody] PacienteDTO pacienteDto)
        {
            await _service.AdicionaPacienteAsync(pacienteDto);
            if (pacienteDto is null)
            {
                return BadRequest("Paciente não foi adicionado.");
            }
                return Ok(pacienteDto); //CreatedAtAction(nameof(RecuperaPacientePorId),
                        //new { id = paciente.Id }, paciente);// entender melhor o pq deste retorno 
        }

        /// <summary>
        /// Recupera pacientes
        /// </summary>
        /// <param name="skip">Método de paginação que pula 0 itens</param>
        /// <param name="take">Método de paginação que lê quantidade de itens informada</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RecuperaPacientes(int skip, int take)
        {
            var pacientes = await _service.RecuperaPacientes(skip, take);
            if (pacientes is null)
                return StatusCode(500, "Lista é nula");

            if (!pacientes.Any())
                return NotFound();

            List<ReadPacienteDTO> pacienteDTOs = new();
            for (int i = 0; i < pacientes.Count; i++)
            {
                var pacienteDto = _mapper.Map<ReadPacienteDTO>(pacientes[i]);
                pacienteDTOs.Add(pacienteDto);
            }

            return Ok(pacienteDTOs);
        }

        /// <summary>
        /// Recupera paciente por id
        /// </summary>
        /// <param name="id">para identificar paciente pelo Id</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ReadPacienteDTO> RecuperaPacientePorId(int id)
        {
            var paciente = _service.RecuperaPacientePorId(id);
            if (paciente is null)
            {
                return NotFound();
            }
            var pacienteDto = _mapper.Map<ReadPacienteDTO>(paciente);
            return Ok(pacienteDto);
        }

        /// <summary>
        /// Adiciona medicamentos em paciente
        /// </summary>
        /// <param name="pacienteId">Recupera paciente a ser adicionado medicamentos</param>
        /// <param name="medicamentos">Lista de medicamentos a ser adicionado em paciente</param>
        /// <returns>IActionResult</returns>
        [HttpPost("{pacienteId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AdicionaMedicamentosNoPaciente(int pacienteId, [FromBody] List<MedicamentoDTO> medicamentos)//FUNCIONA
        {
            var paciente = _service.RecuperaPacientePorId(pacienteId);
            if (paciente is null)
                return NotFound();

            if (!medicamentos.Any())
                return BadRequest();

            List<int> idsValidos = new();
            List<int> idsInvalidos = new();
            //adicionar validação para ver se já existe medicamento no paciente

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
                return NotFound(new { mensagem = "Estes medicamentos não foram encontrados: ", idsInvalidos });
            }

        }
    }
}

