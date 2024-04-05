using System.ComponentModel.DataAnnotations;

namespace azure.Model
{
    public class InventoryItems
    {
        [Key]
        public int Id { get; set; }
        public string? SkuName { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public int CostCurrencyId { get; set; }
        public decimal ProfitPerItem { get; set; }  
        public int Quantity { get; set; }
        public int WasDeleted { get; set; }

    }
}
