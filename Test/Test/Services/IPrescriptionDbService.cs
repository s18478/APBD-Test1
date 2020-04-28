
using System.Collections.Generic;
using Test.Models;

namespace Test.Services
{
    public interface IPrescriptionDbService
    {
        public List<Prescription> GetPrescriptions(string lastName);
        public bool ExistsPrescription(int idPrescription);
        public List<Medicament> AddMedicaments(List<Medicament> medicaments, int idPrescription);
    }
}