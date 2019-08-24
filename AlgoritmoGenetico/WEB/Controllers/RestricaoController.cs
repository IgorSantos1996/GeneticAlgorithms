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
    public class RestricaoController : Controller
    {
        private readonly PersistenciaRestricao persistenciaRestricao;
        private readonly PersistenciaProfessor persistenciaProfessor;
        private readonly PersistenciaAno persistenciaAno;

        public RestricaoController(DbContextAG contexto)
        {
            persistenciaRestricao = new PersistenciaRestricao(contexto);
            persistenciaProfessor = new PersistenciaProfessor(contexto);
            persistenciaAno = new PersistenciaAno(contexto);
        }

        // GET: Restricao
        public IActionResult Index()
        {
            var lista = persistenciaRestricao.ObterTodos();
            return View(lista);
        }

        // GET: Restricao/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Restricao/Create
        public IActionResult Cadastrar()
        {
            ViewBag.AllProfessores = new SelectList(persistenciaProfessor.ObterTodos(), "Id", "Nome");
            ViewBag.AllAno = new SelectList(persistenciaAno.ObterTodos(), "Id", "Periodo");
            return View();
        }

        // POST: Restricao/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Restricao restricao)
        {
            if (ModelState.IsValid)
            {
                persistenciaRestricao.Adiconar(restricao);
                return RedirectToAction(nameof(Index));
            }
            return View(restricao);
        }

        // GET: Restricao/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Restricao/Edit/5
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

        // GET: Restricao/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Restricao/Delete/5
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