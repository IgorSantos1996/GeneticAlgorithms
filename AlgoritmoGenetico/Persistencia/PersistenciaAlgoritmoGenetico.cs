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
        private readonly int qtdIndividuos = 4;
        private readonly DbContextAG _contexto;
        private Individuo individuo;
        public PersistenciaAlgoritmoGenetico(DbContextAG contexto)
        {
            _contexto = contexto;
        }

        public List<Individuo> AlgoritmoGenetico(string ano)
        {
            var arrayIndividuo = InicializarPopulação(ano);
            // array de individuos avaliados
            arrayIndividuo = FuncaoFitness(arrayIndividuo, ano);
            // escolher mais aptos para cross-over

            // cross-over e/ou mutação

            // concepção da nova geração

            return arrayIndividuo;
        }

        private List<Individuo> InicializarPopulação(string ano)
        {
            List<Individuo> arrayIndividuo = new List<Individuo>();

            for (int i = 0; i < qtdIndividuos; i++)
            {
                var horariosBanco = getHorarios(ano);

                individuo = CriarIndividuo(horariosBanco, ano);

                arrayIndividuo.Add(individuo);
            }
            return arrayIndividuo;
        }
        private Individuo CriarIndividuo(List<Horario> horarios, string ano)
        {
            // variaveis para auxiliar o sorteador
            int periodo1_2 = 0;
            int periodo3_4 = 0;
            int periodo5_6 = 0;
            int periodo7_8 = 0;
            var periodos = GetPeriodos(ano);
            foreach (var item in periodos)
            {
                if (item == 1 || item == 2)
                    periodo1_2 = item;
                if (item == 3 || item == 4)
                    periodo3_4 = item;
                if (item == 5 || item == 6)
                    periodo5_6 = item;
                if (item == 7 || item == 8)
                    periodo7_8 = item;
            }
            // fim do auxilio sorteador

            Individuo individuo = new Individuo();

            Random random = new Random();

            var arraySorteia = Sorteador.GetSorteia(periodo1_2, periodo3_4, periodo5_6, periodo7_8);

            arraySorteia = RemoveHorarioComReserva(arraySorteia, ano);

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
                    int diaSort = 0;
                    int horarioDisciplinaSort = 0;

                    if (periodo == 1 || periodo == 2)
                    {
                        (diaSort, horarioDisciplinaSort, arraySorteia) = GetSortDiaOrHorario(arraySorteia, periodo);

                        while
                            (ExisteDisciplinaNoIndividuo(individuo.Periodo_1_2, diaSort, horarioDisciplinaSort))
                        {
                            (diaSort, horarioDisciplinaSort, arraySorteia) = GetSortDiaOrHorario(arraySorteia, periodo);
                        }
                        individuo = AtribuirDisciplinaAoIndividuo(individuo, disciplina, diaSort, horarioDisciplinaSort);
                    }
                    if (periodo == 3 || periodo == 4)
                    {
                        (diaSort, horarioDisciplinaSort, arraySorteia) = GetSortDiaOrHorario(arraySorteia, periodo);
                        while
                            (ExisteDisciplinaNoIndividuo(individuo.Periodo_3_4, diaSort, horarioDisciplinaSort))
                        {
                            (diaSort, horarioDisciplinaSort, arraySorteia) = GetSortDiaOrHorario(arraySorteia, periodo);
                        }
                        individuo = AtribuirDisciplinaAoIndividuo(individuo, disciplina, diaSort, horarioDisciplinaSort);
                    }
                    if (periodo == 5 || periodo == 6)
                    {
                        (diaSort, horarioDisciplinaSort, arraySorteia) = GetSortDiaOrHorario(arraySorteia, periodo);
                        while
                            (ExisteDisciplinaNoIndividuo(individuo.Periodo_5_6, diaSort, horarioDisciplinaSort))
                        {
                            (diaSort, horarioDisciplinaSort, arraySorteia) = GetSortDiaOrHorario(arraySorteia, periodo);
                        }
                        individuo = AtribuirDisciplinaAoIndividuo(individuo, disciplina, diaSort, horarioDisciplinaSort);
                    }
                    if (periodo == 7 || periodo == 8)
                    {
                        (diaSort, horarioDisciplinaSort, arraySorteia) = GetSortDiaOrHorario(arraySorteia, periodo);
                        while
                            (ExisteDisciplinaNoIndividuo(individuo.Periodo_7_8, diaSort, horarioDisciplinaSort))
                        {
                            (diaSort, horarioDisciplinaSort, arraySorteia) = GetSortDiaOrHorario(arraySorteia, periodo);
                        }
                        individuo = AtribuirDisciplinaAoIndividuo(individuo, disciplina, diaSort, horarioDisciplinaSort);
                    }
                }

                horarios.RemoveAt(posicaoAleatoriaArray);
            }
            return individuo;
        }
        // avaliar os individuos
        public List<Individuo> FuncaoFitness(List<Individuo> individuos, string ano)
        {
            List<Individuo> arrayIndividuos = new List<Individuo>();

            foreach (var item in individuos)
            {
                var individuo = item;
                individuo = Restricao1(individuo);
                individuo = Restricao2(individuo, ano);
                individuo = Restricao3(individuo);
                individuo = RestricaoDosea(individuo);

                arrayIndividuos.Add(individuo);
            }
            return arrayIndividuos;
        }
        private (Individuo, Individuo) Selecao(List<Individuo> populacao)
        {
            var totalAptidaoPopulacao = populacao.Sum(p => p.Aptidao);
            Random random = new Random();

            Individuo individuo1 = null;
            Individuo individuo2 = null;

            int cont = 1; // saber qual individuo estou

            while (cont <= 2)
            {
                var numeroAleatorio = random.Next(0, totalAptidaoPopulacao);
                var somaAptidoes = 0;
                foreach (var item in populacao)
                {
                    somaAptidoes += item.Aptidao;
                    if (somaAptidoes >= numeroAleatorio)
                    {
                        if (cont == 1)
                        {
                            individuo1 = item;
                            cont++;
                        }
                        if(cont == 2)
                        {
                            individuo2 = item;
                            cont++;
                        }
                    }
                }
            }
            return (individuo1, individuo2);
        }
        private List<Individuo> GerarNovaPopulacao(List<Individuo> populacao)
        {
            // chamar o selecao para populacao
            // excluir os individuos ja selecionados
            // pegar os 2 individuos
            // passar para o crossover
            return null;
        }
        private (Individuo, Individuo) CrossOver(Individuo individuo1, Individuo individuo2)
        {
            // cruzamento segunda e terça
            // cruzamento quarta, quinta e sexta de outro
            // gerando assim dois individuos
            return (null, null);
        }

        /* Restricao Disciplina de 6 créditos com todas as aulas no mesmo dia */
        private Individuo Restricao1(Individuo individuo)
        {
            int penalizacao = 5;
            int aptidao = 100;
            var disciplinas6Creditos = GetDisciplinasNCreditos(6);

            // 1 ou 2 periodo
            if (individuo.Periodo_1_2.Segunda.Disciplina_1Horario != null) // p1 - 90h ou 6 creditos
            {
                if (individuo.Periodo_1_2.Segunda.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Segunda.Disciplina_2Horario)
                    && individuo.Periodo_1_2.Segunda.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Segunda.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_1_2.Segunda.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_1_2.Terca.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_1_2.Terca.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Terca.Disciplina_2Horario)
                    && individuo.Periodo_1_2.Terca.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Terca.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_1_2.Terca.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_1_2.Quarta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_1_2.Quarta.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Quarta.Disciplina_2Horario)
                    && individuo.Periodo_1_2.Quarta.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Quarta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_1_2.Quarta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_1_2.Quinta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_1_2.Quinta.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Quinta.Disciplina_2Horario)
                    && individuo.Periodo_1_2.Quinta.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Quinta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_1_2.Quinta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_1_2.Sexta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_1_2.Sexta.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Sexta.Disciplina_2Horario)
                    && individuo.Periodo_1_2.Sexta.Disciplina_1Horario.Equals(individuo.Periodo_1_2.Sexta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_1_2.Sexta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }

            // 3 ou 4 periodo
            if (individuo.Periodo_3_4.Segunda.Disciplina_1Horario != null) // p1 - 90h ou 6 creditos
            {
                if (individuo.Periodo_3_4.Segunda.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Segunda.Disciplina_2Horario)
                    && individuo.Periodo_3_4.Segunda.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Segunda.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_3_4.Segunda.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_3_4.Terca.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_3_4.Terca.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Terca.Disciplina_2Horario)
                    && individuo.Periodo_3_4.Terca.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Terca.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_3_4.Terca.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_3_4.Quarta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_3_4.Quarta.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Quarta.Disciplina_2Horario)
                    && individuo.Periodo_3_4.Quarta.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Quarta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_3_4.Quarta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_3_4.Quinta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_3_4.Quinta.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Quinta.Disciplina_2Horario)
                    && individuo.Periodo_3_4.Quinta.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Quinta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_3_4.Quinta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_3_4.Sexta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_3_4.Sexta.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Sexta.Disciplina_2Horario)
                    && individuo.Periodo_3_4.Sexta.Disciplina_1Horario.Equals(individuo.Periodo_3_4.Sexta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_3_4.Sexta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }

            // 5 ou 6 periodo
            if (individuo.Periodo_5_6.Segunda.Disciplina_1Horario != null) // p1 - 90h ou 6 creditos
            {
                if (individuo.Periodo_5_6.Segunda.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Segunda.Disciplina_2Horario)
                    && individuo.Periodo_5_6.Segunda.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Segunda.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_5_6.Segunda.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_5_6.Terca.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_5_6.Terca.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Terca.Disciplina_2Horario)
                    && individuo.Periodo_5_6.Terca.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Terca.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_5_6.Terca.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_5_6.Quarta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_5_6.Quarta.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Quarta.Disciplina_2Horario)
                    && individuo.Periodo_5_6.Quarta.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Quarta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_5_6.Quarta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_5_6.Quinta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_5_6.Quinta.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Quinta.Disciplina_2Horario)
                    && individuo.Periodo_5_6.Quinta.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Quinta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_5_6.Quinta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_5_6.Sexta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_5_6.Sexta.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Sexta.Disciplina_2Horario)
                    && individuo.Periodo_5_6.Sexta.Disciplina_1Horario.Equals(individuo.Periodo_5_6.Sexta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_5_6.Sexta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }

            // 7 ou 8 periodo
            if (individuo.Periodo_7_8.Segunda.Disciplina_1Horario != null) // p1 - 90h ou 6 creditos
            {
                if (individuo.Periodo_7_8.Segunda.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Segunda.Disciplina_2Horario)
                    && individuo.Periodo_7_8.Segunda.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Segunda.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_7_8.Segunda.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_7_8.Terca.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_7_8.Terca.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Terca.Disciplina_2Horario)
                    && individuo.Periodo_7_8.Terca.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Terca.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_7_8.Terca.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_7_8.Quarta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_7_8.Quarta.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Quarta.Disciplina_2Horario)
                    && individuo.Periodo_7_8.Quarta.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Quarta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_7_8.Quarta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_7_8.Quinta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_7_8.Quinta.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Quinta.Disciplina_2Horario)
                    && individuo.Periodo_7_8.Quinta.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Quinta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_7_8.Quinta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            if (individuo.Periodo_7_8.Sexta.Disciplina_1Horario != null)
            {
                if (individuo.Periodo_7_8.Sexta.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Sexta.Disciplina_2Horario)
                    && individuo.Periodo_7_8.Sexta.Disciplina_1Horario.Equals(individuo.Periodo_7_8.Sexta.Disciplina_3Horario)
                    && disciplinas6Creditos.Contains(individuo.Periodo_7_8.Sexta.Disciplina_1Horario))
                {
                    aptidao -= penalizacao;
                }
            }
            return individuo;
        }
        /*Restrição para disponibilidade dos professores, ex: André Vinicius, não pode ir a UFS na sexta*/
        private Individuo Restricao2(Individuo individuo, string ano)
        {
            var disponibilidades = GetDisponibilidadeProfessores(ano);
            int penalizacao = 5;

            if (disponibilidades.Count > 0)
            {
                foreach (var item in disponibilidades)
                {
                    if (!item.Segunda)
                    {
                        var professor = item.Professor.Nome;
                        // 1 e 2 periodo
                        if (individuo.Periodo_1_2.Segunda.Disciplina_1Horario != null
                            && individuo.Periodo_1_2.Segunda.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Segunda.Disciplina_2Horario != null
                            && individuo.Periodo_1_2.Segunda.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Segunda.Disciplina_3Horario != null
                            && individuo.Periodo_1_2.Segunda.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 3 e 4 periodo
                        if (individuo.Periodo_3_4.Segunda.Disciplina_1Horario != null
                            && individuo.Periodo_3_4.Segunda.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Segunda.Disciplina_2Horario != null
                            && individuo.Periodo_3_4.Segunda.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Segunda.Disciplina_3Horario != null
                            && individuo.Periodo_3_4.Segunda.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 5 e 6 periodo
                        if (individuo.Periodo_5_6.Segunda.Disciplina_1Horario != null
                            && individuo.Periodo_5_6.Segunda.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Segunda.Disciplina_2Horario != null
                            && individuo.Periodo_5_6.Segunda.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Segunda.Disciplina_3Horario != null
                            && individuo.Periodo_5_6.Segunda.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 7 e 8 periodo
                        if (individuo.Periodo_7_8.Segunda.Disciplina_1Horario != null
                            && individuo.Periodo_7_8.Segunda.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Segunda.Disciplina_2Horario != null
                            && individuo.Periodo_7_8.Segunda.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Segunda.Disciplina_3Horario != null
                            && individuo.Periodo_7_8.Segunda.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                    }
                    if (!item.Terca)
                    {
                        var professor = item.Professor.Nome;
                        // 1 e 2 periodo
                        if (individuo.Periodo_1_2.Terca.Disciplina_1Horario != null
                            && individuo.Periodo_1_2.Terca.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Terca.Disciplina_2Horario != null
                            && individuo.Periodo_1_2.Terca.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Terca.Disciplina_3Horario != null
                            && individuo.Periodo_1_2.Terca.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 3 e 4 periodo
                        if (individuo.Periodo_3_4.Terca.Disciplina_1Horario != null
                            && individuo.Periodo_3_4.Terca.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Terca.Disciplina_2Horario != null
                            && individuo.Periodo_3_4.Terca.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Terca.Disciplina_3Horario != null
                            && individuo.Periodo_3_4.Terca.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 5 e 6 periodo
                        if (individuo.Periodo_5_6.Terca.Disciplina_1Horario != null
                            && individuo.Periodo_5_6.Terca.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Terca.Disciplina_2Horario != null
                            && individuo.Periodo_5_6.Terca.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Terca.Disciplina_3Horario != null
                            && individuo.Periodo_5_6.Terca.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 7 e 8 periodo
                        if (individuo.Periodo_7_8.Terca.Disciplina_1Horario != null
                            && individuo.Periodo_7_8.Terca.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Terca.Disciplina_2Horario != null
                            && individuo.Periodo_7_8.Terca.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Terca.Disciplina_3Horario != null
                            && individuo.Periodo_7_8.Terca.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                    }
                    if (!item.Quarta)
                    {
                        var professor = item.Professor.Nome;
                        // 1 e 2 periodo
                        if (individuo.Periodo_1_2.Quarta.Disciplina_1Horario != null
                            && individuo.Periodo_1_2.Quarta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Quarta.Disciplina_2Horario != null
                            && individuo.Periodo_1_2.Quarta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Quarta.Disciplina_3Horario != null
                            && individuo.Periodo_1_2.Quarta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 3 e 4 periodo
                        if (individuo.Periodo_3_4.Quarta.Disciplina_1Horario != null
                            && individuo.Periodo_3_4.Quarta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Quarta.Disciplina_2Horario != null
                            && individuo.Periodo_3_4.Quarta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Quarta.Disciplina_3Horario != null
                            && individuo.Periodo_3_4.Quarta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 5 e 6 periodo
                        if (individuo.Periodo_5_6.Quarta.Disciplina_1Horario != null
                            && individuo.Periodo_5_6.Quarta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Quarta.Disciplina_2Horario != null
                            && individuo.Periodo_5_6.Quarta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Quarta.Disciplina_3Horario != null
                            && individuo.Periodo_5_6.Quarta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 7 e 8 periodo
                        if (individuo.Periodo_7_8.Quarta.Disciplina_1Horario != null
                            && individuo.Periodo_7_8.Quarta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Quarta.Disciplina_2Horario != null
                            && individuo.Periodo_7_8.Quarta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Quarta.Disciplina_3Horario != null
                            && individuo.Periodo_7_8.Quarta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                    }
                    if (!item.Quinta)
                    {
                        var professor = item.Professor.Nome;
                        // 1 e 2 periodo
                        if (individuo.Periodo_1_2.Quinta.Disciplina_1Horario != null
                            && individuo.Periodo_1_2.Quinta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Quinta.Disciplina_2Horario != null
                            && individuo.Periodo_1_2.Quinta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Quinta.Disciplina_3Horario != null
                            && individuo.Periodo_1_2.Quinta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 3 e 4 periodo
                        if (individuo.Periodo_3_4.Quinta.Disciplina_1Horario != null
                            && individuo.Periodo_3_4.Quinta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Quinta.Disciplina_2Horario != null
                            && individuo.Periodo_3_4.Quinta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Quinta.Disciplina_3Horario != null
                            && individuo.Periodo_3_4.Quinta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 5 e 6 periodo
                        if (individuo.Periodo_5_6.Quinta.Disciplina_1Horario != null
                            && individuo.Periodo_5_6.Quinta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Quinta.Disciplina_2Horario != null
                            && individuo.Periodo_5_6.Quinta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Quinta.Disciplina_3Horario != null
                            && individuo.Periodo_5_6.Quinta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 7 e 8 periodo
                        if (individuo.Periodo_7_8.Quinta.Disciplina_1Horario != null
                            && individuo.Periodo_7_8.Quinta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Quinta.Disciplina_2Horario != null
                            && individuo.Periodo_7_8.Quinta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Quinta.Disciplina_3Horario != null
                            && individuo.Periodo_7_8.Quinta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                    }
                    if (!item.Sexta)
                    {
                        var professor = item.Professor.Nome;
                        // 1 e 2 periodo
                        if (individuo.Periodo_1_2.Sexta.Disciplina_1Horario != null
                            && individuo.Periodo_1_2.Sexta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Sexta.Disciplina_2Horario != null
                            && individuo.Periodo_1_2.Sexta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_1_2.Sexta.Disciplina_3Horario != null
                            && individuo.Periodo_1_2.Sexta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 3 e 4 periodo
                        if (individuo.Periodo_3_4.Sexta.Disciplina_1Horario != null
                            && individuo.Periodo_3_4.Sexta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Sexta.Disciplina_2Horario != null
                            && individuo.Periodo_3_4.Sexta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_3_4.Sexta.Disciplina_3Horario != null
                            && individuo.Periodo_3_4.Sexta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 5 e 6 periodo
                        if (individuo.Periodo_5_6.Sexta.Disciplina_1Horario != null
                            && individuo.Periodo_5_6.Sexta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Sexta.Disciplina_2Horario != null
                            && individuo.Periodo_5_6.Sexta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_5_6.Sexta.Disciplina_3Horario != null
                            && individuo.Periodo_5_6.Sexta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;

                        // 7 e 8 periodo
                        if (individuo.Periodo_7_8.Sexta.Disciplina_1Horario != null
                            && individuo.Periodo_7_8.Sexta.Disciplina_1Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Sexta.Disciplina_2Horario != null
                            && individuo.Periodo_7_8.Sexta.Disciplina_2Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                        if (individuo.Periodo_7_8.Sexta.Disciplina_3Horario != null
                            && individuo.Periodo_7_8.Sexta.Disciplina_3Horario.Professor.Nome.Equals(professor))
                            individuo.Aptidao -= penalizacao;
                    }
                }
            }
            return individuo;
        }
        /* Disciplina de 60h ou 4 creditos com horario quebrado no mesmo dia */
        private Individuo Restricao3(Individuo individuo)
        {
            var disciplinas4cr = GetDisciplinasNCreditos(4);
            int penalizacao = 5;
            // 1 ou 2 periodo
            if (individuo.Periodo_1_2.Segunda.Disciplina_1Horario != null
                && individuo.Periodo_1_2.Segunda.Disciplina_3Horario != null
                && individuo.Periodo_1_2.Segunda.Disciplina_1Horario.Nome.Equals(individuo.Periodo_1_2.Segunda.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_1_2.Segunda.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_1_2.Terca.Disciplina_1Horario != null
                && individuo.Periodo_1_2.Terca.Disciplina_3Horario != null
                && individuo.Periodo_1_2.Terca.Disciplina_1Horario.Nome.Equals(individuo.Periodo_1_2.Terca.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_1_2.Terca.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_1_2.Quarta.Disciplina_1Horario != null
                && individuo.Periodo_1_2.Quarta.Disciplina_3Horario != null
                && individuo.Periodo_1_2.Quarta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_1_2.Quarta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_1_2.Quarta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_1_2.Quinta.Disciplina_1Horario != null
                && individuo.Periodo_1_2.Quinta.Disciplina_3Horario != null
                && individuo.Periodo_1_2.Quinta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_1_2.Quinta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_1_2.Quinta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_1_2.Sexta.Disciplina_1Horario != null
                && individuo.Periodo_1_2.Sexta.Disciplina_3Horario != null
                && individuo.Periodo_1_2.Sexta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_1_2.Sexta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_1_2.Sexta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }

            // 3 ou 4 periodo
            if (individuo.Periodo_3_4.Segunda.Disciplina_1Horario != null
                && individuo.Periodo_3_4.Segunda.Disciplina_3Horario != null
                && individuo.Periodo_3_4.Segunda.Disciplina_1Horario.Nome.Equals(individuo.Periodo_3_4.Segunda.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_3_4.Segunda.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_3_4.Terca.Disciplina_1Horario != null
                && individuo.Periodo_3_4.Terca.Disciplina_3Horario != null
                && individuo.Periodo_3_4.Terca.Disciplina_1Horario.Nome.Equals(individuo.Periodo_3_4.Terca.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_3_4.Terca.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_3_4.Quarta.Disciplina_1Horario != null
                && individuo.Periodo_3_4.Quarta.Disciplina_3Horario != null
                && individuo.Periodo_3_4.Quarta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_3_4.Quarta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_3_4.Quarta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_3_4.Quinta.Disciplina_1Horario != null
                && individuo.Periodo_3_4.Quinta.Disciplina_3Horario != null
                && individuo.Periodo_3_4.Quinta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_3_4.Quinta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_3_4.Quinta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_3_4.Sexta.Disciplina_1Horario != null
                && individuo.Periodo_3_4.Sexta.Disciplina_3Horario != null
                && individuo.Periodo_3_4.Sexta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_3_4.Sexta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_3_4.Sexta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }

            // 5 ou 6 periodo
            if (individuo.Periodo_5_6.Segunda.Disciplina_1Horario != null
                && individuo.Periodo_5_6.Segunda.Disciplina_3Horario != null
                && individuo.Periodo_5_6.Segunda.Disciplina_1Horario.Nome.Equals(individuo.Periodo_5_6.Segunda.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_5_6.Segunda.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_5_6.Terca.Disciplina_1Horario != null
                && individuo.Periodo_5_6.Terca.Disciplina_3Horario != null
                && individuo.Periodo_5_6.Terca.Disciplina_1Horario.Nome.Equals(individuo.Periodo_5_6.Terca.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_5_6.Terca.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_5_6.Quarta.Disciplina_1Horario != null
                && individuo.Periodo_5_6.Quarta.Disciplina_3Horario != null
                && individuo.Periodo_5_6.Quarta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_5_6.Quarta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_5_6.Quarta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_5_6.Quinta.Disciplina_1Horario != null
                && individuo.Periodo_5_6.Quinta.Disciplina_3Horario != null
                && individuo.Periodo_5_6.Quinta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_5_6.Quinta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_5_6.Quinta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_5_6.Sexta.Disciplina_1Horario != null
                && individuo.Periodo_5_6.Sexta.Disciplina_3Horario != null
                && individuo.Periodo_5_6.Sexta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_5_6.Sexta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_5_6.Sexta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }

            // 7 ou 8 periodo
            if (individuo.Periodo_7_8.Segunda.Disciplina_1Horario != null
                && individuo.Periodo_7_8.Segunda.Disciplina_3Horario != null
                && individuo.Periodo_7_8.Segunda.Disciplina_1Horario.Nome.Equals(individuo.Periodo_7_8.Segunda.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_7_8.Segunda.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_7_8.Terca.Disciplina_1Horario != null
                && individuo.Periodo_7_8.Terca.Disciplina_3Horario != null
                && individuo.Periodo_7_8.Terca.Disciplina_1Horario.Nome.Equals(individuo.Periodo_7_8.Terca.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_7_8.Terca.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_7_8.Quarta.Disciplina_1Horario != null
                && individuo.Periodo_7_8.Quarta.Disciplina_3Horario != null
                && individuo.Periodo_7_8.Quarta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_7_8.Quarta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_7_8.Quarta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_7_8.Quinta.Disciplina_1Horario != null
                && individuo.Periodo_7_8.Quinta.Disciplina_3Horario != null
                && individuo.Periodo_7_8.Quinta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_7_8.Quinta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_7_8.Quinta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            if (individuo.Periodo_7_8.Sexta.Disciplina_1Horario != null
                && individuo.Periodo_7_8.Sexta.Disciplina_3Horario != null
                && individuo.Periodo_7_8.Sexta.Disciplina_1Horario.Nome.Equals(individuo.Periodo_7_8.Sexta.Disciplina_3Horario.Nome)
                && disciplinas4cr.Contains(individuo.Periodo_7_8.Sexta.Disciplina_1Horario))
            {
                individuo.Aptidao -= penalizacao;
            }
            return individuo;
        }
        /*Restricao de Dósea - aulas em dias seguidos*/
        private Individuo RestricaoDosea(Individuo individuo)
        {
            int penalizacao = 5;
            var disciplinasDosea = GetDisciplinasProfessor("Marcos Dósea");

            //se a lista de disciplinas de Dosea conter a disciplina do referido horario
            // verificar se ela tbm esta no dia seguinte e penalizar
            #region 
            Boolean contemNoPrimeiroDia1_2Periodo = ((disciplinasDosea.Contains(individuo.Periodo_1_2.Segunda.Disciplina_1Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Segunda.Disciplina_2Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Segunda.Disciplina_3Horario)));

            Boolean contemNoSegundoDia1_2Periodo = ((disciplinasDosea.Contains(individuo.Periodo_1_2.Terca.Disciplina_1Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Terca.Disciplina_2Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Quarta.Disciplina_3Horario)));

            Boolean contemNoTerceiroDia1_2Periodo = ((disciplinasDosea.Contains(individuo.Periodo_1_2.Quarta.Disciplina_1Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Quarta.Disciplina_2Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Quinta.Disciplina_3Horario)));

            Boolean contemNoQuartoDia1_2Periodo = ((disciplinasDosea.Contains(individuo.Periodo_1_2.Quinta.Disciplina_1Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Quinta.Disciplina_2Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Sexta.Disciplina_3Horario)));

            Boolean contemNoQuintaDia1_2Periodo = ((disciplinasDosea.Contains(individuo.Periodo_1_2.Sexta.Disciplina_1Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Sexta.Disciplina_2Horario))
                 || (disciplinasDosea.Contains(individuo.Periodo_1_2.Sexta.Disciplina_3Horario)));
            if (contemNoPrimeiroDia1_2Periodo && contemNoSegundoDia1_2Periodo || contemNoSegundoDia1_2Periodo && contemNoTerceiroDia1_2Periodo
               || contemNoTerceiroDia1_2Periodo && contemNoQuartoDia1_2Periodo || contemNoQuartoDia1_2Periodo && contemNoQuintaDia1_2Periodo)
            {
                individuo.Aptidao -= penalizacao;
            }
            #endregion
            #region
            Boolean contemNoPrimeiroDia3_4Periodo = ((disciplinasDosea.Contains(individuo.Periodo_3_4.Segunda.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Segunda.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Segunda.Disciplina_3Horario)));
            Boolean contemNoSegundoDia3_4Periodo = ((disciplinasDosea.Contains(individuo.Periodo_3_4.Terca.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Terca.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Terca.Disciplina_3Horario)));
            Boolean contemNoTerceiroDia3_4Periodo = ((disciplinasDosea.Contains(individuo.Periodo_3_4.Quarta.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Quarta.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Quarta.Disciplina_3Horario)));
            Boolean contemNoQuartoDia3_4Periodo = ((disciplinasDosea.Contains(individuo.Periodo_3_4.Quinta.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Quinta.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Quinta.Disciplina_3Horario)));
            Boolean contemNoQuintoDia3_4Periodo = ((disciplinasDosea.Contains(individuo.Periodo_3_4.Sexta.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Sexta.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_3_4.Sexta.Disciplina_3Horario)));
            if (contemNoPrimeiroDia3_4Periodo && contemNoSegundoDia3_4Periodo || contemNoSegundoDia3_4Periodo && contemNoTerceiroDia3_4Periodo
               || contemNoTerceiroDia3_4Periodo && contemNoQuartoDia3_4Periodo || contemNoQuartoDia3_4Periodo && contemNoQuintoDia3_4Periodo)
            {
                individuo.Aptidao -= penalizacao;
            }
            #endregion
            #region
            Boolean contemNoPrimeiroDia5_6Periodo = ((disciplinasDosea.Contains(individuo.Periodo_5_6.Segunda.Disciplina_1Horario))
               || (disciplinasDosea.Contains(individuo.Periodo_5_6.Segunda.Disciplina_2Horario))
               || (disciplinasDosea.Contains(individuo.Periodo_5_6.Segunda.Disciplina_3Horario)));
            Boolean contemNoSegundoDia5_6Periodo = ((disciplinasDosea.Contains(individuo.Periodo_5_6.Terca.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_5_6.Terca.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_5_6.Terca.Disciplina_3Horario)));
            Boolean contemNoTerceiroDia5_6Periodo = ((disciplinasDosea.Contains(individuo.Periodo_5_6.Quarta.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_5_6.Quarta.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_5_6.Quarta.Disciplina_3Horario)));
            Boolean contemNoQuartoDia5_6Periodo = ((disciplinasDosea.Contains(individuo.Periodo_5_6.Quinta.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_5_6.Quinta.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_5_6.Quinta.Disciplina_3Horario)));
            Boolean contemNoQuintoDia5_6Periodo = ((disciplinasDosea.Contains(individuo.Periodo_5_6.Sexta.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_5_6.Sexta.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_5_6.Sexta.Disciplina_3Horario)));
            if (contemNoPrimeiroDia5_6Periodo && contemNoSegundoDia5_6Periodo || contemNoSegundoDia5_6Periodo && contemNoTerceiroDia5_6Periodo
               || contemNoTerceiroDia5_6Periodo && contemNoQuartoDia5_6Periodo || contemNoQuartoDia5_6Periodo && contemNoQuintoDia5_6Periodo)
            {
                individuo.Aptidao -= penalizacao;
            }

            #endregion
            #region
            Boolean contemNoPrimeiroDia7_8Periodo = ((disciplinasDosea.Contains(individuo.Periodo_7_8.Segunda.Disciplina_1Horario))
               || (disciplinasDosea.Contains(individuo.Periodo_7_8.Segunda.Disciplina_2Horario))
               || (disciplinasDosea.Contains(individuo.Periodo_7_8.Segunda.Disciplina_3Horario)));
            Boolean contemNoSegundoDia7_8Periodo = ((disciplinasDosea.Contains(individuo.Periodo_7_8.Terca.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_7_8.Terca.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_7_8.Terca.Disciplina_3Horario)));
            Boolean contemNoTerceiroDia7_8Periodo = ((disciplinasDosea.Contains(individuo.Periodo_7_8.Quarta.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_7_8.Quarta.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_7_8.Quarta.Disciplina_3Horario)));
            Boolean contemNoQuartoDia7_8Periodo = ((disciplinasDosea.Contains(individuo.Periodo_7_8.Quinta.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_7_8.Quinta.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_7_8.Quinta.Disciplina_3Horario)));
            Boolean contemNoQuintoDia7_8Periodo = ((disciplinasDosea.Contains(individuo.Periodo_7_8.Sexta.Disciplina_1Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_7_8.Sexta.Disciplina_2Horario))
                || (disciplinasDosea.Contains(individuo.Periodo_7_8.Sexta.Disciplina_3Horario)));
            if (contemNoPrimeiroDia7_8Periodo && contemNoSegundoDia7_8Periodo || contemNoSegundoDia7_8Periodo && contemNoTerceiroDia7_8Periodo
               || contemNoTerceiroDia7_8Periodo && contemNoQuartoDia7_8Periodo || contemNoQuartoDia7_8Periodo && contemNoQuintoDia7_8Periodo)
            {
                individuo.Aptidao -= penalizacao;
            }
            #endregion
            return individuo;
        }
        private List<Disciplina> GetDisciplinasProfessor(string nome)
            => _contexto.Disciplinas.Where(n => n.Professor.Nome.Equals(nome)).ToList();
        private List<Disciplina> GetDisciplinasNCreditos(int nHoras)
            => _contexto
                .Disciplinas
                .Where(d => d.Horas == nHoras)
                .ToList();
        // Metodo criado para verificar a existência de uma disciplina já cadastrada no indiviuo em determinado dia e horario
        private bool ExisteDisciplinaNoIndividuo(Periodo periodo, int dia, int horarioDisciplina)
        {
            switch (dia)
            {
                case 1: // segunda
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Segunda.Disciplina_1Horario != null) // já existe disciplina cadastrada
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Segunda.Disciplina_2Horario != null)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Segunda.Disciplina_3Horario != null)
                            return true;
                    break;
                case 2: // terça
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Terca.Disciplina_1Horario != null)
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Terca.Disciplina_2Horario != null)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Terca.Disciplina_3Horario != null)
                            return true;
                    break;
                case 3: // quarta
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Quarta.Disciplina_1Horario != null)
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Quarta.Disciplina_2Horario != null)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Quarta.Disciplina_3Horario != null)
                            return true;
                    break;
                case 4: // quinta
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Quinta.Disciplina_1Horario != null)
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Quinta.Disciplina_2Horario != null)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Quinta.Disciplina_3Horario != null)
                            return true;
                    break;
                case 5: // sexta
                    if (horarioDisciplina == 1) //1º horario
                        if (periodo.Sexta.Disciplina_1Horario != null)
                            return true;
                    if (horarioDisciplina == 2) //2º horario
                        if (periodo.Sexta.Disciplina_2Horario != null)
                            return true;
                    if (horarioDisciplina == 3) //3º horario
                        if (periodo.Sexta.Disciplina_3Horario != null)
                            return true;
                    break;
            }
            return false;
        }
        // retorna dia e horario respectivamente
        private (int, int, List<Sorteador>) GetSortDiaOrHorario(List<Sorteador> sorts, int periodo)
        {
            var auxSorts = sorts.Where(s => s.Periodo == periodo).ToList();
            Random random = new Random();
            var index = random.Next(0, auxSorts.Count);
            var idSorteador = auxSorts.ElementAt(index).Id;
            var sorteador = sorts.SingleOrDefault(s => s.Id == idSorteador);
            sorts.Remove(sorteador);
            return (sorteador.Dia, sorteador.Horario, sorts);
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
        private List<int> GetPeriodos(string ano)
            => _contexto
            .Horarios
            .Where(h => h.Ano.Periodo.Equals(ano))
            .Select(h => h.Disciplina.Periodo)
            .ToList();
        private Individuo AtribuirDisciplinaAoIndividuo(Individuo individuo, Disciplina disciplina, int dia, int horarioDisciplina)
        {
            switch (dia)
            {
                case 1: // segunda
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Segunda.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Segunda.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Segunda.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Segunda.Disciplina_1Horario = disciplina;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Segunda.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Segunda.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Segunda.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Segunda.Disciplina_2Horario = disciplina;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Segunda.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Segunda.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Segunda.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Segunda.Disciplina_3Horario = disciplina;
                    }
                    break;
                case 2: // terça
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Terca.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Terca.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Terca.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Terca.Disciplina_1Horario = disciplina;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Terca.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Terca.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Terca.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Terca.Disciplina_2Horario = disciplina;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Terca.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Terca.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Terca.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Terca.Disciplina_3Horario = disciplina;
                    }
                    break;
                case 3: // quarta
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quarta.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quarta.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quarta.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quarta.Disciplina_1Horario = disciplina;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quarta.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quarta.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quarta.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quarta.Disciplina_2Horario = disciplina;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quarta.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quarta.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quarta.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quarta.Disciplina_3Horario = disciplina;
                    }
                    break;
                case 4: // quinta
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quinta.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quinta.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quinta.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quinta.Disciplina_1Horario = disciplina;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quinta.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quinta.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quinta.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quinta.Disciplina_2Horario = disciplina;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Quinta.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Quinta.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Quinta.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Quinta.Disciplina_3Horario = disciplina;
                    }
                    break;
                case 5: // sexta
                    if (horarioDisciplina == 1)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Sexta.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Sexta.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Sexta.Disciplina_1Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Sexta.Disciplina_1Horario = disciplina;
                    }
                    if (horarioDisciplina == 2)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Sexta.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Sexta.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Sexta.Disciplina_2Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Sexta.Disciplina_2Horario = disciplina;
                    }
                    if (horarioDisciplina == 3)
                    {
                        if (disciplina.Periodo == 1 || disciplina.Periodo == 2)
                            individuo.Periodo_1_2.Sexta.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 3 || disciplina.Periodo == 4)
                            individuo.Periodo_3_4.Sexta.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 5 || disciplina.Periodo == 6)
                            individuo.Periodo_5_6.Sexta.Disciplina_3Horario = disciplina;

                        if (disciplina.Periodo == 7 || disciplina.Periodo == 8)
                            individuo.Periodo_7_8.Sexta.Disciplina_3Horario = disciplina;
                    }
                    break;
            }
            return individuo;
        }
        private List<Sorteador> RemoveHorarioComReserva(List<Sorteador> sorts, string ano)
        {
            var restricoes = GetRestricoesHorarios(ano);

            var segunda = 1;
            var terca = 2;
            var quarta = 3;
            var quinta = 4;
            var sexta = 5;

            var horario1 = 1;
            var horario2 = 2;
            var horario3 = 3;

            foreach (var item in restricoes)
            {
                if (item.SegundaHorario1)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == segunda && s.Horario == horario1 && s.Periodo == item.Periodo));
                if (item.SegundaHorario2)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == segunda && s.Horario == horario2 && s.Periodo == item.Periodo));
                if (item.SegundaHorario3)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == segunda && s.Horario == horario3 && s.Periodo == item.Periodo));

                if (item.TercaHorario1)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == terca && s.Horario == horario1 && s.Periodo == item.Periodo));
                if (item.TercaHorario2)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == terca && s.Horario == horario2 && s.Periodo == item.Periodo));
                if (item.TercaHorario3)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == terca && s.Horario == horario3 && s.Periodo == item.Periodo));

                if (item.QuartaHorario1)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == quarta && s.Horario == horario1 && s.Periodo == item.Periodo));
                if (item.QuartaHorario2)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == quarta && s.Horario == horario2 && s.Periodo == item.Periodo));
                if (item.QuartaHorario3)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == quarta && s.Horario == horario3 && s.Periodo == item.Periodo));

                if (item.QuintaHorario1)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == quinta && s.Horario == horario1 && s.Periodo == item.Periodo));
                if (item.QuintaHorario2)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == quinta && s.Horario == horario2 && s.Periodo == item.Periodo));
                if (item.QuintaHorario3)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == quinta && s.Horario == horario3 && s.Periodo == item.Periodo));

                if (item.SextaHorario1)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == sexta && s.Horario == horario1 && s.Periodo == item.Periodo));
                if (item.SextaHorario2)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == sexta && s.Horario == horario2 && s.Periodo == item.Periodo));
                if (item.SextaHorario3)
                    sorts.Remove(sorts.SingleOrDefault(s => s.Dia == sexta && s.Horario == horario3 && s.Periodo == item.Periodo));
            }
            return sorts;
        }
        // Restrição para horarios de outros departamento em que as disciplinas já são fixas
        private List<RestricaoHorario> GetRestricoesHorarios(string ano)
            => _contexto
            .RestricaoHorarios
            .Where(rh => rh.Ano.Periodo.Equals(ano))
            .ToList();
        // Metodo com os dias que os professores se fazem disponiveis 
        private List<RestricaoProfessor> GetDisponibilidadeProfessores(string ano)
            => _contexto
            .RestricoesProfessores
            .Include(rp => rp.Ano)
            .Include(rp => rp.Professor)
            .Where(rp => rp.Ano.Periodo.Equals(ano))
            .ToList();
    }
}
