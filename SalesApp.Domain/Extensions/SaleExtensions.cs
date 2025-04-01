namespace SalesApp.Domain.Extensions;
public static class SaleExtensions
{
    public static IEnumerable<Sale> TrimAllStringFields(this IEnumerable<Sale> sales)
    {
        foreach (var sale in sales)
        {
            sale.Segment = sale.Segment?.Trim() ?? string.Empty;
            sale.Country = sale.Country?.Trim() ?? string.Empty;
            sale.Product = sale.Product?.Trim() ?? string.Empty;
            sale.DiscountBand = sale.DiscountBand?.Trim() ?? string.Empty;
            yield return sale;
        }
    }
}

