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
    public class HorarioController : Controller
    {
        private readonly PersistenciaHorario persistenciaHorario;
        private readonly PersistenciaAno persistenciaAno;
        private readonly PersistenciaDisciplina persistenciaDisciplina;
        private readonly List<Horario> listaHorario;
        
        public HorarioController(DbContextAG contexto)
        {
            persistenciaHorario = new PersistenciaHorario(contexto);
            persistenciaAno = new PersistenciaAno(contexto);
            persistenciaDisciplina = new PersistenciaDisciplina(contexto);
            listaHorario = new List<Horario>();
        }

        // GET: Horario
        public ActionResult Index()
        {
            var lista = persistenciaHorario.ObterTodos();
            return View(lista);
        }

        // GET: Horario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Horario/Create
        public ActionResult Cadastrar(string periodo = "")
        {
            //if(string.IsNullOrEmpty(periodo))
                ViewBag.AllAno = new SelectList(persistenciaAno.ObterTodos(), "Id", "Periodo");
            //else
                //ViewBag.AllAno = new SelectList(persistenciaAno.ObterTodos(periodo), "Id", "Periodo");
            ViewBag.AllDisciplina = new SelectList(persistenciaDisciplina.ObterTodosComProfessor(), "Id", "Nome");
            
            return View();
        }

        // POST: Horario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Horario horario)
        {
            if (ModelState.IsValid)
            {
                persistenciaHorario.Adicionar(horario);
                return RedirectToAction(nameof(Index));
            }
            return View(horario);
        }

        // GET: Horario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Horario/Edit/5
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

        // GET: Horario/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Horario/Delete/5
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