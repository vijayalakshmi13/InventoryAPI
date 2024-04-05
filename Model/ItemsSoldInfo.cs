using System.ComponentModel.DataAnnotations;

namespace azure.Model
{
    public class ItemsSoldInfo
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTimeSold { get; set; }
        public int QuantitySold { get; set; }
        public decimal Cost { get; set; }
        public int CostCurrencyId { get; set; }
        public int PaidCurrencyId { get; set; }
        public int Quantity { get; set; }
        public int WasDeleted { get; set; }
        public int SoldById { get; set; }



    }
}
