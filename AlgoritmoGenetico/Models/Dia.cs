namespace Models
{
    public class Dia
    {
        public int Disciplina_1Horario { get; set; }
        public int Disciplina_2Horario { get; set; }
        public int Disciplina_3Horario { get; set; }

        public Dia()
        {
            Disciplina_1Horario = -1;
            Disciplina_2Horario = -1;
            Disciplina_3Horario = -1;
        }
    }
}
