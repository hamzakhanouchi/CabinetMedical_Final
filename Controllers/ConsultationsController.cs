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
    public class ConsultationsController : Controller
    {
        private CabinetContext db = new CabinetContext();

        // GET: Consultations (Affiche l'historique complet)
        public ActionResult Index(int? idPatient)
        {
            var consultations = db.Consultations.Include(c => c.Medecin).Include(c => c.Patient);

            // Si on demande l'historique d'un patient précis
            if (idPatient != null)
            {
                consultations = consultations.Where(c => c.IdPatient == idPatient);
                ViewBag.PatientId = idPatient; // Pour garder le lien
            }

            return View(consultations.OrderByDescending(c => c.DateConsultation).ToList());
        }

        // GET: Consultations/Create
        public ActionResult Create(int? idPatient)
        {
            // --- SECURITÉ : Si ce n'est pas un médecin, on refuse l'accès ---
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Medecin")
            {
                // On redirige vers la liste des patients si on n'est pas médecin
                return RedirectToAction("Index", "Patients");
            }
            // ---------------------------------------------------------------

            // Liste des médecins
            ViewBag.IdMedecin = new SelectList(db.Utilisateurs.Where(u => u.Role == "Medecin"), "IdUtilisateur", "NomComplet");

            // Si on vient de la page patient, on pré-sélectionne le patient
            if (idPatient != null)
            {
                ViewBag.IdPatient = new SelectList(db.Patients, "IdPatient", "Nom", idPatient);

                // On pré-remplit le modèle avec l'ID du patient pour fixer la liste déroulante
                var consult = new Consultation { IdPatient = idPatient.Value, DateConsultation = DateTime.Now };
                return View(consult);
            }

            // Sinon liste vide normale
            ViewBag.IdPatient = new SelectList(db.Patients, "IdPatient", "Nom");
            return View(new Consultation { DateConsultation = DateTime.Now });
        }
        // GET: Consultations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            Consultation consultation = db.Consultations.Find(id);
            if (consultation == null) return HttpNotFound();

            // Listes déroulantes pour modifier le patient ou le médecin si besoin
            ViewBag.IdPatient = new SelectList(db.Patients, "IdPatient", "Nom", consultation.IdPatient);
            ViewBag.IdMedecin = new SelectList(db.Utilisateurs.Where(u => u.Role == "Medecin"), "IdUtilisateur", "Nom", consultation.IdMedecin);
            return View(consultation);
        }
        // POST: Consultations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdConsultation,DateConsultation,Diagnostic,Traitement,Observations,IdPatient,IdMedecin")] Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPatient = new SelectList(db.Patients, "IdPatient", "Nom", consultation.IdPatient);
            ViewBag.IdMedecin = new SelectList(db.Utilisateurs.Where(u => u.Role == "Medecin"), "IdUtilisateur", "Nom", consultation.IdMedecin);
            return View(consultation);
        }

        // POST: Consultations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdConsultation,DateConsultation,Diagnostic,Traitement,Observations,IdPatient,IdMedecin")] Consultation consultation)
        {
            // --- SECURITÉ POST : On revérifie au cas où ---
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Medecin")
            {
                return RedirectToAction("Index", "Patients");
            }

            if (ModelState.IsValid)
            {
                db.Consultations.Add(consultation);
                db.SaveChanges();
                // Retourne à la liste des consultations de ce patient
                return RedirectToAction("Index", new { idPatient = consultation.IdPatient });
            }

            ViewBag.IdMedecin = new SelectList(db.Utilisateurs.Where(u => u.Role == "Medecin"), "IdUtilisateur", "NomComplet", consultation.IdMedecin);
            ViewBag.IdPatient = new SelectList(db.Patients, "IdPatient", "Nom", consultation.IdPatient);
            return View(consultation);
        }

        // GET: Consultations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Consultation consultation = db.Consultations.Find(id);
            if (consultation == null) return HttpNotFound();
            return View(consultation);
        }

        // Ajoute ici ta méthode Ordonnance si elle manquait dans ton copier-coller précédent
        public ActionResult Ordonnance(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Consultation consultation = db.Consultations
                                          .Include(c => c.Patient)
                                          .Include(c => c.Medecin)
                                          .Include(c => c.Prescriptions) // Important pour la nouvelle vue
                                          .FirstOrDefault(c => c.IdConsultation == id);

            if (consultation == null) return HttpNotFound();
            return View(consultation);
        }
    }
}