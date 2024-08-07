using System;

namespace Validators.Models
{
    public class BirthNumberValidationDto
    {
        public string Input { get; set; } = null!;
        public bool IsValid { get; set; }
        public string? InvalidityReason { get; set; }
        public string? CanonicalBirthNumber { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Sex { get; set; }
    }
}