namespace FastFood.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class EmployeeOrderDto
    {
        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
    }
}
