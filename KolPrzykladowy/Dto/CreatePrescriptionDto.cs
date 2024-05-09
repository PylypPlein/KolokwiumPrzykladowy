using System.ComponentModel.DataAnnotations;

namespace KolPrzykladowy.Dto;

public class CreatePrescriptionDto
{
    [Required]
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
}