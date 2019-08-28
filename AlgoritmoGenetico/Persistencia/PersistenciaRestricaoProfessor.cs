using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    public class PersistenciaRestricaoProfessor
    {
        private readonly DbContextAG _contexto;
        public PersistenciaRestricaoProfessor(DbContextAG contexto)
        {
            _contexto = contexto;
        }
        public void Adiconar(RestricaoProfessor restricao)
        {
            _contexto.RestricoesProfessores.Add(restricao);
            _contexto.SaveChanges();
        }
        public List<RestricaoProfessor> ObterTodos()
        {
            return _contexto
                .RestricoesProfessores
                .OrderBy(r => r.Ano.Periodo)
                .Include(r => r.Ano)
                .Include(r => r.Professor)
                .ToList();
        }
    }
}
