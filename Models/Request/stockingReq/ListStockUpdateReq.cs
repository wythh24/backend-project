namespace productstockingv1.Models.Request;

public class ListStockUpdateReq
{
    public IEnumerable<StockUpdateReq> command { get; set; }
}