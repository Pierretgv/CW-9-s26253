namespace CW_9_S262535.DTOs;

public class MedicamentWithDetailsDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = null!;
    public int Dose { get; set; }
    public string Description { get; set; } = null!;
}