using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinetMedical.Models
{
    [Table("Prescriptions")]
    public class Prescription
    {
        [Key]
        public int IdPrescription { get; set; }

        [Required]
        [Display(Name = "Médicament")]
        public string Medicament { get; set; }

        [Required]
        public string Posologie { get; set; } // Ex: 2 fois par jour

        public string Duree { get; set; } // Ex: 1 semaine

        [ForeignKey("Consultation")]
        public int IdConsultation { get; set; }

        public virtual Consultation Consultation { get; set; }
    }
}