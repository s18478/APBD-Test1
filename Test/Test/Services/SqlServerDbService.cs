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

        public bool ExistsPrescription(int idPrescription)
        {
            using (var conn = new SqlConnection(connecionString))
            using (var comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;

                comm.CommandText = "SELECT IdPrescription FROM Prescription WHERE IdPrescription = @id";
                comm.Parameters.AddWithValue("id", idPrescription);
                var reader = comm.ExecuteReader();

                if (!reader.Read())
                {
                    return false;
                }

                return true;
            }
        }

        public List<Medicament> AddMedicaments(List<Medicament> medicaments, int idPrescription)
        {
            using (var conn = new SqlConnection(connecionString))
            using (var comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                SqlTransaction transaction = conn.BeginTransaction();
                comm.Transaction = transaction;
                comm.Parameters.AddWithValue("idPrescription", idPrescription);
                
                try
                {
                    foreach (var medicament in medicaments)
                    {
                        comm.Parameters.Clear();
                        
                        comm.CommandText = "SELECT IdMedicament FROM Medicament WHERE IdMedicament = @id";
                        comm.Parameters.AddWithValue("id", medicament.IdMedicament);
                        var reader = comm.ExecuteReader();
                    
                        if (!reader.Read())
                        {
                            reader.Close();
                            transaction.Rollback();
                            throw new Exception("Medicament does not exist");
                        }
                        else
                        {
                            reader.Close();
                            comm.CommandText =
                                "INSERT INTO Prescription_Medicament VALUES (@id, @idPrescription, @dose, @details)";
                            comm.Parameters.AddWithValue("idPrescription", idPrescription);
                            comm.Parameters.AddWithValue("dose", medicament.Dose);
                            comm.Parameters.AddWithValue("details", medicament.Details);
                            var write = comm.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (SqlException exc)
                {
                    transaction.Rollback();
                    throw new Exception(exc.Message + exc.LineNumber);
                }
            }

            return medicaments;
        }
    }
}