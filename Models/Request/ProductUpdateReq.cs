namespace productstockingv1.Models.Request;

public class ProductUpdateReq
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
}

