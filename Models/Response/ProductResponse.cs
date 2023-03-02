namespace productstockingv1.models;

public class ProductResponse
{
    public string Id { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Description { get; set; }

}