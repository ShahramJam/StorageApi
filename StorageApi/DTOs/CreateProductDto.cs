using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs
{
    public record CreateProductDto
    {


        [Required]
        [MaxLength(150)]
        public string? Name { get; set; }
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Category { get; set; }
        [Required]
        [MaxLength(150)]
        public string? Shelf { get; set; }
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        [Required]
        public string? Description { get; set; }

    }
}
