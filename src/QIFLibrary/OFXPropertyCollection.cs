namespace Tudormobile.QIFLibrary;

/// <summary>
/// Collection of properties.
/// </summary>
public class OFXPropertyCollection : List<OFXProperty>
{
    /// <summary>
    /// Locate a child property by name.
    /// </summary>
    /// <param name="key">Name of the child to locate.</param>
    /// <returns>Child property if found; otherwise an empty property witht the name is returned.</returns>
    public OFXProperty this[string key]
    {
        get
        {
            var p = this.FirstOrDefault(x => x.Name == key);
            return p ?? new OFXProperty(key);
        }
    }
}
