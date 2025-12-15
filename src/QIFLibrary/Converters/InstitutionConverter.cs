using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary.Converters
{
    /// <summary>
    /// Provides mechanism for converting an OFX property to an Institution.
    /// </summary>
    public class InstitutionConverter : PropertyConverterBase<Institution>
    {
        /// <summary>
        /// Key property for this entity.
        /// </summary>
        public static readonly string KEY = "FI";

        /// <inheritdoc/>
        public override Institution? Convert(OFXProperty root)
        {
            var p = DigForProperty(root, KEY);
            return p == null ? null : new Institution()
            {
                Name = p.Children["ORG"].Value,
                Id = p.Children["FID"].Value
            };
        }
    }

    /// <inheritdoc/>
    public static partial class OFXPropertyConverterExtensions
    {
        /// <summary>
        /// Converts OFX property to institution.
        /// </summary>
        /// <param name="converter">Converter to extend.</param>
        /// <param name="root">Root property.</param>
        /// <returns>Intitusion if successful; otherwise (null).</returns>
        public static Institution? GetInstitution(this OFXPropertyConverter converter, OFXProperty root)
            => new InstitutionConverter().Convert(root);
    }

}
