namespace Tudormobile.QIFLibrary;

/// <summary>
/// Provides interface for building an object.
/// </summary>
/// <typeparam name="T">Type of object to build.</typeparam>
public interface IBuilder<T>
{
    /// <summary>
    /// Build an object.
    /// </summary>
    /// <returns>An objetc of generic type <typeparamref name="T"/></returns>
    public T Build();
}