namespace productstockingv1.Interfaces;

public interface IKey<T>
{
    T Id { get; set; }
}