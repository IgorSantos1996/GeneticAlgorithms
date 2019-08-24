using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Persistencia
{
    public class PersistenciaHorario
    {
        private readonly DbContextAG _contexto;

        public PersistenciaHorario(DbContextAG contexto)
        {
            _contexto = contexto;
        }

        public void Adicionar(Horario horario)
        {
            _contexto.Horarios.Add(horario);
            _contexto.SaveChanges();
        }

        public List<Horario> ObterTodos()
            => _contexto
            .Horarios
            .OrderBy(h => h.Disciplina.Periodo)
            .OrderBy(h => h.Ano.Periodo)
            .Include(h => h.Ano)
            .Include(h => h.Disciplina)
            .Include(h => h.Disciplina.Professor)
            .ToList();
    }
}
