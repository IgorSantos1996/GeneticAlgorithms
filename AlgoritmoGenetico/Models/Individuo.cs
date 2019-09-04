namespace Models
{
    public class Individuo
    {
        public Periodo Periodo_1_2 { get; set; }
        public Periodo Periodo_3_4 { get; set; }
        public Periodo Periodo_5_6 { get; set; }
        public Periodo Periodo_7_8 { get; set; }
        public int Aptidao { get; set; }

        public Individuo()
        {
            Periodo_1_2 = new Periodo();
            Periodo_3_4 = new Periodo();
            Periodo_5_6 = new Periodo();
            Periodo_7_8 = new Periodo();
            Aptidao = 100;
        }
    }
}
