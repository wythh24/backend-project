namespace productstockingv1.models;

public class Stocking
{
    public string id { get; set; }
    public DateTimeOffset documentDate { get; set; }
    public DateTimeOffset postingDate { get; set; }
}