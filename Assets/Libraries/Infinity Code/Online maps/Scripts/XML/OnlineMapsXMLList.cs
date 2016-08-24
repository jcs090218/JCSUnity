/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.Collections;
using System.Xml;

/// <summary>
/// Wrapper for XmlNodeList.
/// </summary>
public class OnlineMapsXMLList : IEnumerable
{
    private readonly XmlNodeList _list;

    /// <summary>
    /// Count of the elements.
    /// </summary>
    public int count
    {
        get { return (_list != null) ? _list.Count: 0; }
    }

    /// <summary>
    /// Reference to XmlNodeList.
    /// </summary>
    public XmlNodeList list
    {
        get { return _list; }
    }

    /// <summary>
    /// Create empty list.
    /// </summary>
    public OnlineMapsXMLList()
    {
        
    }

    /// <summary>
    /// Create wrapper for XmlNodeList.
    /// </summary>
    /// <param name="list">XmlNodeList.</param>
    public OnlineMapsXMLList(XmlNodeList list)
    {
        _list = list;
    }

    /// <summary>
    /// Get the element by index.
    /// </summary>
    /// <param name="index">Index of element.</param>
    /// <returns>Element.</returns>
    public OnlineMapsXML this[int index]
    {
        get
        {
            if (_list == null || index < 0 || index >= _list.Count) return new OnlineMapsXML();
            return new OnlineMapsXML(_list[index] as XmlElement);
        }
    }

    public IEnumerator GetEnumerator()
    {
        return new OnlineMapsXMLListEnum(this);
    }
}