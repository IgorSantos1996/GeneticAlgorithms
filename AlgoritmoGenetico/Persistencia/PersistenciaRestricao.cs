using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Persistencia
{
    public class PersistenciaRestricao
    {
        private readonly DbContextAG _contexto;
        public PersistenciaRestricao(DbContextAG contexto)
        {
            _contexto = contexto;
        }
        public void Adiconar(Restricao restricao)
        {
            _contexto.Restricoes.Add(restricao);
            _contexto.SaveChanges();
        }
        public List<Restricao> ObterTodos()
        {
            return _contexto
                .Restricoes
                .OrderBy(r => r.Ano.Periodo)
                .Include(r => r.Ano)
                .Include(r => r.Professor)
                .ToList();
        }
    }
}
