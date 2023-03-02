namespace productstockingv1.Models.Request;

public class WareCreateReq
{
    public string name { get; set; }
    public string? description { get; set; }
    public string Code { get; set; }
}