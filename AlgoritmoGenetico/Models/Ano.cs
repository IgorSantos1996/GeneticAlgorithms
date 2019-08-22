using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("tb_ano")]
    public class Ano
    {
        [Column("idano")]
        public int Id { get; set; }
        [Column("periodo")]
        public string Periodo { get; set; }
        [Column("tipo")]
        public string Tipo { get; set; }
    }
}
