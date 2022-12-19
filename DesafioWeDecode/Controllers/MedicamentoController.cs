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
        public async Task<IActionResult> AdicionaMedicamento([FromBody] CreateMedicamentoDTO medicamentoDto) //ADICIONA 
        {
            var foiAdicionado = await _service.AdicionaMedicamentoAsync(medicamentoDto);
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

        //[HttpGet("{id}")]//FUNCIONA
        //public IActionResult RecuperaMedicacaoPorId(int id) //READ POR ID
        //{
        //    var medicamento = _context.Medicamentos
        //        .FirstOrDefault(m => m.Id == id);
        //    if (medicamento == null) return NotFound();
        //    return Ok(medicamento);
        //}

        //[HttpPut("{id}")]//FUNCIONA
        //public IActionResult AtualizaFilme(int id, [FromBody] UpdateMedicamentoDTO medicamentoDto)
        //{
        //    var medicamento = _context.Medicamentos.FirstOrDefault(m => m.Id == id);
        //    if (medicamento == null) return NotFound();
        //    _mapper.Map(medicamentoDto, medicamento);
        //    _context.SaveChanges();
        //    return NoContent();
        //}

    }
}
