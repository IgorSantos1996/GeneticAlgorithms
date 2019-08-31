namespace Models
{
    public class Dia
    {
        public Disciplina Disciplina_1Horario { get; set; }
        public Disciplina Disciplina_2Horario { get; set; }
        public Disciplina Disciplina_3Horario { get; set; }

        public Dia()
        {
            Disciplina_1Horario = null;
            Disciplina_2Horario = null;
            Disciplina_3Horario = null;
        }
    }
}
