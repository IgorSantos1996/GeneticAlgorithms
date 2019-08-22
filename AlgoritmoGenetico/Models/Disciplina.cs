using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tb_disciplina")]
    public class Disciplina
    {
        [Column("iddisciplina")]
        public int Id { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
        [Column("periodo")]
        public int Periodo { get; set; }
        [Column("idprofessor")]
        public int IdProfessor { get; set; }
        [ForeignKey("IdProfessor")]
        public virtual Professor Professor{ get; set; }
    }
}
