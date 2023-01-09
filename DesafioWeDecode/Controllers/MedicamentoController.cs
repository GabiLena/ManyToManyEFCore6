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
    public class MedicamentoController : ControllerBase
    {

        private readonly IMedicamentoService _service;
        private readonly IPacienteService _pacienteService;
        private readonly IMapper _mapper;

        public MedicamentoController(IMedicamentoService service, IPacienteService pacienteservice, IMapper mapper)
        {
            _service = service;
            _pacienteService = pacienteservice;
            _mapper = mapper;
        }

        /// <summary>
        /// Adiciona medicamento ao banco de dados
        /// </summary>
        /// <param name="medicamentoDto">Objeto com os campos necessarios para criação de um medicamento </param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AdicionaMedicamento([FromBody] MedicamentoDTO medicamentoDto) //ADICIONA 
        {
            var foiAdicionado = await _service.AdicionaMedicamentoAsync(medicamentoDto);//pq esse é async e o debaixo não?
            if (foiAdicionado)
                return Ok("Medicamento adicionado com sucesso");

            return StatusCode(304);
        }

        /// <summary>
        /// Recupera medicamentos
        /// </summary>
        /// <param name="skip">Método de paginação que pula quantidade desejada de resultados da pesquisa</param>
        /// <param name="take">Método de paginação que retorna quantidade desejada de resultados da pesquisa</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<Medicamento>> RecuperaMedicamentos([FromQuery] int skip = 0, [FromQuery] int take = 10)// READ TODOS 
        {
            var medicamentos = _service.SelecionaMedicamentos(skip, take);
            if (medicamentos is null)
                return StatusCode(500, "Lista é nula");

            if (!medicamentos.Any())
                return NotFound();

            return Ok(medicamentos);
        }

        /// <summary>
        /// Recupera medicamento pelo id
        /// </summary>
        /// <param name="id">Parâmetro para identificar medicamento a ser recuperado</param>
        /// <returns>ActionResult</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ReadMedicamentoDTO> RecuperaMedicacaoPorId(int id) //READ POR ID
        {
            var medicamento = _service.RecuperaMedicamentoPorId(id);
            if (medicamento is null)
            {
                return NotFound();
            }
            var medicamentoDTO = _mapper.Map<ReadMedicamentoDTO>(medicamento);
            return Ok(medicamentoDTO);
        }

        /// <summary>
        /// Atualiza medicamento
        /// </summary>
        /// <param name="id">Parâmetro para identificar medicamento a ser atualizado</param>
        /// <param name="medicamentoDto">Obejto com os campos necessários para atualizar medicamento</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AtualizaMedicamento(int id, [FromBody] UpdateMedicamentoDTO medicamentoDto)
        {
            var medicamentoFoiAtualizado = await _service.AtualizaMedicamentoAsync(id, medicamentoDto);
            if (!medicamentoFoiAtualizado)
                return NotFound();

            return Ok(medicamentoDto);
        }

        /// <summary>
        /// Adiciona Pacientes em medicamento
        /// </summary>
        /// <param name="medicamentoId">Parâmetro para identificar medicamento a ser atualizado</param>
        /// <param name="pacientes">Lista de pacientes a serem adicionados em medicamento</param>
        /// <returns>IactionResult</returns>
        [HttpPost("{medicamentoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AdicionaPacienteAMedicamento(int medicamentoId, [FromBody] List<PacienteDTO> pacientes)
        {
            var medicamento = _service.RecuperaMedicamentoPorId(medicamentoId);
                if (medicamento is null)
                return NotFound();

            if (!pacientes.Any())
                return BadRequest();

            List<int> idsValidos = new();
            List<int> idsInvalidos = new();
            //adicionar validação para ver se já existe medicamento no paciente

            foreach (var paciente in pacientes)
            {
                var idPaciente = paciente.Id.Value;//value, pq é um int nullable

                var existe = await _pacienteService.PacienteExisteAsync(idPaciente);
                if (existe) 
                {
                    var possuiMedicamento = await _pacienteService.PossuiMedicamentoAsync(idPaciente, medicamentoId);
                    idsValidos.Add(idPaciente);
                }
                else
                    idsInvalidos.Add(idPaciente);
            }

            if (idsValidos.Any())
            {
                for (int i = 0; i < idsValidos.Count; i++)
                {
                    await _service.AdicionaPacienteAMedicamentoAsync(medicamentoId, idsValidos[i]);
                }
                return Ok();
            }
            //if (medicamento.PacienteMedicamentos.Any == idsValidos) 
            //{
              //  return BadRequest("Paciente já existe em medicamento");
            //}
            else
            {
                return NotFound(new { mensagem = "Estes pacientes não foram encontrados: ", idsInvalidos });
            }
        }
    }
}
