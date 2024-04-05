using System.ComponentModel.DataAnnotations;

namespace azure.Model
{
    public class Currencies
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Abbrevation { get; set; }
        public string? Symbol { get; set; }
        public decimal? ConversionRateToUSD { get; set;}
    }
}
