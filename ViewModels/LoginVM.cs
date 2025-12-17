using System.ComponentModel.DataAnnotations;

namespace CabinetMedical.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress]
        [Display(Name = "Adresse Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string MotDePasse { get; set; }
    }
}