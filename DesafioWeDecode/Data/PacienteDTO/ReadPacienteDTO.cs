using DesafioWeDecode.Model;
using System.ComponentModel.DataAnnotations;

namespace DesafioWeDecode.Data.PacienteDTO
{
    public class ReadPacienteDTO
    {
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public int Idade { get; set; }
        public List<string> Medicamentos { get; set; } = new();
    }
}

