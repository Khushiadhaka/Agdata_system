using System.ComponentModel.DataAnnotations;

public sealed class ProductCreateDto
{
	[Required]
	public string Name { get; set; } = string.Empty;

	[Range(1, int.MaxValue)]
	public int RequiredPoints { get; set; }

	[Range(0, int.MaxValue)]
	public int InitialStock { get; set; }

	public string? Description { get; set; }
	public string? SKU { get; set; }
}
