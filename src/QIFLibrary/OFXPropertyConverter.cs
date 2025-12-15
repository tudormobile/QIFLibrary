using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Converts OFX properties to known objects.
/// </summary>
public class OFXPropertyConverter
{
    /// <summary>
    /// Converts an OFX property to another type.
    /// </summary>
    /// <typeparam name="T">Converted type.</typeparam>
    /// <param name="root">Root property.</param>
    /// <returns>Converted type.</returns>
    /// <exception cref="NotSupportedException">Throws if property cannot be converted to the indicated type.</exception>
    public static T? Convert<T>(OFXProperty root) where T : class
    {
        var runtimeType = typeof(T);

        if (runtimeType == typeof(Institution)) return Convert(root, new InstitutionConverter()) as T;

        throw new NotSupportedException();
    }

    /// <summary>
    /// Converts an OFX property to another type.
    /// </summary>
    /// <typeparam name="T">Converted type.</typeparam>
    /// <param name="root">Root property.</param>
    /// <param name="converter">Converter to use.</param>
    /// <returns>Converted type.</returns>
    public static T? Convert<T>(OFXProperty root, IPropertyConverter<T> converter)
        => converter.Convert(root);

}
