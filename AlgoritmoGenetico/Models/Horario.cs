using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tb_horario")]
    public class Horario
    {
        [Column("idhorario")]
        public int Id { get; set; }
        [Column("iddisciplina")]
        public int IdDisciplina { get; set; }
        [Column("idano")]
        public int IdAno { get; set; }

        [ForeignKey("IdDisciplina")]
        public virtual Disciplina Disciplina { get; set; }
        [ForeignKey("IdAno")]
        public virtual Ano Ano { get; set; }
    }
}
