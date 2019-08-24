using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Persistencia;

namespace WEB.Controllers
{
    public class DisciplinaController : Controller
    {
        private readonly PersistenciaDisciplina persistenciaDisciplina;
        private readonly PersistenciaProfessor persistenciaProfessor;

        public DisciplinaController(DbContextAG contexto)
        {
            persistenciaDisciplina = new PersistenciaDisciplina(contexto);
            persistenciaProfessor = new PersistenciaProfessor(contexto);
        }

        // GET: Disciplina
        public IActionResult Index()
        {
            var lista = persistenciaDisciplina.ObterTodos();
            return View(lista);
        }

        // GET: Disciplina/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Disciplina/Create
        public IActionResult Cadastrar()
        {
            ViewBag.AllProfessores = new SelectList(persistenciaProfessor.ObterTodos(), "Id", "Nome");
            return View();
        }

        // POST: Disciplina/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Disciplina disciplina)
        {
            if (ModelState.IsValid)
            {
                persistenciaDisciplina.Adicionar(disciplina);
                return RedirectToAction(nameof(Index));
            }
            return View(disciplina);
        }

        // GET: Disciplina/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Disciplina/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Disciplina/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Disciplina/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}