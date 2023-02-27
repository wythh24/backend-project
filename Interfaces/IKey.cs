namespace productstockingv1.Interfaces;

public interface IKey<T>
{
    T Id { get; set; }
}
public interface IKey1<T>
{
    T productid { get; set; }
}