using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public partial class TB_USUARIO
    {
        [Key]
        [DisplayName("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Display(Name = "Nome")]
        [MaxLength(255, ErrorMessage = "Tamanho máximo de 255 caracteres")]
        [Required(ErrorMessage = "O nome deve ser informado!")]
        public string Nome { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "O status deve ser informado!")]
        public EStatus Status { get; set; } = EStatus.Ativo;

        [Display(Name = "Senha")]
        [MaxLength(255, ErrorMessage = "Tamanho máximo de 255 caracteres")]
        [Required(ErrorMessage = "A senha deve ser informada!")]
        public string Senha { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.Now;

        public enum EStatus
        {
            Inativo = 0,
            Ativo = 1,
            [Display(Name = "Excluído")]
            Excluido = 2
        }

        public List<TB_Exercicios> Exercicios { get; set; }
    }
}