using System;
using System.Linq;
using System.Web.Mvc;
using CabinetMedical.Context;

namespace CabinetMedical.Controllers
{
    [Authorize] // On protège aussi l'accueil
    public class HomeController : Controller
    {
        private CabinetContext db = new CabinetContext();

        public ActionResult Index()
        {
            // 1. Compter les Patients
            ViewBag.NombrePatients = db.Patients.Count();

            // 2. Compter les RDV d'aujourd'hui
            ViewBag.RdvAujourdhui = db.RendezVous.Count(r => r.DateHeure.Year == DateTime.Now.Year
                                                           && r.DateHeure.Month == DateTime.Now.Month
                                                           && r.DateHeure.Day == DateTime.Now.Day);

            // 3. --- C'EST ICI QUE TU DOIS AJOUTER LES COMPTEURS MANQUANTS ---
            ViewBag.NombreMedecins = db.Utilisateurs.Count(u => u.Role == "Medecin");
            ViewBag.NombreSecretaires = db.Utilisateurs.Count(u => u.Role == "Secretaire");

            // 4. Charger les prochains RDV
            var prochainsRdv = db.RendezVous
                                 .Include("Patient")
                                 .Include("Medecin")
                                 .Where(r => r.DateHeure >= DateTime.Now)
                                 .OrderBy(r => r.DateHeure)
                                 .Take(5)
                                 .ToList();

            return View(prochainsRdv);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Description de votre application.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Page de contact.";
            return View();
        }
    }
}