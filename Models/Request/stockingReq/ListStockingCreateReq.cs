namespace productstockingv1.Models.Request;

public class ListStockingCreateReq
{
    public IEnumerable<StockingCreateReq> commands { get; set; }
}