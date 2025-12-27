using CabinetMedical.Context;
using CabinetMedical.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CabinetMedical.Controllers
{
    [Authorize] // On protège aussi l'accueil
    public class HomeController : Controller
    {
        private CabinetContext db = new CabinetContext();

        public ActionResult Index()
        {
            // 1. Récupérer le rôle et l'ID
            string role = Session["UserRole"] != null ? Session["UserRole"].ToString() : "";
            int userId = Session["UserId"] != null ? (int)Session["UserId"] : 0;
            DateTime aujourdhui = DateTime.Now;

            if (role == "Medecin")
            {
                // === LOGIQUE MÉDECIN ===
                // Ses patients
                ViewBag.NombrePatients = db.Patients.Count(p => p.RendezVous.Any(r => r.IdMedecin == userId)
                                                               || p.Consultations.Any(c => c.IdMedecin == userId));
                // Ses RDV du jour
                ViewBag.RdvAujourdhui = db.RendezVous.Count(r => r.IdMedecin == userId
                                                              && r.DateHeure.Year == aujourdhui.Year
                                                              && r.DateHeure.Month == aujourdhui.Month
                                                              && r.DateHeure.Day == aujourdhui.Day);
                // On masque le reste
                ViewBag.NombreMedecins = 0;
                ViewBag.NombreSecretaires = 0;
            }
            else
            {
                // === LOGIQUE GLOBALE ===
                ViewBag.NombrePatients = db.Patients.Count();
                ViewBag.RdvAujourdhui = db.RendezVous.Count(r => r.DateHeure.Year == aujourdhui.Year
                                                              && r.DateHeure.Month == aujourdhui.Month
                                                              && r.DateHeure.Day == aujourdhui.Day);
                ViewBag.NombreMedecins = db.Utilisateurs.Count(u => u.Role == "Medecin");
                ViewBag.NombreSecretaires = db.Utilisateurs.Count(u => u.Role == "Secretaire");
            }

            // 2. Charger les prochains RDV
            // CORRECTION ICI : On utilise explicitement IQueryable<RendezVous> au lieu de 'var'
            IQueryable<RendezVous> query = db.RendezVous.Include("Patient").Include("Medecin");

            if (role == "Medecin")
            {
                query = query.Where(r => r.IdMedecin == userId);
            }

            // On exécute la requête à la fin avec le tri et la limite
            var prochainsRdv = query.Where(r => r.DateHeure >= DateTime.Now)
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