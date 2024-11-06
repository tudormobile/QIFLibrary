using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary
{
    /// <summary>
    /// OFX Security List message.
    /// </summary>
    public class OFXSecurityList : OFXMessage
    {
        /// <summary>
        /// Create and initialize a new instance.
        /// </summary>
        /// <param name="securities">Securitites in this list.</param>
        public OFXSecurityList(SecurityList securities)
        {
            Name = "SECLIST";
            foreach (var security in securities.Items)
            {
                this.Properties.Add(new OFXSecurity(security));
            }
        }
    }
}
