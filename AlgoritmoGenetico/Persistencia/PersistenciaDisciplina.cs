using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Persistencia
{
    public class PersistenciaDisciplina
    {
        private readonly DbContextAG _contexto;
        public PersistenciaDisciplina(DbContextAG contexto)
        {
            _contexto = contexto;
        }

        public List<Disciplina> ObterTodos()
            => _contexto
            .Disciplinas
            .Include(p => p.Professor)
            .OrderBy(p => p.Periodo)
            .ToList();

        public List<Disciplina> ObterTodosComProfessor()
            => _contexto
            .Disciplinas
            .OrderBy(p => p.Periodo)
            .Include(p => p.Professor)
            .Select(p => new Disciplina
            {
                Id = p.Id,
                Nome = p.Nome + " -> " + p.Professor.Nome
            })
            .ToList();

        public void Adicionar(Disciplina disciplina)
        {
            _contexto.Disciplinas.Add(disciplina);
            _contexto.SaveChanges();
        }
    }
}
