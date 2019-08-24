using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Persistencia
{
    public class PersistenciaRestricaoHorario
    {
        private readonly DbContextAG _contexto;

        public PersistenciaRestricaoHorario(DbContextAG contexto)
        {
            _contexto = contexto;
        }

        public List<RestricaoHorario> ObterTodos()
        {
            return _contexto
                .RestricaoHorarios
                .Include(r => r.Ano)
                .ToList();
        }
        public void Adicionar(RestricaoHorario restricaoHorario)
        {
            _contexto.RestricaoHorarios.Add(restricaoHorario);
            _contexto.SaveChanges();
        }
    }
}
