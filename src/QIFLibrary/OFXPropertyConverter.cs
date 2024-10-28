using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;
using Tudormobile.QIFLibrary.Interfaces;

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
    public T? Convert<T>(OFXProperty root) where T : class
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
    public T? Convert<T>(OFXProperty root, IPropertyConverter<T> converter)
        => converter.Convert(root);

}
