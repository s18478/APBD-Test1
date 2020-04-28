
using System.Collections.Generic;
using Test.Models;

namespace Test.Services
{
    public interface IPrescriptionDbService
    {
        public List<Prescription> GetPrescriptions(string lastName);
    }
}