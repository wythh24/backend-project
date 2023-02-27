namespace productstockingv1.Models.Request;

public class ListStockCreateReq
{
    public IEnumerable<string>? ProductId { get; set; }
}

public class ListStockUpdateReq
{
    public IEnumerable<StockUpdateReq> command { get; set; }
}

public class ListStockTransferReq
{
    public IEnumerable<StockTransferReq> commands { get; set; }
}

public class ListCodeProduct
{
    public IEnumerable<string>? productcodes { get; set; }
}