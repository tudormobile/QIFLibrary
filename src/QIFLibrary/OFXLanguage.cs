namespace Tudormobile.QIFLibrary;

/// <summary>
/// OFX language codes.
/// </summary>
/// <remarks>
/// The accepted values are based on the ISO-639 three-letter language codes.
/// </remarks>
public enum OFXLanguage
{
    /// <summary>
    /// Unknown or unsupported language code.
    /// </summary>
    UNKNOWN = 0,
    /// <summary>
    /// English.
    /// </summary>
    ENG = 1,
    /// <summary>
    /// French.
    /// </summary>
    FRA,
    /// <summary>
    /// Spanish.
    /// </summary>
    SPA,
    /// <summary>
    /// Japanese.
    /// </summary>
    JPN
}
