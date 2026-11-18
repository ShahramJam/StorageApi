namespace StorageApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Price { get; set; }
        public required string Category { get; set; }
        public required string Shelf { get; set; }
        public int Count { get; set; }
        public string? Description { get; set; }

    }
}
