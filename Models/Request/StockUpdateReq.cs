namespace productstockingv1.Models.Request;

public class StockUpdateReq
{
    public string Id { get; set; }
    public int? Quantity { get; set; }
}

public class StockTransferReq
{
    public string productId { get; set; }
    public int? quantity { get; set; }
    public string sourceWareId { get; set; }
    public string targetWareId { get; set; }
}