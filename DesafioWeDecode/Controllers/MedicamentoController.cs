using AutoMapper;
using DesafioWeDecode.Data;
using DesafioWeDecode.Data.MedicamentoDTO;
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

        public MedicamentoController(IMedicamentoService service)
        {
            _service = service;
        }

        [HttpPost]//FUNCIONA
        public async Task<IActionResult> AdicionaMedicamento([FromBody] MedicamentoDTO medicamentoDto) //ADICIONA 
        {
            var foiAdicionado = await _service.AdicionaMedicamentoAsync(medicamentoDto);//pq esse é async e o debaixo não?
            if (foiAdicionado)
                return Ok("Medicamento adicionado com sucesso");

            return StatusCode(304);
        }

        [HttpGet]//FUNCIONA
        public ActionResult<IEnumerable<Medicamento>> RecuperaMedicamentos([FromQuery] int skip = 0, [FromQuery] int take = 10)// READ TODOS 
        {
            var medicamentos = _service.SelecionaMedicamentos(skip, take);
            if (medicamentos is null)
                return StatusCode(500, "Lista é nula");

            if (!medicamentos.Any())
                return NotFound();

            return Ok(medicamentos);
        }

        [HttpGet("{id}")]//FUNCIONA
        public IActionResult RecuperaMedicacaoPorId(int id) //READ POR ID
        {
            var medicamento = _service.RecuperaMedicamentoPorId(id);
            if (medicamento == null) return NotFound();
            return Ok(medicamento);
        }

        [HttpPut("{id}")]//FUNCIONA
        public async Task<IActionResult> AtualizaFilme(int id, [FromBody] MedicamentoDTO medicamentoDto)
        {
            var medicamentoFoiAtualizado = await _service.AtualizaMedicamentoAsync(id, medicamentoDto);
            if (!medicamentoFoiAtualizado)
                return NotFound();

            return Ok();
        }

    }
}
