namespace FastFood.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class EmployeeDto
    {
        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [Range(15, 80)]
        [Required]
        public int Age { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string Position { get; set; }
    }
}
