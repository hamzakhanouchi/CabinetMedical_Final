using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CabinetMedical.Models
{
    [Table("RendezVous")]
    public class RendezVous
    {
        [Key]
        public int IdRendezVous { get; set; }

        [Required]
        [Display(Name = "Date et Heure")]
        [DataType(DataType.DateTime)]
        public DateTime DateHeure { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Motif { get; set; }

        public string Statut { get; set; } // Planifie, Confirme, Termine, Annule

        // Clés étrangères
        [ForeignKey("Patient")]
        [Display(Name = "Patient")]
        public int IdPatient { get; set; }

        [ForeignKey("Medecin")]
        [Display(Name = "Médecin")]
        public int IdMedecin { get; set; }

        // Objets de navigation (pour afficher les noms au lieu des ID dans les vues)
        public virtual Patient Patient { get; set; }
        public virtual Utilisateur Medecin { get; set; }
    }
}