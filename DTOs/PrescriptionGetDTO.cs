namespace CW_9_S262535.DTOs;

public class PrescriptionGetDTO
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public List<MedicamentWithDetailsDTO> Medicaments { get; set; } = new();
    public DoctorDTO Doctor { get; set; } = null!;
    public int IdDoctor { get; set; }
}