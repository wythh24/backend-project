namespace productstockingv1.Models.Request;

public class StockCreateProdReq
{
    public string ProductCode { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string? ProductDescription { get; set; }
    public string WareCode { get; set; }
    public int Quantity { get; set; }
}