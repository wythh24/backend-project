namespace productstockingv1.Models.Request;

public class StockCreateRequest
{
    public string name { get; set; }
    public string quantity { get; set; }
    public DateTimeOffset? documentDate { get; set; }
    public DateTimeOffset? postionDate { get; set; }
}

public class Create
{
    public string ProductCode { get; set; }
    public string WareCode { get; set; }
    public int? Quantity { get; set; }
}
public class StockGetById
{
    public string Id { get; set; } = "";
}

public class ProductGetCode
{
    public string Code { get; set; } = "";
}