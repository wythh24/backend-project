using productstockingv1.Interfaces;

namespace productstockingv1.models;

public class Warse: IKey<String>
{
    public string Id { get; set; }
    public string code { get; set; }
    public string name { get; set; }
    public string? decscription { get; set; }
}