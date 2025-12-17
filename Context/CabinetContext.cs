using System.Data.Entity;
using CabinetMedical.Models;

namespace CabinetMedical.Context
{
    public class CabinetContext : DbContext
    {
        // Le nom "CabinetCon" fait référence à la chaîne de connexion dans Web.config
        public CabinetContext() : base("name=CabinetCon")
        {
        }

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<RendezVous> RendezVous { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
    }
}