using System.ComponentModel.DataAnnotations;

namespace DesafioWeDecode.Data.MedicamentoDTO
{
    public class MedicamentoDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "O tipo Do Medicamento é obrigatório.")]
        [MaxLength(30, ErrorMessage = "O tipo de Medicamento não pode exceder 30 caracteres.")]
        public string Tipo { get; set; } //liquido, comprimido

        [Required(ErrorMessage = "A Indicação é obrigatória.")]
        [MaxLength(25, ErrorMessage = "A Indicação não pode exceder 25 caracteres.")]
        public string Indicacao { get; set; }// dor, febre

        [Required(ErrorMessage = "O Nome do medicamento é obrigatório.")]
        [MaxLength(20, ErrorMessage = "O Nome do Medicamento não pode exceder 30 caracteres.")]
        public string Nome { get; set; }

        [Range(1, 90, ErrorMessage = "O Mg deve ser até 90.")]
        public int Mg { get; set; }
    }
}
