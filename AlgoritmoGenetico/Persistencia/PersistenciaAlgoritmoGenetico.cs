using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    public class PersistenciaAlgoritmoGenetico
    {
        private readonly int qtdIndividuos = 16;
        private readonly DbContextAG _contexto;
        private Individuo individuo;
        public PersistenciaAlgoritmoGenetico(DbContextAG contexto)
        {
            _contexto = contexto;
        }

        public List<Individuo> InicializarPopulação(string ano)
        {
            List<Individuo> arrayIndividuo = new List<Individuo>();
            int maxIdHorario = int.MinValue;
            int minIdHorario = int.MaxValue;

            var idsHorarios = getIdHorarios(ano);

            foreach (var item in idsHorarios)
            {
                if (item < minIdHorario)
                    minIdHorario = item;
                if (item > maxIdHorario)
                    maxIdHorario = item;
            }

            Random random = new Random();

            for (int i = 0; i < qtdIndividuos; i++)
            {
                var idAleatorioDisciplina = random.Next(minIdHorario, maxIdHorario);
                while ( ! idsHorarios.Contains(idAleatorioDisciplina))
                {
                    idAleatorioDisciplina = random.Next(minIdHorario, maxIdHorario);
                }
                var disciplina = getDisciplina(idAleatorioDisciplina);

                // não posso ter individuo com mais de uma disciplina igual
                idsHorarios.Remove(idAleatorioDisciplina);

                individuo = new Individuo();
                

            }
            return null;
        }

        private Disciplina getDisciplina(int idDisciplina)
        {
            return _contexto
                .Disciplinas
                .Include(d => d.Professor)
                .SingleOrDefault(d => d.Id == idDisciplina);
        }

        private List<int> getIdHorarios(string ano)
        {
            return _contexto
                .Horarios
                .Where(h => h.Ano.Periodo.Equals(ano))
                .Select(h => h.IdDisciplina)
                .ToList();
        }

    }
}
