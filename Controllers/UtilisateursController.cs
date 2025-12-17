using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CabinetMedical.Context;
using CabinetMedical.Models;

namespace CabinetMedical.Controllers
{
    [Authorize]
    public class UtilisateursController : Controller
    {
        private CabinetContext db = new CabinetContext();

        // Vérification de sécurité : Est-ce un Admin ?
        private bool IsAdmin()
        {
            return Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin";
        }

        // GET: Utilisateurs
        public ActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View(db.Utilisateurs.ToList());
        }

        // GET: Utilisateurs/Create
        public ActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: Utilisateurs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUtilisateur,Nom,Prenom,Email,MotDePasse,Role,Specialite")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                utilisateur.DateCreation = System.DateTime.Now;
                db.Utilisateurs.Add(utilisateur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null) return HttpNotFound();
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUtilisateur,Nom,Prenom,Email,MotDePasse,Role,Specialite,DateCreation")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(utilisateur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            if (utilisateur == null) return HttpNotFound();
            return View(utilisateur);
        }

        // POST: Utilisateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Utilisateur utilisateur = db.Utilisateurs.Find(id);
            db.Utilisateurs.Remove(utilisateur);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}