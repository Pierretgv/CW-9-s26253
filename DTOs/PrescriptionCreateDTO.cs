namespace CW_9_S262535.DTOs;
public class PrescriptionCreateDto
{
    public List<MedicamentDTO> Medicaments { get; set; } = null!;
    public int IdDoctor { get; set; }
    public int IdPatient { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}
