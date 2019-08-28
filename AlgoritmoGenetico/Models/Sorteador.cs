using System.Collections.Generic;

namespace Models
{
    public class Sorteador
    {
        public int Id { get; set; }
        public int Dia { get; set; }
        public int Horario { get; set; }
        public int Periodo { get; set; }

        public static List<Sorteador> GetSorteia()
        {
            List<Sorteador> sorts = new List<Sorteador>{
                new Sorteador()
                {

                }
            };
            return sorts;
        }
    }
}
