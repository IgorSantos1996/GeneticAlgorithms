namespace Models
{
    public class Periodo
    {
        public Dia Segunda { get; set; }
        public Dia Terca { get; set; }
        public Dia Quarta { get; set; }
        public Dia Quinta { get; set; }
        public Dia Sexta { get; set; }

        public Periodo()
        {
            Segunda = new Dia();
            Terca = new Dia();
            Quarta = new Dia();
            Quinta = new Dia();
            Sexta = new Dia();
        }
    }
}
