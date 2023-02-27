using productstockingv1.Interfaces;

namespace productstockingv1.models;

public class Ware : IKey<string>
{
    public string Id { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Name { get; set; }
    public string? Description { get; set; }

    public ICollection<Stocking> Stockings { get; } = new List<Stocking>();
}