using Microsoft.EntityFrameworkCore;
namespace CW_9_S262535.Services;
using Models;
using Exceptions;
using DTOs;
using Data;

public interface IDbService
{
    Task<ICollection<PatientGetDTO>> GetPatientsAsync();
    Task<PatientGetDTO> GetPatientByIdAsync(int idPatient);
    Task<PatientGetDTO> CreatePatientAsync(PatientCreateDTO patientData);
    Task UpdatePatientAsync(int idPatient, PatientUpdateDTO patientData);
    Task RemovePatientAsync(int idPatient);
}

public class DbService : IDbService
{
    private readonly PharmacyDbContext _context;

    public DbService(PharmacyDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<PatientGetDTO>> GetPatientsAsync()
    {
        return await _context.Patients
            .Select(p => new PatientGetDTO
            {
                IdPatient = p.IdPatient,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                Prescriptions = p.Prescriptions.Select(pr => new PrescriptionGetDTO
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    IdDoctor = pr.IdDoctor,
                    Doctor = new DoctorDTO
                    {
                        IdDoctor = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName,
                        LastName = pr.Doctor.LastName
                    }
                }).ToList() 
            })
            .ToListAsync<PatientGetDTO>(); 
    }
    public async Task<PrescriptionGetDTO> AddPrescriptionAsync(PrescriptionCreateDto dto)
    {
       
        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            IdDoctor = dto.IdDoctor,
            IdPatient = dto.IdPatient
        };

        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();

        
        return new PrescriptionGetDTO
        {
            IdPrescription = prescription.IdPrescription,
            Date = prescription.Date,
            DueDate = prescription.DueDate,
            IdDoctor = prescription.IdDoctor,
            Doctor = await _context.Doctors
                .Where(d => d.IdDoctor == prescription.IdDoctor)
                .Select(d => new DoctorDTO
                {
                    IdDoctor = d.IdDoctor,
                    FirstName = d.FirstName,
                    LastName = d.LastName
                })
                .FirstOrDefaultAsync()
        };
    }
    public async Task<PatientGetDTO> GetPatientByIdAsync(int idPatient)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == idPatient);

        if (patient == null)
            throw new NotFoundException($"Patient with id: {idPatient} not found");

        var patientDto = new PatientGetDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            Prescriptions = patient.Prescriptions.Select(pr => new PrescriptionGetDTO
            {
                IdPrescription = pr.IdPrescription,
                Date = pr.Date,
                DueDate = pr.DueDate,
                IdDoctor = pr.IdDoctor,
                Doctor = new DoctorDTO
                {
                    IdDoctor = pr.Doctor.IdDoctor,
                    FirstName = pr.Doctor.FirstName,
                    LastName = pr.Doctor.LastName
                }
            }).ToList()
        };

        return patientDto;
    }

    public async Task<PatientGetDTO> CreatePatientAsync(PatientCreateDTO patientData)
    {
        var patient = new Patient
        {
            FirstName = patientData.FirstName,
            LastName = patientData.LastName,
            Email = patientData.Email
        };

        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();

        return new PatientGetDTO
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            Prescriptions = new List<PrescriptionGetDTO>()
        };
    }

    public async Task UpdatePatientAsync(int idPatient, PatientUpdateDTO patientData)
    {
        var affected = await _context.Patients
            .Where(p => p.IdPatient == idPatient)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.FirstName, patientData.FirstName)
                .SetProperty(p => p.LastName, patientData.LastName)
                .SetProperty(p => p.Email, patientData.Email)
            );

        if (affected == 0)
            throw new NotFoundException($"Patient with id: {idPatient} not found");
    }

    public async Task RemovePatientAsync(int idPatient)
    {
        var affected = await _context.Patients
            .Where(p => p.IdPatient == idPatient)
            .ExecuteDeleteAsync();

        if (affected == 0)
            throw new NotFoundException($"Patient with id: {idPatient} not found");
    }
}
