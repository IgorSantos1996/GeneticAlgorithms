using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Persistencia;

namespace WEB.Controllers
{
    public class RestricaoHorarioController : Controller
    {
        private readonly PersistenciaRestricaoHorario persistenciaRestricaoHorario;
        private readonly PersistenciaAno persistenciaAno;

        public RestricaoHorarioController(DbContextAG contexto)
        {
            persistenciaRestricaoHorario = new PersistenciaRestricaoHorario(contexto);
            persistenciaAno = new PersistenciaAno(contexto);
        }


        // GET: RestricaoHorario
        public IActionResult Index()
        {
            var lista = persistenciaRestricaoHorario.ObterTodos();
            return View(lista);
        }

        // GET: RestricaoHorario/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: RestricaoHorario/Create
        public IActionResult Cadastrar()
        {
            ViewBag.AllAno = new SelectList(persistenciaAno.ObterTodos(), "Id", "Periodo");
            return View();
        }

        // POST: RestricaoHorario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(RestricaoHorario restricaoHorario)
        {
            if (ModelState.IsValid)
            {
                persistenciaRestricaoHorario.Adicionar(restricaoHorario);
                return RedirectToAction(nameof(Index));
            }
            return View(restricaoHorario);
        }

        // GET: RestricaoHorario/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: RestricaoHorario/Edit/5
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

        // GET: RestricaoHorario/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: RestricaoHorario/Delete/5
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