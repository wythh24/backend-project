namespace productstockingv1.Models.Request;

public class ListWareCreateReq
{
    public IEnumerable<WareCreateReq> command { get; set; }
}