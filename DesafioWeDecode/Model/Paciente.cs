using System.ComponentModel.DataAnnotations;

namespace DesafioWeDecode.Model
{
    public class Paciente
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do Paciente é obrigatório.")]
        [MaxLength(30, ErrorMessage = "O nome não pode exceder 30 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O sexo do paciente é obrigatório.")]
        [MaxLength(30, ErrorMessage = "O sexo não pode exceder 30 caracteres.")]
        public string Sexo { get; set; }

        [Range(1, 120, ErrorMessage = "A Idade não pode exceder 120 anos.")]
        public int Idade { get; set; }


        public List<PacienteMedicamento> PacienteMedicamentos { get; set; } = new();
    }
}
