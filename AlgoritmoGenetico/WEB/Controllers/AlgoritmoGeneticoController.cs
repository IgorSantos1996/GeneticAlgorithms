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
    public class AlgoritmoGeneticoController : Controller
    {
        private readonly PersistenciaAlgoritmoGenetico persistenciaAlgoritmoGenetico;
        private readonly PersistenciaAno persistenciaAno;

        public AlgoritmoGeneticoController(DbContextAG contexto)
        {
            persistenciaAlgoritmoGenetico = new PersistenciaAlgoritmoGenetico(contexto);
            persistenciaAno = new PersistenciaAno(contexto);
        }

        // GET: AlgoritmoGenetico
        public IActionResult Index(string ano)
        {
            var populacao = persistenciaAlgoritmoGenetico.AlgoritmoGenetico(ano);
            return View(populacao);
        }

        public IActionResult SelecionarAno()
        {
            ViewBag.AllAno = new SelectList(persistenciaAno.ObterTodos(), "Periodo", "Periodo");
            return View();
        }

        // GET: AlgoritmoGenetico/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AlgoritmoGenetico/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlgoritmoGenetico/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AlgoritmoGenetico/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AlgoritmoGenetico/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: AlgoritmoGenetico/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AlgoritmoGenetico/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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