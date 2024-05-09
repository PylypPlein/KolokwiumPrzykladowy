using KolPrzykladowy.Dto;

namespace KolPrzykladowy.Repositories;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

public interface IPrescriptionRepository
{
    public IEnumerable<PrescriptionDto> GetPrescriptions(string doctorLastName);

    public bool CreatePrescription(CreatePrescriptionDto prescription);
}

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly IConfiguration _configuration;

    public PrescriptionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<PrescriptionDto> GetPrescriptions(string doctorLastName = null)
        {
            //var connectionString = _configuration.GetConnectionString("ConnectionStrings:DefaultConnection");
            var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            using (connection)
            {
                var query = "SELECT p.IdPrescription, p.Date, p.DueDate, pat.LastName AS PatientLastName, doc.LastName AS DoctorLastName " +
                            "FROM Prescription p " +
                            "INNER JOIN Patient pat ON p.IdPatient = pat.IdPatient " +
                            "INNER JOIN Doctor doc ON p.IdDoctor = doc.IdDoctor ";

                if (!string.IsNullOrEmpty(doctorLastName))
                {
                    query += "WHERE doc.LastName = @DoctorLastName ";
                }

                query += "ORDER BY p.Date DESC"; // Sortowanie malejące według daty

                var command = new SqlCommand(query, connection);

                if (!string.IsNullOrEmpty(doctorLastName))
                {
                    command.Parameters.AddWithValue("@DoctorLastName", doctorLastName);
                }

                connection.Open();
                var reader = command.ExecuteReader();

                var prescriptions = new List<PrescriptionDto>();
                while (reader.Read())
                {
                    var prescription = new PrescriptionDto
                    {
                        IdPrescription = Convert.ToInt32(reader["IdPrescription"]),
                        Date = Convert.ToDateTime(reader["Date"]),
                        DueDate = Convert.ToDateTime(reader["DueDate"]),
                        PatientLastName = reader["PatientLastName"].ToString(),
                        DoctorLastName = reader["DoctorLastName"].ToString()
                    };
                    prescriptions.Add(prescription);
                }
                reader.Close();

                return prescriptions;
            }
        }
    public bool CreatePrescription(CreatePrescriptionDto prescription) 
    {   
        var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using (connection)
        {
            connection.Open();

            using (SqlCommand command = new SqlCommand(
                       "INSERT INTO Prescription (Date, DueDate, IdPatient, IdDoctor) VALUES (@Date, @DueDate, @IdPatient, @IdDoctor)",
                       connection))
            {
                command.Parameters.AddWithValue("@Date", prescription.Date);
                command.Parameters.AddWithValue("@DueDate", prescription.DueDate);
                command.Parameters.AddWithValue("@IdPatient", prescription.IdPatient);
                command.Parameters.AddWithValue("@IdDoctor", prescription.IdDoctor);
                    
                // Pobierz nowo wygenerowany klucz główny
                var affectedRows = command.ExecuteNonQuery();
                return affectedRows == 1;
            }
        }
    }
}
