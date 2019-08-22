using Models;
using System.Collections.Generic;
using System.Linq;

namespace Persistencia
{
    public class PersistenciaProfessor
    {
        private readonly DbContextAG _contexto;
        public PersistenciaProfessor(DbContextAG contexto)
        {
            _contexto = contexto;
        }

        public List<Professor> ObterTodos() => _contexto.Professores.ToList();

        public void Adicionar(Professor professor)
        {
            _contexto.Professores.Add(professor);
            _contexto.SaveChanges();
        }
    }
}
