namespace productstockingv1.models;

public class WareResponse
{
    public string Id { get; set; }
    public string Code { get; set; }
    public ICollection<Stocking> stocking { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
}