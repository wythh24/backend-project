namespace productstockingv1.Models.Request;

public class StockingCreateReq

{
    public string ProductCode { get; set; }
    public string WareCode { get; set; }
    public int? Quantity { get; set; }
}