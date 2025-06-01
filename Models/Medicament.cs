namespace CW_9_S262535.Models;

public class Medicament
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new List<PrescriptionMedicament>();
}