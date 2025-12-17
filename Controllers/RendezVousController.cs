using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CabinetMedical.Context;
using CabinetMedical.Models;

namespace CabinetMedical.Controllers
{
    [Authorize]
    public class RendezVousController : Controller
    {
        private CabinetContext db = new CabinetContext();

        // GET: RendezVous
        public ActionResult Index()
        {
            // On inclut les données des Patients et Médecins pour afficher les noms au lieu des ID
            var rendezVous = db.RendezVous.Include(r => r.Medecin).Include(r => r.Patient);
            return View(rendezVous.ToList());
        }

        // GET: RendezVous/Create
        public ActionResult Create()
        {
            // On prépare les listes déroulantes (ViewBag)
            ViewBag.IdMedecin = new SelectList(db.Utilisateurs.Where(u => u.Role == "Medecin"), "IdUtilisateur", "NomComplet");
            ViewBag.IdPatient = new SelectList(db.Patients, "IdPatient", "Nom"); // Tu peux concaténer Nom+Prénom si tu veux
            return View();
        }

        // POST: RendezVous/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRendezVous,DateHeure,Motif,Statut,IdPatient,IdMedecin")] RendezVous rendezVous)
        {
            if (ModelState.IsValid)
            {
                db.RendezVous.Add(rendezVous);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Si erreur, on recharge les listes
            ViewBag.IdMedecin = new SelectList(db.Utilisateurs.Where(u => u.Role == "Medecin"), "IdUtilisateur", "NomComplet", rendezVous.IdMedecin);
            ViewBag.IdPatient = new SelectList(db.Patients, "IdPatient", "Nom", rendezVous.IdPatient);
            return View(rendezVous);
        }

        // GET: RendezVous/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RendezVous rendezVous = db.RendezVous.Find(id);
            if (rendezVous == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMedecin = new SelectList(db.Utilisateurs.Where(u => u.Role == "Medecin"), "IdUtilisateur", "NomComplet", rendezVous.IdMedecin);
            ViewBag.IdPatient = new SelectList(db.Patients, "IdPatient", "Nom", rendezVous.IdPatient);
            return View(rendezVous);
        }

        // POST: RendezVous/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRendezVous,DateHeure,Motif,Statut,IdPatient,IdMedecin")] RendezVous rendezVous)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rendezVous).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdMedecin = new SelectList(db.Utilisateurs.Where(u => u.Role == "Medecin"), "IdUtilisateur", "NomComplet", rendezVous.IdMedecin);
            ViewBag.IdPatient = new SelectList(db.Patients, "IdPatient", "Nom", rendezVous.IdPatient);
            return View(rendezVous);
        }

        // GET: RendezVous/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RendezVous rendezVous = db.RendezVous.Find(id);
            if (rendezVous == null)
            {
                return HttpNotFound();
            }
            return View(rendezVous);
        }

        // POST: RendezVous/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RendezVous rendezVous = db.RendezVous.Find(id);
            db.RendezVous.Remove(rendezVous);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}