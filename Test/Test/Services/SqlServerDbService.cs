using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Test.Models;

namespace Test.Services
{
    public class SqlServerDbService : IPrescriptionDbService
    {
        private const string connecionString = "Data Source=db-mssql.pjwstk.edu.pl;" +
                                               "Initial Catalog=s18478;" +
                                               "Integrated Security=True";
        
        public List<Prescription> GetPrescriptions(string lastName)
        {
            List<Prescription> prescriptions = new List<Prescription>();
            
            using (var conn = new SqlConnection(connecionString))
            using (var comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                
                if (lastName == null)
                {
                    comm.CommandText = "SELECT * FROM Prescription ORDER BY Date DESC;";
                }
                else
                {
                    comm.CommandText = "SELECT * FROM Prescription " +
                                       "INNER JOIN Patient ON Prescription.IdPatient = Patient.IdPatient " +
                                       "WHERE LastName = @lastName";
                    comm.Parameters.AddWithValue("lastName", lastName);
                }

                var reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    var prescription = new Prescription();
                    prescription.IdPrescription = (int) reader["IdPrescription"];
                    prescription.Date = (DateTime) reader["Date"];
                    prescription.DueDate = (DateTime) reader["DueDate"];
                    prescription.IdPatient = (int) reader["IdPatient"];
                    prescription.IdDoctor = (int) reader["IdDoctor"];
                    prescriptions.Add(prescription);
                }
            }

            return prescriptions;
        }
    }
}