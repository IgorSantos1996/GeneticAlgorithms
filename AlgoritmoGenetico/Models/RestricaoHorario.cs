using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tb_restricao_horario")]
    public class RestricaoHorario
    {
        [Column("idrestricaohorario")]
        public int Id { get; set; }
        [NotMapped]
        public Dia Segunda { get; set; }
        [NotMapped]
        public Dia Terca { get; set; }
        [NotMapped]
        public Dia Quarta { get; set; }
        [NotMapped]
        public Dia Quinta { get; set; }
        [NotMapped]
        public Dia Sexta { get; set; }
        [Column("idano")]
        public int IdAno { get; set; }
        [ForeignKey("IdAno")]
        public virtual Ano Ano { get; set; }

        public RestricaoHorario()
        {
            Segunda = new Dia();
            Terca = new Dia();
            Quarta = new Dia();
            Quinta = new Dia();
            Sexta = new Dia();
        }
    }
}
