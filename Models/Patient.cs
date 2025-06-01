﻿namespace CW_9_S262535.Models;

public class Patient
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Email { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
