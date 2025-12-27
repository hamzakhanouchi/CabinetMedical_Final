using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CabinetMedical.Context;
using CabinetMedical.Models;
using CabinetMedical.ViewModels;

namespace CabinetMedical.Controllers
{
    public class AccountController : Controller
    {
        private CabinetContext db = new CabinetContext();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // On cherche l'utilisateur dans la BDD
                var user = db.Utilisateurs
                    .FirstOrDefault(u => u.Email == model.Email && u.MotDePasse == model.Password);

                if (user != null)
                {
                    // Utilisateur trouvé ! On crée le cookie d'authentification
                    FormsAuthentication.SetAuthCookie(user.Email, false);

                    // On stocke le rôle et le nom dans la Session pour les utiliser plus tard
                    Session["UserNom"] = user.NomComplet;
                    Session["UserRole"] = user.Role;
                    Session["UserId"] = user.IdUtilisateur;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email ou mot de passe incorrect.");
                }
            }
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}