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
            // 1. Les Chiffres
            ViewBag.NombrePatients = db.Patients.Count();

            // RDV d'aujourd'hui
            ViewBag.RdvAujourdhui = db.RendezVous.Count(r => r.DateHeure.Year == DateTime.Now.Year
                                                           && r.DateHeure.Month == DateTime.Now.Month
                                                           && r.DateHeure.Day == DateTime.Now.Day);

            // 2. Les 5 Prochains Rendez-vous (C'est ça le "Darouri")
            // On prend les RDV futurs, on les trie par date, et on prend les 5 premiers
            var prochainsRdv = db.RendezVous
                                 .Include("Patient")
                                 .Include("Medecin")
                                 .Where(r => r.DateHeure >= DateTime.Now)
                                 .OrderBy(r => r.DateHeure)
                                 .Take(5)
                                 .ToList();

            return View(prochainsRdv); // On envoie la liste à la vue
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