using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinetMedical.Models
{
    [Table("Patients")]
    public class Patient
    {
        [Key]
        public int IdPatient { get; set; }

        [Required]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required]
        [StringLength(50)]
        public string Prenom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date de Naissance")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateNaissance { get; set; }

        [Required]
        public string Sexe { get; set; }

        public string Adresse { get; set; }

        [Required]
        [Phone]
        public string Telephone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Groupe Sanguin")]
        public string GroupeSanguin { get; set; }

        [DataType(DataType.MultilineText)]
        public string Allergies { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Maladies Chroniques")]
        public string MaladiesChroniques { get; set; }

        // Relations pour navigation facile
        public virtual ICollection<RendezVous> RendezVous { get; set; }
        public virtual ICollection<Consultation> Consultations { get; set; }
    }
}