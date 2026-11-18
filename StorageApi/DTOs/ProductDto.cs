using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs
{
    public record ProductDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        public int InventoryValue => Price * Count;
    }
}
