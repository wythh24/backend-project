using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace productstockingv1.models;

public class StockResponse
{
    public string Id { get; set; }
    public Product Product { get; set; }
    public Ware Ware { get; set; }
    public string ProductId { get; set; }
    public string WareId { get; set; }
    public  int? Quantity { get; set; }
    public DateTime? DocumentDate { get; set; }
    public DateTime? PostingDate { get; set; }
}