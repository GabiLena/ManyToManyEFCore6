using DesafioWeDecode.Model;

namespace DesafioWeDecode.Data.MedicamentoDTO
{
    public class ReadMedicamentoDTO
    {
        public int? Id { get; set; }
        public string Tipo { get; set; } //liquido, comprimido
        public string Indicacao { get; set; }// dor, febre
        public string Nome { get; set; }
        public int Mg { get; set; }
        public List<string> Pacientes { get; set; } = new();
    }
}
