namespace productstockingv1.Models.Request;

public class ListStockStringReq
{
    public IEnumerable<string>? ProductId { get; set; }
}