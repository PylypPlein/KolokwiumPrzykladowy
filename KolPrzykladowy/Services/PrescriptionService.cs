using KolPrzykladowy.Dto;
using KolPrzykladowy.Repositories;
namespace KolPrzykladowy.Services;

using System;
using System.Collections.Generic;

public interface IPrescriptionService
{
    public IEnumerable<PrescriptionDto> GetPrescriptions(string doctorLastName);
    public bool AddPrescription(CreatePrescriptionDto createPrescriptionDto);
}
public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _repository;
    

    public PrescriptionService(IPrescriptionRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<PrescriptionDto> GetPrescriptions(string doctorLastName = null)
    {
        try
        {
            return _repository.GetPrescriptions(doctorLastName);
        }
        catch (Exception ex)
        {
            // Log exception or perform other error handling
            throw ex;
        }
    }

    public bool AddPrescription(CreatePrescriptionDto createPrescriptionDto)
    {
        return _repository.CreatePrescription(createPrescriptionDto);
    }
}
