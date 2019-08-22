using Microsoft.EntityFrameworkCore;
using Models;

namespace Persistencia
{
    public class DbContextAG : DbContext
    {
        public DbContextAG(DbContextOptions<DbContextAG> options) : base (options) { }

        public DbSet<Professor> Professores { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Ano> Anos { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Restricao> Restricoes { get; set; }
    }
}
