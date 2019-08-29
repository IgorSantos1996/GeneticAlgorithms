using System.Collections.Generic;

namespace Models
{
    public class Sorteador
    {
        public int Id { get; set; }
        public int Dia { get; set; }
        public int Horario { get; set; }
        public int Periodo { get; set; }

        public static List<Sorteador> GetSorteia(int periodo1_2, int periodo3_4, int periodo5_6, int periodo7_8)
        {
            int contadorDia = 1;
            int contadorHorario = 0;
            List<Sorteador> sorts = new List<Sorteador> { };
            for (int i = 1; i < 61; i++)
            {
                if (contadorDia == 6) // se colocasse 5, repetia horarios
                    contadorDia = 1;
                contadorHorario++;
                Sorteador s = new Sorteador() { };
                s.Id = i;
                s.Dia = contadorDia;
                s.Horario = contadorHorario;
                if (i < 16)
                    s.Periodo = periodo1_2;
                if (i > 15 && i < 31)
                    s.Periodo = periodo3_4;
                if (i > 30 && i < 46)
                    s.Periodo = periodo5_6;
                if (i > 45 && i < 61)
                    s.Periodo = periodo7_8;
                if (contadorHorario == 3)
                {
                    contadorHorario = 0;
                    contadorDia++;
                }
                sorts.Add(s);
            }
            return sorts;
        }
    }
}
