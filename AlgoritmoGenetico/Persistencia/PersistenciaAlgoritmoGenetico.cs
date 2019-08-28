using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistencia
{
    public class PersistenciaAlgoritmoGenetico
    {
        private Random random = new Random();
        private readonly int qtdIndividuos = 1;
        private readonly DbContextAG _contexto;
        private Individuo individuo;
        public PersistenciaAlgoritmoGenetico(DbContextAG contexto)
        {
            _contexto = contexto;
        }

        public List<Individuo> InicializarPopulação(string ano)
        {
            List<Individuo> arrayIndividuo = new List<Individuo>();

            var horariosBanco = getHorarios(ano);

            for (int i = 0; i < qtdIndividuos; i++)
            {
                individuo = CriarIndividuo(horariosBanco, ano);

                arrayIndividuo.Add(individuo);
            }
            return arrayIndividuo;
        }

        private Individuo CriarIndividuo(List<Horario> horarios, string ano)
        {
            Individuo individuo = new Individuo();

            Random random = new Random();

            while (horarios.Count > 0)
            {
                var posicaoAleatoriaArray = random.Next(0, horarios.Count); // get posicao do array horarios

                var idDisciplina = horarios.ElementAt(posicaoAleatoriaArray).IdDisciplina;
                var disciplina = getDisciplina(idDisciplina);

                var periodo = disciplina.Periodo;

                // rodar de acordo com a quantidade de horas por disciplina
                int diasDeAula = 2;
                if (disciplina.Horas == 6)
                    diasDeAula = 3;

                // laço para colocar as disciplinas em mais de 1 horario
                for (int i = 0; i < diasDeAula; i++)
                {
                    var diaSort = GetSortDiaOrHorario("dia");
                    var horarioDisciplinaSort = GetSortDiaOrHorario("horario");

                    if (periodo == 1 || periodo == 2)
                    {
                        while
                            (ExisteDisciplinaNoIndividuo(individuo.Periodo_1_2, diaSort, horarioDisciplinaSort)
                            || ExisteReservaNoDiaEHorarioDisciplina(ano, periodo, diaSort, horarioDisciplinaSort))
                        {
                            diaSort = GetSortDiaOrHorario("dia");
                            horarioDisciplinaSort = GetSortDiaOrHorario("horario");
                        }
                        individuo = AtribuirDisciplinaAoIndividuo(individuo, disciplina, diaSort, horarioDisciplinaSort);
                    }
                    if (periodo == 3 || periodo == 4)
                    {
                        while
                            (ExisteDisciplinaNoIndividuo(individuo.Periodo_3_4, diaSort, horarioDisciplinaSort)
                            || ExisteReservaNoDiaEHorarioDisciplina(ano, periodo, diaSort, horarioDisciplinaSort))
                        {
                            diaSort = GetSortDiaOrHorario("dia");
                            horarioDisciplinaSort = GetSortDiaOrHorario("horario");
                        }
                        individuo = AtribuirDisciplinaAoIndividuo(individuo, disciplina, diaSort, horarioDisciplinaSort);
                    }
                    if (periodo == 5 || periodo == 6)
                    {
                        while
                            (ExisteDisciplinaNoIndividuo(individuo.Periodo_5_6, diaSort, horarioDisciplinaSort)
                            || ExisteReservaNoDiaEHorarioDisciplina(ano, periodo, diaSort, horarioDisciplinaSort))
                        {
                            diaSort = GetSortDiaOrHorario("dia");
                            horarioDisciplinaSort = GetSortDiaOrHorario("horario");
                        }
                        individuo = AtribuirDisciplinaAoIndividuo(individuo, disciplina, diaSort, horarioDisciplinaSort);
                    }
                    if (periodo == 7 || periodo == 8)
                    {
                        while
                            (ExisteDisciplinaNoIndividuo(individuo.Periodo_7_8, diaSort, horarioDisciplinaSort)
                            || ExisteReservaNoDiaEHorarioDisciplina(ano, periodo, diaSort, horarioDisciplinaSort))
                        {
                            diaSort = GetSortDiaOrHorario("dia");
                            horarioDisciplinaSort = GetSortDiaOrHorario("horario");
                        }
                        individuo = AtribuirDisciplinaAoIndividuo(individuo, disciplina, diaSort, horarioDisciplinaSort);
                    }
                }

                horarios.RemoveAt(posicaoAleatoriaArray);
            }
            return individuo;
        }

        // Metodo criado para verificar a existência de uma disciplina já cadastrada no indiviuo em determinado dia e horario
        private bool ExisteDisciplinaNoIndividuo(Periodo periodo, int dia, int horarioDisciplina)
        {
            switch (dia)
            {
                case 1: // segunda
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Segunda.Disciplina_1Horario != -1) // já existe disciplina cadastrada
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Segunda.Disciplina_2Horario != -1)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Segunda.Disciplina_3Horario != -1)
                            return true;
                    break;
                case 2: // terça
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Terca.Disciplina_1Horario != -1)
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Terca.Disciplina_2Horario != -1)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Terca.Disciplina_3Horario != -1)
                            return true;
                    break;
                case 3: // quarta
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Quarta.Disciplina_1Horario != -1)
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Quarta.Disciplina_2Horario != -1)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Quarta.Disciplina_3Horario != -1)
                            return true;
                    break;
                case 4: // quinta
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Quinta.Disciplina_1Horario != -1)
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Quinta.Disciplina_2Horario != -1)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Quinta.Disciplina_3Horario != -1)
                            return true;
                    break;
                case 5: // sexta
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Sexta.Disciplina_1Horario != -1)
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Sexta.Disciplina_2Horario != -1)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Sexta.Disciplina_3Horario != -1)
                            return true;
                    break;
            }
            return false;
        }

        // para os departamentos que já tem as disciplinas fixas
        private bool ExisteReservaNoDiaEHorarioDisciplina(string ano, int periodoDisciplina, int dia, int horarioDisciplina)
        {
            var restricaoHorario = GetRestricaoHorario(ano, periodoDisciplina);

            if (restricaoHorario == null)
                return false;

            switch (dia)
            {
                case 1: // segunda
                    if (horarioDisciplina == 1) // 1º horário
                        return restricaoHorario.SegundaHorario1;
                    if (horarioDisciplina == 2)
                        return restricaoHorario.SegundaHorario2;
                    if (horarioDisciplina == 3)
                        return restricaoHorario.SegundaHorario3;
                    break;
                case 2:
                    if (horarioDisciplina == 1)
                        return restricaoHorario.TercaHorario1;
                    if (horarioDisciplina == 2)
                        return restricaoHorario.TercaHorario2;
                    if (horarioDisciplina == 3)
                        return restricaoHorario.TercaHorario3;
                    break;
                case 3:
                    if (horarioDisciplina == 1)
                        return restricaoHorario.QuartaHorario1;
                    if (horarioDisciplina == 2)
                        return restricaoHorario.QuartaHorario2;
                    if (horarioDisciplina == 3)
                        return restricaoHorario.QuartaHorario3;
                    break;
                case 4:
                    if (horarioDisciplina == 1)
                        return restricaoHorario.QuintaHorario1;
                    if (horarioDisciplina == 2)
                        return restricaoHorario.QuintaHorario2;
                    if (horarioDisciplina == 3)
                        return restricaoHorario.QuintaHorario3;
                    break;
                case 5:
                    if (horarioDisciplina == 1)
                        return restricaoHorario.SextaHorario1;
                    if (horarioDisciplina == 2)
                        return restricaoHorario.SextaHorario1;
                    if (horarioDisciplina == 3)
                        return restricaoHorario.SextaHorario3;
                    break;
            }
            return false;
        }

        private int GetSortDiaOrHorario(string tipoSort)
        {
            if (tipoSort.Equals("horario"))
                return random.Next(1, 3);
            if (tipoSort.Equals("dia"))
                return random.Next(1, 5);
            return -1;
        }
        private Disciplina getDisciplina(int idDisciplina)
            => _contexto
                .Disciplinas
                .Include(d => d.Professor)
                .SingleOrDefault(d => d.Id == idDisciplina);
        private List<Horario> getHorarios(string ano)
            => _contexto
                .Horarios
                .Where(h => h.Ano.Periodo.Equals(ano))
                .ToList();
        private bool ExisteRestricaoHorario(string ano, int periodoDisciplina)
            => _contexto
            .RestricaoHorarios
            .Any(rh => rh.Ano.Periodo.Equals(ano) && rh.Periodo == periodoDisciplina);
        private RestricaoHorario GetRestricaoHorario(string ano, int periodoDisciplina)
            => _contexto
                    .RestricaoHorarios
                    .SingleOrDefault(rh => rh.Ano.Periodo.Equals(ano) && rh.Periodo == periodoDisciplina);
        private Individuo AtribuirDisciplinaAoIndividuo(Individuo individuo, Disciplina disciplina, int dia, int horarioDisciplina)
        {
            switch (dia)
            {
                case 1: // segunda
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Segunda.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Segunda.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Segunda.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Segunda.Disciplina_1Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Segunda.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Segunda.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Segunda.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Segunda.Disciplina_2Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Segunda.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Segunda.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Segunda.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Segunda.Disciplina_3Horario = disciplina.Id;
                    }
                    break;
                case 2: // terça
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Terca.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Terca.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Terca.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Terca.Disciplina_1Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Terca.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Terca.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Terca.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Terca.Disciplina_2Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Terca.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Terca.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Terca.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Terca.Disciplina_3Horario = disciplina.Id;
                    }
                    break;
                case 3: // quarta
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quarta.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quarta.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quarta.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quarta.Disciplina_1Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quarta.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quarta.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quarta.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quarta.Disciplina_2Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quarta.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quarta.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quarta.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quarta.Disciplina_3Horario = disciplina.Id;
                    }
                    break;
                case 4: // quinta
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quinta.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quinta.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quinta.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quinta.Disciplina_1Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quinta.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quinta.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quinta.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quinta.Disciplina_2Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quinta.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quinta.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quinta.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quinta.Disciplina_3Horario = disciplina.Id;
                    }
                    break;
                case 5: // sexta
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Sexta.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Sexta.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Sexta.Disciplina_1Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Sexta.Disciplina_1Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Sexta.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Sexta.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Sexta.Disciplina_2Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Sexta.Disciplina_2Horario = disciplina.Id;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Sexta.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Sexta.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Sexta.Disciplina_3Horario = disciplina.Id;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Sexta.Disciplina_3Horario = disciplina.Id;
                    }
                    break;
            }
            return individuo;
        }
    }
}
