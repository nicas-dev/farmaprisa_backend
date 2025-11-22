namespace FarmaPrisa.Models.Dtos.Currency
{
    public class UpdateCurrencyDto
    {
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
