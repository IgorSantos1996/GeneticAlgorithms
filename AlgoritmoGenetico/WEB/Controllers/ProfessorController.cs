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
    public class ProfessorController : Controller
    {
        private readonly PersistenciaProfessor persistenciaProfessor;

        public ProfessorController(DbContextAG contexto)
            => persistenciaProfessor = new PersistenciaProfessor(contexto);

        // GET: Professor
        public IActionResult Index()
        {
            var lista = persistenciaProfessor.ObterTodos();
            return View(lista);
        }

        // GET: Professor/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: Professor/Create
        public IActionResult Cadastrar()
        {
            return View();
        }

        // POST: Professor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(Professor professor)
        {
            if (ModelState.IsValid)
            {
                persistenciaProfessor.Adicionar(professor);
                return RedirectToAction(nameof(Index));
            }
            return View(professor);
        }

        // GET: Professor/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Professor/Edit/5
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

        // GET: Professor/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: Professor/Delete/5
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