namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Employee
	{
	    public int Id { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Required]
	    public string Name { get; set; }

        [Range(15, 80)]
        [Required]
	    public int Age { get; set; }

	    public int PositionId { get; set; }
	    public Position Position { get; set; }

	    public ICollection<Order> Orders { get; set; } = new List<Order>();
	}
}