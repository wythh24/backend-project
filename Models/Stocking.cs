using productstockingv1.Interfaces;

namespace productstockingv1.models;

public class Stocking : IKey<string>
{
    public string Id { get; set; } = null!;
    public string ProductId { get; set; } = null!;
    public string WareId { get; set; } = null!;
    public int? Quantity { get; set; }
    public DateTime? DocumentDate { get; set; }
    public DateTime? PostingDate { get; set; }

    public Product product { get; set; } = null!;
    public Ware Ware { get; set; } = null!;
}