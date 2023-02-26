namespace productstockingv1.Models.Request;

public class ListProductUpdateReq
{
    public IEnumerable<ProductUpdateReq> command { get; set; }

}

public class ListWareUpdateReq
{
    public IEnumerable<WareUpdateReq> command { get; set; }
}