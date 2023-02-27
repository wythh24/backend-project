namespace productstockingv1.Models.Request;

public class ListProductCreateReq
{
    public IEnumerable<ProductCreateReq> command { get; set; }
}

public class ListWareCreateReq
{
    public IEnumerable<WareCreateReq> command { get; set; }
}