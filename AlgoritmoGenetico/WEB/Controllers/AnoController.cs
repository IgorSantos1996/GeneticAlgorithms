using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Persistencia;

namespace WEB.Controllers
{
    public class AnoController : Controller
    {
        private readonly PersistenciaAno persistenciaAno;

        public AnoController(DbContextAG contexto) => persistenciaAno = new PersistenciaAno(contexto);

        // GET: Ano
        public IActionResult Index()
        {
            var lista = persistenciaAno.ObterTodos();
            return View(lista);
        }

        // GET: Ano/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Ano/Create
        public IActionResult Cadastrar()
        {
            return View();
        }

        // POST: Ano/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Ano ano)
        {
            if (ModelState.IsValid)
            {
                persistenciaAno.Adicionar(ano);
                return RedirectToAction(nameof(Index));
            }
            return View(ano);
        }

        // GET: Ano/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Ano/Edit/5
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

        // GET: Ano/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Ano/Delete/5
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