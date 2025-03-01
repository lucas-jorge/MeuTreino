using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class TB_Exercicios
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O exercício deve ser informado!")]
        public string Exercicio { get; set; }

        public int Serie { get; set; }

        public int Repeticoes { get; set; }
        
        public int Tempo { get; set; }
    }
}
