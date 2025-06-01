namespace CW_9_S262535.DTOs;

public class PatientGetDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    
    public ICollection<PrescriptionGetDTO> Prescriptions { get; set; } = new List<PrescriptionGetDTO>();
}
