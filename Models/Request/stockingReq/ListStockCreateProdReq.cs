namespace productstockingv1.Models.Request;

public class ListStockCreateProdReq
{
    public IEnumerable<StockCreateProdReq> commands { get; set; }
}