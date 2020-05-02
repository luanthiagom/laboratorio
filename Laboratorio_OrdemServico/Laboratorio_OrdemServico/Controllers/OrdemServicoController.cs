using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using Laboratorio.Models;
using Laboratorio.ViewModel;

namespace WebApplication1.Controllers
{
    public class OrdemServicoController : Controller
    {
        private LaboratorioEntities db = new LaboratorioEntities();

        // GET: OrdemServico
        public ActionResult Index()
        {
            var ordemServico = db.OrdemServico;
            return View(ordemServico);
        }

        // GET: OrdemServico/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                OrdemServico ordemServico = db.OrdemServico.Find(id);
                if (ordemServico == null)
                {
                    return HttpNotFound();
                }
                return View(ordemServico);
            }
            catch
            {
                return View(new OrdemServico());
            }
        }

        // GET: OrdemServico/Create
        public ActionResult Create()
        {
            OrdemServico ordemServico = new OrdemServico();
            ordemServico.Data = DateTime.Now;

            ViewBag.PacienteId = new SelectList(db.Paciente, "PacienteId", "Nome");
            ViewBag.PostoColetaId = new SelectList(db.PostoColeta, "PostoColetaId", "Descricao");
            ViewBag.MedicoId = new SelectList(db.Medico, "MedicoId", "Nome");
            ViewBag.ExameId = new SelectList(db.Exame, "ExameId", "Descricao");
            return View(ordemServico);
        }

        // POST: OrdemServico/Create
        [HttpPost]
        public ActionResult Create(OrdemServicoViewModel ordem)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    if (ordem.OrdemServicoExame != null && ordem.OrdemServicoExame.Count > 0)
                    {
                        foreach (var item in ordem.OrdemServicoExame)
                        {
                            item.Preco = db.Exame.Find(item.ExameId).Preco;
                        }
                    }

                    OrdemServico ordemServico = new OrdemServico
                    {
                        Convenio = ordem.Convenio,
                        Data = ordem.Data,
                        MedicoId = ordem.MedicoId,
                        PostoColetaId = ordem.PostoColetaId,
                        PacienteId = ordem.PacienteId,
                        OrdemServicoExame = ordem.OrdemServicoExame
                    };

                    db.OrdemServico.Add(ordemServico);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View(ordem);
            }
        }

        // GET: OrdemServico/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                OrdemServico ordemServico = db.OrdemServico.Find(id);
                if (ordemServico == null)
                {
                    return HttpNotFound();
                }

                ViewBag.PacienteId = new SelectList(db.Paciente, "PacienteId", "Nome", ordemServico.PacienteId);
                ViewBag.PostoColetaId = new SelectList(db.PostoColeta, "PostoColetaId", "Descricao", ordemServico.PostoColetaId);
                ViewBag.MedicoId = new SelectList(db.Medico, "MedicoId", "Nome", ordemServico.MedicoId);
                ViewBag.ExameId = new SelectList(db.Exame, "ExameId", "Descricao");

                return View(ordemServico);
            }
            catch
            {
                return View(new OrdemServico());
            }
        }

        // POST: OrdemServico/Edit/5
        [HttpPost]
        public ActionResult Edit(OrdemServicoViewModel ordem)
        {
            ViewBag.PacienteId = new SelectList(db.Paciente, "PacienteId", "Nome", ordem.PacienteId);
            ViewBag.PostoColetaId = new SelectList(db.PostoColeta, "PostoColetaId", "Descricao", ordem.PostoColetaId);
            ViewBag.MedicoId = new SelectList(db.Medico, "MedicoId", "Nome", ordem.MedicoId);
            ViewBag.ExameId = new SelectList(db.Exame, "ExameId", "Descricao");

            var ordemServicoCompleto = db.OrdemServico.Find(ordem.OrdemServicoId);

            ordemServicoCompleto.Convenio = ordem.Convenio;
            ordemServicoCompleto.Data = ordem.Data;
            ordemServicoCompleto.MedicoId = ordem.MedicoId;
            ordemServicoCompleto.PostoColetaId = ordem.PostoColetaId;
            ordemServicoCompleto.PacienteId = ordem.PacienteId;

            try
            {
                if (ModelState.IsValid)
                {
                    if (ordem.OrdemServicoExame != null)
                    {
                        foreach (var item in ordem.OrdemServicoExame)
                        {
                            if (item.OrdemServicoExameId == 0)
                            {
                                item.OrdemServicoId = ordemServicoCompleto.OrdemServicoId;
                                ordemServicoCompleto.OrdemServicoExame.Add(item);
                                db.Entry(item).State = EntityState.Added;
                            }
                        }

                        List<int> lstOrdemServicoExameId = new List<int>();
                        foreach (var item in ordemServicoCompleto.OrdemServicoExame)
                        {
                            bool remove = true;
                            foreach (var i in ordem.OrdemServicoExame)
                            {
                                if (item.OrdemServicoExameId == i.OrdemServicoExameId)
                                    remove = false;
                            }

                            if (remove)
                                lstOrdemServicoExameId.Add(item.OrdemServicoExameId);
                        }

                        foreach (var id in lstOrdemServicoExameId)
                        {
                            RemoveOrdemServicoExame(id);
                        }

                        if (ordem.OrdemServicoExame != null && ordem.OrdemServicoExame.Count > 0)
                        {
                            foreach (var item in ordem.OrdemServicoExame)
                            {
                                item.Preco = db.Exame.Find(item.ExameId).Preco;
                            }
                        }
                    }

                    db.Entry(ordemServicoCompleto).State = EntityState.Modified;
                    db.SaveChanges();


                    return RedirectToAction("Index");
                }
                
                return View(ordemServicoCompleto);
            }
            catch
            {
                return View(ordemServicoCompleto);
            }
        }

        // GET: OrdemServico/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                OrdemServico ordemServico = db.OrdemServico.Find(id);
                if (ordemServico == null)
                {
                    return HttpNotFound();
                }
                return View(ordemServico);
            }
            catch
            {
                return View(new OrdemServico());
            }
        }

        // POST: OrdemServico/Delete/5
        [HttpPost]
        public ActionResult Delete(OrdemServicoViewModel ordem)
        {
            try
            {
                OrdemServico ordemServico = db.OrdemServico.Find(ordem.OrdemServicoId);
                db.OrdemServico.Remove(ordemServico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new OrdemServico());
            }
        }

        public void RemoveOrdemServicoExame(int id)
        {
            var ordemServicoExame = db.OrdemServicoExame.Find(id);
            if (ordemServicoExame == null) return;
            db.OrdemServicoExame.Remove(ordemServicoExame);
        }
    }
}
