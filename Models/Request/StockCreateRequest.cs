namespace productstockingv1.Models.Request;

public class StockCreateRequest
{
    public string name { get; set; }
    public string quantity { get; set; }
    public DateTimeOffset? documentDate { get; set; }
    public DateTimeOffset? postionDate { get; set; }
}