using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tb_restricao_horario")]
    public class RestricaoHorario
    {


        [Column("idrestricaohorario")]
        public int Id { get; set; }
        [Column("segunda1")]
        public bool SegundaHorario1 { get; set; }
        [Column("segunda2")]
        public bool SegundaHorario2 { get; set; }
        [Column("segunda3")]
        public bool SegundaHorario3 { get; set; }
        [Column("terca1")]
        public bool TercaHorario1 { get; set; }
        [Column("terca2")]
        public bool TercaHorario2 { get; set; }
        [Column("terca3")]
        public bool TercaHorario3 { get; set; }
        [Column("quarta1")]
        public bool QuartaHorario1 { get; set; }
        [Column("quarta2")]
        public bool QuartaHorario2 { get; set; }
        [Column("quarta3")]
        public bool QuartaHorario3 { get; set; }
        [Column("quinta1")]
        public bool QuintaHorario1 { get; set; }
        [Column("quinta2")]
        public bool QuintaHorario2 { get; set; }
        [Column("quinta3")]
        public bool QuintaHorario3 { get; set; }
        [Column("sexta1")]
        public bool SextaHorario1 { get; set; }
        [Column("sexta2")]
        public bool SextaHorario2 { get; set; }
        [Column("sexta3")]
        public bool SextaHorario3 { get; set; }
        [Column("periodo")]
        public int Periodo { get; set; }
        [Column("idano")]
        public int IdAno { get; set; }
        [ForeignKey("IdAno")]
        public virtual Ano Ano { get; set; }
    }
}
