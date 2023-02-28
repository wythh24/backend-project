namespace productstockingv1.Models.Request;

public class ProductCreateReq
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string Code { get; set; }
}

