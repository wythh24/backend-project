namespace productstockingv1.Models.Request;

public class ListStockTransferQtyReq
{
    public IEnumerable<StockTransferReq> commands { get; set; } 
}