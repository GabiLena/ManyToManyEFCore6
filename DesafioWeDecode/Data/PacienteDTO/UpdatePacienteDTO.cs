using System.ComponentModel.DataAnnotations;

namespace DesafioWeDecode.Data.PacienteDTO
{
    public class UpdatePacienteDTO
    {
        [MaxLength(30, ErrorMessage = "O nome não pode exceder 30 caracteres.")]
        public string Nome { get; set; }

        [MaxLength(30, ErrorMessage = "O sexo não pode exceder 30 caracteres.")]
        public string Sexo { get; set; }

        [Range(1, 120, ErrorMessage = "a Idade não pode exceder 120 anos.")]
        public int Idade { get; set; }
    }
}
