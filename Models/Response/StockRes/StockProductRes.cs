namespace productstockingv1.models.StockRes;

public class StockProductRes
{
    public string Id { get; set; }
    public string Code { get; set; }
    public ICollection<Stocking> Stockings { get; } = new List<Stocking>();
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}