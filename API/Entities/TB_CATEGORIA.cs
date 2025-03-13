using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class TB_CATEGORIA
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }
        
        public virtual ICollection<TB_Exercicios> Exercicios { get; set; }
    }
}
