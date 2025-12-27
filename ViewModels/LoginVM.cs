using System.ComponentModel.DataAnnotations;

namespace CabinetMedical.ViewModels  // <--- On met le bon dossier ici !
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "L'email est requis.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
    }
}