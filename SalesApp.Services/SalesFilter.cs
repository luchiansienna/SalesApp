namespace SalesApp.Services;

public class SalesFilter
{
    public int? PageIndex { get; set; } = null;
    public int? PageSize { get; set; } = null;
    public string? Segment { get; set; } = null;
    public string? Country { get; set; } = null;
    public string? Product { get; set; } = null;
    public string? DiscountBand { get; set; } = null;
}
