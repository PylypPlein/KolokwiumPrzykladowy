using KolPrzykladowy.Dto;
using KolPrzykladowy.Services;
namespace KolPrzykladowy.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _service;

    public PrescriptionController(IPrescriptionService service)
    {
        _service = service;
    }

    // GET: api/prescription
    // Example request: api/prescription?doctorLastName=Smith
    [HttpGet("prescription")]
    public IActionResult GetPrescriptions(string doctorLastName = null)
    {
        var prescriptions = _service.GetPrescriptions(doctorLastName);
        return Ok(prescriptions);
        
    }
    [HttpPost("prescriptionsPost")]
    public IActionResult CreatePrescription([FromBody] CreatePrescriptionDto prescription)
    {
        try
        {
            // Sprawdź, czy data "DueDate" jest starsza niż "Date"
            if (prescription.DueDate <= prescription.Date)
            {
                return BadRequest("Data 'DueDate' musi być późniejsza niż 'Date'.");
            }

            // Wstaw nowe dane na temat recepty
            //int newPrescriptionId = _service.AddPrescription(prescription);
                
            // Zwróć nowo utworzony obiekt Prescription wraz z kluczem głównym
            //prescription.IdPrescription = newPrescriptionId;
            var succes = _service.AddPrescription(prescription );
            return succes ? StatusCode(StatusCodes.Status201Created) : Conflict();

        }
        catch (Exception ex)
        {
            // Log exception or perform other error handling
            return StatusCode(500, "Wystąpił błąd podczas przetwarzania żądania");
        }
    }
}
