using System.ComponentModel.DataAnnotations;

namespace StorageApi.DTOs
{
    public record ProductStatsDto(
        [Range(0, int.MaxValue)]
        int TotalProducts,
        [Range(0, int.MaxValue)]
        int TotalInventoryValue,
        [Range(0, int.MaxValue)]
        double AveragePrice
    );
}
