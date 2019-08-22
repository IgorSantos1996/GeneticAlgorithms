using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tb_professor")]
    public class Professor
    { 
        [Column("idprofessor")]
        public int Id { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
    }
}
