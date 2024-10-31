using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary.Entities;

/// <summary>
/// Financial institution.
/// </summary>
public class Institution
{
    /// <summary>
    /// Name of the institution.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Financial institution identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;
}
