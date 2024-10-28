using Tudormobile.QIFLibrary.Interfaces;

namespace Tudormobile.QIFLibrary.Converters
{
    /// <summary>
    /// Provides mechanism for converting an OFX property to an Institution.
    /// </summary>
    public class InstitutionConverter : PropertyConverterBase<OFXInstitution>
    {
        /// <summary>
        /// Key property for this entity.
        /// </summary>
        public static string KEY = "FI";

        /// <inheritdoc/>
        public override OFXInstitution? Convert(OFXProperty root)
        {
            var p = digForProperty(root, KEY);
            return p == null ? null : new OFXInstitution()
            {
                Name = p.Children["ORG"].Value,
                Id = p.Children["FID"].Value
            };
        }
    }
}
