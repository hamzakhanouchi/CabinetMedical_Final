using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinetMedical.Models
{
    [Table("Consultations")]
    public class Consultation
    {
        [Key]
        public int IdConsultation { get; set; }

        [Display(Name = "Date Consultation")]
        [DataType(DataType.Date)]
        public DateTime DateConsultation { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Diagnostic { get; set; }

        [DataType(DataType.MultilineText)]
        public string Traitement { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observations { get; set; }

        // Clés étrangères
        [ForeignKey("Patient")]
        public int IdPatient { get; set; }

        [ForeignKey("Medecin")]
        public int IdMedecin { get; set; }

        // Navigation
        public virtual Patient Patient { get; set; }
        public virtual Utilisateur Medecin { get; set; }

        // Liste des médicaments prescrits lors de cette consultation
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}