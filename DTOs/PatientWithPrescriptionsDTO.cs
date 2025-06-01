namespace CW_9_S262535.DTOs;

public class PatientWithPrescriptionsDTO : PatientDTO
{
    public List<PrescriptionGetDTO> Prescriptions { get; set; } = new();
}