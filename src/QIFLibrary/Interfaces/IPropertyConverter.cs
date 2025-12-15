namespace Tudormobile.QIFLibrary
{
    /// <summary>
    /// Provides mechanism for converting OFX properies to entities.
    /// </summary>
    /// <typeparam name="T">Entity type for conversion.</typeparam>
    public interface IPropertyConverter<T>
    {
        /// <summary>
        /// Converts an OFX property to an entity type.
        /// </summary>
        /// <param name="root">Root property to convert.</param>
        /// <returns>Converted entity if conversion is possible; otherwise (null).</returns>
        /// <remarks>
        /// This method will search through the property children for an appropriate conversion candidate.
        /// </remarks>
        T? Convert(OFXProperty root);
    }

    /// <inheritdoc/>
    public abstract class PropertyConverterBase<T> : IPropertyConverter<T>
    {
        /// <inheritdoc/>
        public abstract T? Convert(OFXProperty root);

        /// <summary>
        /// Search through property and children for matching key.
        /// </summary>
        /// <param name="root">Root of the search.</param>
        /// <param name="key">Key to match.</param>
        /// <returns>Matching property if found; otherwise (null).</returns>
        protected OFXProperty? DigForProperty(OFXProperty root, string key)
        {
            OFXProperty? result = null;
            if (root.Name == key) return root;
            foreach (var p in root.Children)
            {
                result = DigForProperty(p, key);
                if (result != null) return result;
            }
            return result;
        }

    }
}
