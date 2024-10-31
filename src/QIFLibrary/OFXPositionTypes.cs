using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.QIFLibrary;

/// <summary>
/// Types of positions.
/// </summary>
public enum OFXPositionTypes
{
    /// <summary>
    /// Unknown or unitialized type.
    /// </summary>
    UNKNOWN = 0,

    /// <summary>
    /// Long.
    /// </summary>
    LONG,

    /// <summary>
    /// Short.
    /// </summary>
    SHORT
}
