using Tudormobile.QIFLibrary.Entities;

namespace Tudormobile.QIFLibrary
{
    /// <summary>
    /// OFX Securiry List Response message.
    /// </summary>
    public class OFXSecurityListResponse : OFXMessageSet
    {
        /// <summary>
        /// Create and initialize a new instance.
        /// </summary>
        /// <param name="securities">Securitites in this list.</param>
        /// <param name="userId">User ID (optional)</param>
        /// <param name="status">Response status (optional)</param>
        /// <param name="cookie">Cookie (optional)</param>
        public OFXSecurityListResponse(SecurityList securities, string? userId = null, OFXStatus? status = null, string? cookie = null)
            : base(OFXMessageSetTypes.SECLIST, OFXMessageDirection.RESPONSE, version: 1)
        {
            if (userId != null || status != null || cookie != null)
            {
                var msg = new OFXMessage() { Name = "SECLISTTRNRS", };
                if (userId != null) msg.Id = userId;
                if (status != null) msg.Status = status;
                if (cookie != null) msg.Properties.Add(new OFXProperty("CLTCOOKIE", cookie)); // 
                this.Messages.Add(msg);
            }
            this.Messages.Add(new OFXSecurityList(securities));
        }
    }
}
