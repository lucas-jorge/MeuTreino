using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class TB_Exercicios
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Exercicio { get; set; }
        
        [Required]
        public int Serie { get; set; }
        
        [Required]
        public int Repeticoes { get; set; }
        
        public int Tempo { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [ForeignKey("UsuarioId")]
        public virtual TB_USUARIO Usuario { get; set; }
        
        public int? CategoriaId { get; set; }
        
        [ForeignKey("CategoriaId")]
        public virtual TB_CATEGORIA Categoria { get; set; }
        
        public System.DateTime DataCriacao { get; set; } = System.DateTime.Now;
    }
}
