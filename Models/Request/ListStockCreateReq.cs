namespace productstockingv1.Models.Request;

public class ListStockCreateReq
{
    public IEnumerable<string>? ProductId { get; set; }
}

public class ListStockUpdateReq
{
    public IEnumerable<StockUpdateReq> command { get; set; }
}