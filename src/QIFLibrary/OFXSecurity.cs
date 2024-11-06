using Tudormobile.QIFLibrary.Converters;
using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary
{
    /// <summary>
    /// OFX Security.
    /// </summary>
    public class OFXSecurity : OFXProperty
    {
        private readonly Security _security;

        /// <inheritdoc/>
        public override OFXPropertyCollection Children => generateProperties();

        /// <summary>
        /// Creates and initializes a new instance.
        /// </summary>
        /// <param name="security">Security.</param>
        public OFXSecurity(Security security) : base($"{security.SecurityType}INFO")
        {
            _security = security;
        }

        private OFXPropertyCollection generateProperties()
            => [SecurityConverter.ToProperty(_security)];

    }
}
