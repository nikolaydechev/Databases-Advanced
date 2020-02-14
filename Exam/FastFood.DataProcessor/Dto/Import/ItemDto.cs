namespace FastFood.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class ItemDto
    {
        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
        public string Category { get; set; }
    }
}
