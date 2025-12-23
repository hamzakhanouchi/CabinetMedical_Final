-- 1. ON FORCE LA BONNE BASE DE DONNÉES
USE CabinetMedicalDB;
GO

-- 2. VIDER LES TABLES (Nettoyage propre - Ordre important pour les clés étrangères)
DELETE FROM Prescriptions;
DELETE FROM Consultations;
DELETE FROM RendezVous;
DELETE FROM Patients;
DELETE FROM Utilisateurs;

-- 3. RÉINITIALISER LES COMPTEURS D'ID (Repartir de 1)
DBCC CHECKIDENT ('Prescriptions', RESEED, 0);
DBCC CHECKIDENT ('Consultations', RESEED, 0);
DBCC CHECKIDENT ('RendezVous', RESEED, 0);
DBCC CHECKIDENT ('Patients', RESEED, 0);
DBCC CHECKIDENT ('Utilisateurs', RESEED, 0);

-- 4. AJOUTER L'ADMINISTRATEUR (ID = 1)
INSERT INTO Utilisateurs (Nom, Prenom, Email, MotDePasse, Role, Specialite, DateCreation)
VALUES ('Admin', 'System', 'admin@cabinet.com', 'admin123', 'Admin', NULL, GETDATE());

-- 5. AJOUTER 5 MÉDECINS (ID = 2 à 6)
INSERT INTO Utilisateurs (Nom, Prenom, Email, MotDePasse, Role, Specialite, DateCreation) VALUES
('Tazi', 'Karim', 'dr.tazi@cabinet.com', 'pass123', 'Medecin', 'Cardiologie', GETDATE()),      -- ID 2
('Berrada', 'Zineb', 'dr.berrada@cabinet.com', 'pass123', 'Medecin', 'Pédiatrie', GETDATE()),     -- ID 3
('Benjelloun', 'Omar', 'dr.benjelloun@cabinet.com', 'pass123', 'Medecin', 'Généraliste', GETDATE()), -- ID 4
('El Idrissi', 'Sara', 'dr.elidrissi@cabinet.com', 'pass123', 'Medecin', 'Dermatologie', GETDATE()),-- ID 5
('Chraibi', 'Youssef', 'dr.chraibi@cabinet.com', 'pass123', 'Medecin', 'Chirurgie', GETDATE());   -- ID 6

-- Ajout d'une secrétaire (ID = 7) - Utile pour tester la prise de RDV
INSERT INTO Utilisateurs (Nom, Prenom, Email, MotDePasse, Role, Specialite, DateCreation)
VALUES ('Mansouri', 'Laila', 'secr@cabinet.com', 'pass123', 'Secretaire', NULL, GETDATE());

-- 6. AJOUTER 15 PATIENTS (Maroc) (ID = 1 à 15)
INSERT INTO Patients (Nom, Prenom, DateNaissance, Sexe, Adresse, Telephone, Email, GroupeSanguin, Allergies, MaladiesChroniques) VALUES
('Alaoui', 'Mohammed', '1985-04-12', 'M', '12 Bd Zerktouni, Casablanca', '0661111111', 'mohammed.a@email.com', 'A+', 'Pollen', NULL),
('El Amrani', 'Fatima', '1990-06-23', 'F', '45 Av. Fal Ould Oumeir, Rabat', '0661111112', 'fatima.e@email.com', 'O-', NULL, 'Asthme'),
('Bouzidi', 'Amine', '1978-01-15', 'M', '8 Rue des FAR, Fès', '0661111113', 'amine.b@email.com', 'B+', 'Pénicilline', NULL),
('Benkirane', 'Ghita', '2001-11-30', 'F', 'Bd Mohamed V, Tanger', '0661111114', 'ghita.b@email.com', 'A-', NULL, NULL),
('Ouazzani', 'Youssef', '1965-08-19', 'M', 'Gueliz, Marrakech', '0661111115', 'youssef.o@email.com', 'AB+', NULL, 'Diabète'),
('Tahiri', 'Salma', '1995-02-10', 'F', 'Agdal, Rabat', '0661111116', 'salma.t@email.com', 'O+', NULL, NULL),
('Jettou', 'Mehdi', '1988-07-07', 'M', 'Maarif, Casablanca', '0661111117', 'mehdi.j@email.com', 'B-', 'Arachides', NULL),
('Akalai', 'Meryem', '1950-12-05', 'F', 'Hay Riad, Rabat', '0661111118', 'meryem.a@email.com', 'A+', NULL, 'Hypertension'),
('Mrabet', 'Rachid', '1999-03-22', 'M', 'Talborjt, Agadir', '0661111119', 'rachid.m@email.com', 'O+', NULL, NULL),
('Filali', 'Houda', '1982-09-14', 'F', 'Ville Nouvelle, Meknès', '0661111120', 'houda.f@email.com', 'AB-', NULL, NULL),
('Skalli', 'Noura', '1992-05-05', 'F', 'Ain Diab, Casablanca', '0661111121', 'noura.s@email.com', 'A+', 'Gluten', NULL),
('Hammani', 'Bilal', '2005-01-20', 'M', 'Tabriquet, Salé', '0661111122', 'bilal.h@email.com', 'B+', NULL, NULL),
('Belkhayat', 'Samia', '1975-10-30', 'F', 'Martil, Tetouan', '0661111123', 'samia.b@email.com', 'O-', NULL, 'Migraine'),
('Ziani', 'Tarik', '1989-04-04', 'M', 'Lazaret, Oujda', '0661111124', 'tarik.z@email.com', 'A-', NULL, NULL),
('Kadiri', 'Leila', '1996-06-18', 'F', 'Bir Rami, Kenitra', '0661111125', 'leila.k@email.com', 'AB+', NULL, NULL);

-- 7. AJOUTER RENDEZ-VOUS (Adaptés aux 5 Médecins IDs 2 à 6)
-- Note: Les IdMedecin doivent être entre 2 et 6.
INSERT INTO RendezVous (DateHeure, Motif, IdPatient, IdMedecin) VALUES
(DATEADD(day, 1, GETDATE()), 'Consultation Cardiologie', 1, 2), -- Dr Tazi
(DATEADD(day, 2, GETDATE()), 'Suivi Asthme', 2, 4),           -- Dr Benjelloun
(DATEADD(day, 0, GETDATE()), 'Urgence Fièvre', 3, 3),         -- Dr Berrada (Pédiatre)
(DATEADD(day, 3, GETDATE()), 'Certificat Médical', 4, 4),     -- Dr Benjelloun
(DATEADD(day, -1, GETDATE()), 'Grippe Saisonnière', 5, 2),    -- Dr Tazi (Passé)
(DATEADD(day, -5, GETDATE()), 'Contrôle Dermatologique', 6, 5),-- Dr El Idrissi
(DATEADD(day, 5, GETDATE()), 'Consultation Chirurgie', 7, 6), -- Dr Chraibi
(DATEADD(day, 1, GETDATE()), 'Bilan Général', 8, 4),          -- Dr Benjelloun
(DATEADD(day, 4, GETDATE()), 'Eczéma', 9, 5),                 -- Dr El Idrissi
(DATEADD(day, 2, GETDATE()), 'Bilan sanguin', 10, 2),         -- Dr Tazi
(DATEADD(day, 6, GETDATE()), 'Suivi Post-op', 11, 6),         -- Dr Chraibi
(DATEADD(day, -2, GETDATE()), 'Renouvellement Ordonnance', 12, 4),
(DATEADD(day, 7, GETDATE()), 'Vaccin Enfant', 13, 3),         -- Dr Berrada
(DATEADD(day, 0, GETDATE()), 'Douleur Thoracique', 14, 2),    -- Dr Tazi (Aujourd'hui)
(DATEADD(day, 8, GETDATE()), 'Allergie', 15, 5),              -- Dr El Idrissi
(DATEADD(day, 10, GETDATE()), 'Fatigue chronique', 1, 4),
(DATEADD(day, 3, GETDATE()), 'Check-up', 2, 6);