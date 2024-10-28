using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Financial institution.
/// </summary>
public class OFXInstitution
{
    /// <summary>
    /// Name of the institution.
    /// </summary>
    public String Name { get; set; } = String.Empty;

    /// <summary>
    /// Financial institution identifier.
    /// </summary>
    public String Id { get; set; } = String.Empty;
}
