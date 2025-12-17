using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CabinetMedical.Context;
using CabinetMedical.Models;

namespace CabinetMedical.Controllers
{
    [Authorize]
    public class PrescriptionsController : Controller
    {
        private CabinetContext db = new CabinetContext();

        // GET: Prescriptions (Liste des médicaments pour une consultation donnée)
        public ActionResult Index(int? idConsultation)
        {
            var prescriptions = db.Prescriptions.Include(p => p.Consultation);

            if (idConsultation != null)
            {
                // Filtrer pour n'afficher que les médicaments de CETTE consultation
                prescriptions = prescriptions.Where(p => p.IdConsultation == idConsultation);
                ViewBag.IdConsultation = idConsultation;

                // Récupérer le nom du patient pour l'afficher
                var consultation = db.Consultations.Include(c => c.Patient).FirstOrDefault(c => c.IdConsultation == idConsultation);
                if (consultation != null)
                {
                    ViewBag.PatientNom = consultation.Patient.Nom + " " + consultation.Patient.Prenom;
                }
            }

            return View(prescriptions.ToList());
        }

        // GET: Prescriptions/Create
        public ActionResult Create(int? idConsultation)
        {
            if (idConsultation == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // On pré-remplit l'ID de la consultation, mais on le cache à l'utilisateur
            Prescription prescription = new Prescription();
            prescription.IdConsultation = idConsultation.Value;

            return View(prescription);
        }

        // POST: Prescriptions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPrescription,Medicament,Posologie,Duree,IdConsultation")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Prescriptions.Add(prescription);
                db.SaveChanges();
                // Après l'ajout, on reste sur la liste pour en ajouter un autre si besoin
                return RedirectToAction("Index", new { idConsultation = prescription.IdConsultation });
            }

            return View(prescription);
        }

        // GET: Prescriptions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Prescription prescription = db.Prescriptions.Find(id);
            if (prescription == null) return HttpNotFound();
            return View(prescription);
        }

        // POST: Prescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prescription prescription = db.Prescriptions.Find(id);
            int idConsultation = prescription.IdConsultation; // On garde l'ID pour le retour
            db.Prescriptions.Remove(prescription);
            db.SaveChanges();
            return RedirectToAction("Index", new { idConsultation = idConsultation });
        }
    }
}