using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinetMedical.Models
{
    [Table("Utilisateurs")]
    public class Utilisateur
    {
        [Key]
        public int IdUtilisateur { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(50)]
        public string Prenom { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Adresse Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string MotDePasse { get; set; }

        [Required]
        public string Role { get; set; } // Admin, Medecin, Secretaire

        public string Specialite { get; set; } // Nullable si pas médecin

        public DateTime DateCreation { get; set; }

        // Propriété calculée pour afficher le nom complet
        public string NomComplet
        {
            get { return Prenom + " " + Nom; }
        }
    }
}