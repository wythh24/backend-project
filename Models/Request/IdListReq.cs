namespace productstockingv1.Models.Request;

public class IdReq
{
    //change to can be null
    public ICollection<string?>? Id { get; set; }
}

public class GetT
{
    //change to can be null
    public string? Id { get; set; }
}