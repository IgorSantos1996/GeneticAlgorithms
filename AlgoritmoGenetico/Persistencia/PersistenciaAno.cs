using Models;
using System.Collections.Generic;
using System.Linq;

namespace Persistencia
{
    public class PersistenciaAno
    {
        private readonly DbContextAG _contexto;

        public PersistenciaAno(DbContextAG contexto)
        {
            _contexto = contexto;
        }
        public void Adicionar (Ano ano)
        {
            _contexto.Anos.Add(ano);
            _contexto.SaveChanges();
        }

        public List<Ano> ObterTodos() => _contexto.Anos.ToList();
        public List<Ano> ObterTodos(string periodo) => _contexto.Anos.Where(a => a.Periodo.Equals(periodo)).ToList();
    }
}
