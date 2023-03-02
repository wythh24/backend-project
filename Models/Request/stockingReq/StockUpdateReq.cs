namespace productstockingv1.Models.Request;

public class StockUpdateReq
{
    public string Id { get; set; }
    public int? Quantity { get; set; }
}