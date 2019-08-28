using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Persistencia;

namespace WEB.Controllers
{
    public class RestricaoProfessorController : Controller
    {
        private readonly PersistenciaRestricaoProfessor persistenciaRestricaoProfessor;
        private readonly PersistenciaProfessor persistenciaProfessor;
        private readonly PersistenciaAno persistenciaAno;

        public RestricaoProfessorController(DbContextAG contexto)
        {
            persistenciaRestricaoProfessor = new PersistenciaRestricaoProfessor(contexto);
            persistenciaProfessor = new PersistenciaProfessor(contexto);
            persistenciaAno = new PersistenciaAno(contexto);
        }

        // GET: RestricaoProfessor
        public IActionResult Index()
        {
            var lista = persistenciaRestricaoProfessor.ObterTodos();
            return View(lista);
        }

        // GET: RestricaoProfessor/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: RestricaoProfessor/Create
        public IActionResult Cadastrar()
        {
            ViewBag.AllProfessores = new SelectList(persistenciaProfessor.ObterTodos(), "Id", "Nome");
            ViewBag.AllAno = new SelectList(persistenciaAno.ObterTodos(), "Id", "Periodo");
            return View();
        }

        // POST: RestricaoProfessor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(RestricaoProfessor restricao)
        {
            if (ModelState.IsValid)
            {
                persistenciaRestricaoProfessor.Adiconar(restricao);
                return RedirectToAction(nameof(Index));
            }
            return View(restricao);
        }

        // GET: RestricaoProfessor/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: RestricaoProfessor/Edit/5
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

        // GET: RestricaoProfessor/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: RestricaoProfessor/Delete/5
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