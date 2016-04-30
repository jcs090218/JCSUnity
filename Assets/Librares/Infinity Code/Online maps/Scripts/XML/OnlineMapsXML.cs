/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Wrapper for XML.
/// </summary>
public class OnlineMapsXML : IEnumerable
{
    private XmlDocument _document;
    private XmlElement _element;

    /// <summary>
    /// Name of the node.
    /// </summary>
    public string name
    {
        get { return (_element != null)? _element.Name: null; }
    }

    /// <summary>
    /// The number of child nodes.
    /// </summary>
    public int count
    {
        get { return (_element != null && _element.HasChildNodes) ? _element.ChildNodes.Count : 0; }
    }

    /// <summary>
    /// Checks whether the contents of the node.
    /// </summary>
    public bool isNull
    {
        get { return _document == null || _element == null; }
    }

    /// <summary>
    /// Reference to XmlDocument.
    /// </summary>
    public XmlDocument document
    {
        get { return _document; }
    }

    /// <summary>
    /// Reference to XmlElement.
    /// </summary>
    public XmlElement element
    {
        get { return _element; }
    }

    /// <summary>
    /// Content of node as string.
    /// </summary>
    public string outerXml
    {
        get
        {
            return (_element != null)? _element.OuterXml: null;
        }
    }

    /// <summary>
    /// Get the child element by index.
    /// </summary>
    /// <param name="index">Index of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML this[int index]
    {
        get
        {
            if (_element == null || !_element.HasChildNodes) return new OnlineMapsXML();
            if (index < 0 || index >= _element.ChildNodes.Count) return new OnlineMapsXML();
            return new OnlineMapsXML(_element.ChildNodes[index] as XmlElement);
        }
    }

    /// <summary>
    /// Get the child element by name.
    /// </summary>
    /// <param name="childName">Name of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML this[string childName]
    {
        get
        {
            if (_element == null || !_element.HasChildNodes) return new OnlineMapsXML();
            return new OnlineMapsXML(_element[childName]);
        }
    }

    /// <summary>
    /// Creates an empty element.
    /// </summary>
    public OnlineMapsXML()
    {
        
    }

    /// <summary>
    /// Creates a new element with the specified name.
    /// </summary>
    /// <param name="nodeName">Name of element.</param>
    public OnlineMapsXML(string nodeName)
    {
        try
        {
            _document = new XmlDocument();
            _element = _document.CreateElement(nodeName);
            _document.AppendChild(_element);
        }
        catch (Exception)
        {
            _document = null;
            _element = null;
        }
    }

    /// <summary>
    /// Creates a new element based on the XmlElement.
    /// </summary>
    /// <param name="xmlElement">XmlElement for which will create the wrapper.</param>
    public OnlineMapsXML(XmlElement xmlElement)
    {
        if (xmlElement == null) return;

        _element = xmlElement;
        _document = _element.OwnerDocument;
    }

    /// <summary>
    /// Get an attribute by name.
    /// </summary>
    /// <param name="attributeName">Name of attribute.</param>
    /// <returns>Value of attribute as string.</returns>
    public string A(string attributeName)
    {
        return A<string>(attributeName);
    }

    /// <summary>
    /// Get an attribute by name, and return as the specified type.
    /// </summary>
    /// <typeparam name="T">Type of attribute.</typeparam>
    /// <param name="attributeName">Name of attribute.</param>
    /// <returns>Value of attribute as specified type.</returns>
    public T A<T>(string attributeName)
    {
        if (_element == null || !_element.HasAttributes) return default(T);

        XmlAttribute el = _element.Attributes[attributeName];
        if (el == null) return default(T);

        string value = el.Value;
        if (string.IsNullOrEmpty(value)) return default(T);

        Type type = typeof(T);
        if (type == typeof(string)) return (T)Convert.ChangeType(value, type);

        T obj = default(T);
        PropertyInfo[] properties = type.GetProperties();
        Type underlyingType = type;

        if (properties.Length == 2 && string.Equals(properties[0].Name, "HasValue", StringComparison.InvariantCultureIgnoreCase))
            underlyingType = properties[1].PropertyType;

        try
        {
            MethodInfo method = underlyingType.GetMethod("Parse", new[] { typeof(string) });
            obj = (T)method.Invoke(null, new[] { value });
        }
        catch (Exception)
        {
            throw;
        }

        return obj;
    }

    /// <summary>
    /// Set an named attribute.
    /// </summary>
    /// <param name="attributeName">Name of attribute.</param>
    /// <param name="value">Value of attribute.</param>
    public void A(string attributeName, object value)
    {
        if (_element == null) return;
        _element.SetAttribute(attributeName, value.ToString());
    }

    /// <summary>
    /// Sets the color attribute as hex value.
    /// </summary>
    /// <param name="attributeName">Name of attribute.</param>
    /// <param name="value">Color</param>
    public void A(string attributeName, Color32 value)
    {
        A(attributeName, value.r.ToString("X2") + value.g.ToString("X2") + value.b.ToString("X2"));
    }

    /// <summary>
    /// Append a child element.
    /// </summary>
    /// <param name="newChild">Element.</param>
    public void AppendChild(XmlElement newChild)
    {
        if (_element == null) return;
        _element.AppendChild(newChild);
    }

    /// <summary>
    /// Append a child element.
    /// </summary>
    /// <param name="newChild">Element.</param>
    public void AppentChild(OnlineMapsXML newChild)
    {
        if (_element == null || newChild._element == null) return;
        _element.AppendChild(newChild._element);
    }

    /// <summary>
    /// Append a child elements.
    /// </summary>
    /// <param name="list">List of elements.</param>
    public void AppendChilds(IEnumerable<XmlNode> list)
    {
        if (_element == null) return;

        foreach (XmlNode node in list) _element.AppendChild(node);
    }

    /// <summary>
    /// Append a child elements.
    /// </summary>
    /// <param name="list">List of elements.</param>
    public void AppendChilds(IEnumerable<OnlineMapsXML> list)
    {
        if (_element == null) return;

        foreach (OnlineMapsXML node in list)
        {
            if (node._element != null) _element.AppendChild(node._element);
        }
    }

    /// <summary>
    /// Append a child elements.
    /// </summary>
    /// <param name="list">List of elements.</param>
    public void AppendChilds(XmlNodeList list)
    {
        if (_element == null) return;

        foreach (XmlNode node in list) _element.AppendChild(node);
    }

    /// <summary>
    /// Append a child elements.
    /// </summary>
    /// <param name="list">List of elements.</param>
    public void AppendChilds(OnlineMapsXMLList list)
    {
        if (_element == null) return;

        foreach (OnlineMapsXML node in list)
        {
            if (node._element != null) _element.AppendChild(node._element);
        }
    }

    /// <summary>
    /// Creates a child element with the specified name.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName)
    {
        if (_document == null || _element == null) return new OnlineMapsXML();

        XmlElement xmlElement = _document.CreateElement(nodeName);
        _element.AppendChild(xmlElement);
        return new OnlineMapsXML(xmlElement);
    }

    /// <summary>
    /// Creates a child element with the specified name and value.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <param name="value">Value of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName, bool value)
    {
        return Create(nodeName, value ? "True" : "False");
    }

    /// <summary>
    /// Creates a child element with the specified name and value.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <param name="value">Value of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName, Color32 value)
    {
        return Create(nodeName, value.r.ToString("X2") + value.g.ToString("X2") + value.b.ToString("X2"));
    }

    /// <summary>
    /// Creates a child element with the specified name and value.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <param name="value">Value of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName, float value)
    {
        return Create(nodeName, value.ToString());
    }

    /// <summary>
    /// Creates a child element with the specified name and value.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <param name="value">Value of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName, int value)
    {
        return Create(nodeName, value.ToString());
    }

    /// <summary>
    /// Creates a child element with the specified name and value.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <param name="value">Value of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName, OnlineMapsRange value)
    {
        OnlineMapsXML node = Create(nodeName);
        node.Create("Min", value.min);
        node.Create("Max", value.max);
        return node;
    }

    /// <summary>
    /// Creates a child element with the specified name and value.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <param name="value">Value of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName, Object value)
    {
        return Create(nodeName, (value != null) ? value.GetInstanceID() : 0);
    }

    /// <summary>
    /// Creates a child element with the specified name and value.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <param name="value">Value of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName, string value)
    {
        OnlineMapsXML node = Create(nodeName);
        node.SetChild(value);
        return node;
    }

    /// <summary>
    /// Creates a child element with the specified name and value.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <param name="value">Value of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName, Vector2 value)
    {
        OnlineMapsXML node = Create(nodeName);
        node.Create("X", value.x);
        node.Create("Y", value.y);
        return node;
    }

    /// <summary>
    /// Creates a child element with the specified name and value.
    /// </summary>
    /// <param name="nodeName">Name of child element.</param>
    /// <param name="value">Value of child element.</param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Create(string nodeName, Vector3 value)
    {
        OnlineMapsXML node = Create(nodeName);
        node.Create("X", value.x);
        node.Create("Y", value.y);
        node.Create("Z", value.z);
        return node;
    }

    /// <summary>
    /// Find a child at the specified XPath.
    /// </summary>
    /// <param name="xpath">XPath string.</param>
    /// <param name="nsmgr">An XmlNamespaceManager to use for resolving namespaces for prefixes in the XPath expression. </param>
    /// <returns>Child element.</returns>
    public OnlineMapsXML Find(string xpath, XmlNamespaceManager nsmgr = null)
    {
        if (_element == null || !_element.HasChildNodes) return new OnlineMapsXML();

        XmlElement xmlElement = _element.SelectSingleNode(xpath, nsmgr) as XmlElement;
        
        if (xmlElement != null) return new OnlineMapsXML(xmlElement);
        return new OnlineMapsXML();
    }

    /// <summary>
    /// Find a child at the specified XPath, and return value as the specified type.
    /// </summary>
    /// <typeparam name="T">Type of child element.</typeparam>
    /// <param name="xpath">XPath string.</param>
    /// <param name="nsmgr">An XmlNamespaceManager to use for resolving namespaces for prefixes in the XPath expression. </param>
    /// <returns>Value of child element as the specified type.</returns>
    public T Find<T>(string xpath, XmlNamespaceManager nsmgr = null)
    {
        if (_element == null || !_element.HasChildNodes) return default(T);
        return Get<T>(_element.SelectSingleNode(xpath, nsmgr) as XmlElement);
    }

    /// <summary>
    /// Finds all childs at the specified XPath.
    /// </summary>
    /// <param name="xpath">XPath string.</param>
    /// <param name="nsmgr">An XmlNamespaceManager to use for resolving namespaces for prefixes in the XPath expression. </param>
    /// <returns>List of the elements.</returns>
    public OnlineMapsXMLList FindAll(string xpath, XmlNamespaceManager nsmgr = null)
    {
        if (_element == null || !_element.HasChildNodes) return new OnlineMapsXMLList();
        return new OnlineMapsXMLList(_element.SelectNodes(xpath, nsmgr));
    }

    /// <summary>
    /// Get the value of element as string.
    /// </summary>
    /// <param name="childName">Name of child.</param>
    /// <returns>Value of element as string.</returns>
    public string Get(string childName)
    {
        return Get<string>(childName);
    }

    /// <summary>
    /// Get the value of element as the specified type.
    /// </summary>
    /// <typeparam name="T">Type of element</typeparam>
    /// <param name="el">Element</param>
    /// <returns>Value of element as the specified type.</returns>
    public T Get<T>(XmlElement el)
    {
        if (el == null) return default(T);
        
        string value = el.InnerXml;
        if (string.IsNullOrEmpty(value)) return default(T);

        Type type = typeof(T);
        if (type == typeof(string)) return (T)Convert.ChangeType(value, type);
        if (type == typeof (Vector2)) return (T)Convert.ChangeType(new Vector2(Get<float>(el["X"]), Get<float>(el["Y"])), type);
        if (type == typeof (Vector3)) return (T)Convert.ChangeType(new Vector3(Get<float>(el["X"]), Get<float>(el["Y"]), Get<float>(el["Z"])), type);
        if (type == typeof(Color) || type == typeof(Color32)) return (T)Convert.ChangeType(OnlineMapsUtils.HexToColor(value), type);
        if (type == typeof(OnlineMapsRange)) return (T)Convert.ChangeType(new OnlineMapsRange(Get<int>("Min"), Get<int>("Max")), type);

        T obj = default(T);
        PropertyInfo[] properties = type.GetProperties();
        Type underlyingType = type;

        if (properties.Length == 2 && string.Equals(properties[0].Name, "HasValue", StringComparison.InvariantCultureIgnoreCase))
            underlyingType = properties[1].PropertyType;

        try
        {
            MethodInfo method = underlyingType.GetMethod("Parse", new[] { typeof(string) });
            obj = (T)method.Invoke(null, new[] { value });
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message + "\n" + exception.StackTrace);
            throw;
        }

        return obj;
    }

    /// <summary>
    /// Get the value of element as the specified type or default value if the child is not found.
    /// </summary>
    /// <typeparam name="T">Type of element</typeparam>
    /// <param name="el">Element</param>
    /// <param name="defaultValue">Default value</param>
    /// <returns>Value of element as the specified type or default value.</returns>
    public T Get<T>(XmlElement el, T defaultValue)
    {
        if (el == null) return defaultValue;

        string value = el.InnerXml;
        if (string.IsNullOrEmpty(value)) return defaultValue;

        Type type = typeof(T);
        if (type == typeof(string)) return (T)Convert.ChangeType(value, type);
        if (type == typeof(Vector2)) return (T)Convert.ChangeType(new Vector2(Get<float>(el["X"]), Get<float>(el["Y"])), type);
        if (type == typeof(Vector3)) return (T)Convert.ChangeType(new Vector3(Get<float>(el["X"]), Get<float>(el["Y"]), Get<float>(el["Z"])), type);
        if (type == typeof(Color) || type == typeof(Color32)) return (T)Convert.ChangeType(OnlineMapsUtils.HexToColor(value), type);
        if (type == typeof(OnlineMapsRange)) return (T)Convert.ChangeType(new OnlineMapsRange(Get<int>("Min"), Get<int>("Max")), type);

        T obj = defaultValue;
        PropertyInfo[] properties = type.GetProperties();
        Type underlyingType = type;

        if (properties.Length == 2 && string.Equals(properties[0].Name, "HasValue", StringComparison.InvariantCultureIgnoreCase))
            underlyingType = properties[1].PropertyType;

        try
        {
            MethodInfo method = underlyingType.GetMethod("Parse", new[] { typeof(string) });
            obj = (T)method.Invoke(null, new[] { value });
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message + "\n" + exception.StackTrace);
            throw;
        }

        return obj;
    }

    /// <summary>
    /// Get the value of child element as the specified type.
    /// </summary>
    /// <typeparam name="T">Type of child element.</typeparam>
    /// <param name="childName">Name of child.</param>
    /// <returns>Value of element as the specified type.</returns>
    public T Get<T>(string childName)
    {
        if (_element == null || !_element.HasChildNodes) return default(T);
        return Get<T>(_element[childName]);
    }

    /// <summary>
    /// Get the value of child element as the specified type or default value if the child is not found.
    /// </summary>
    /// <typeparam name="T">Type of child element.</typeparam>
    /// <param name="childName">Name of child.</param>
    /// <param name="defaultValue">Default value.</param>
    /// <returns>Value of element as the specified type or default value.</returns>
    public T Get<T>(string childName, T defaultValue)
    {
        if (_element == null || !_element.HasChildNodes) return defaultValue;
        return Get(_element[childName], defaultValue);
    }

    public IEnumerator GetEnumerator()
    {
        return new OnlineMapsXMLEnum(this);
    }

    /// <summary>
    /// Get NamespaceManager for current xml node.
    /// </summary>
    /// <param name="prefix">Namespace prefix.</param>
    /// <returns>NamespaceManager</returns>
    public OnlineMapsXMLNamespaceManager GetNamespaceManager(string prefix = null)
    {
        OnlineMapsXMLNamespaceManager nsmgr = new OnlineMapsXMLNamespaceManager(document.NameTable);
        if (prefix == null) prefix = element.GetPrefixOfNamespace(element.NamespaceURI);
        nsmgr.AddNamespace(prefix, element.NamespaceURI);
        return nsmgr;
    }

    /// <summary>
    /// Loads the XML from a string.
    /// </summary>
    /// <param name="xmlString">XML string.</param>
    /// <returns>First element.</returns>
    public static OnlineMapsXML Load(string xmlString)
    {
        try
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlString);
            return new OnlineMapsXML(document.DocumentElement);
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
            return new OnlineMapsXML();
        }
    }

    /// <summary>
    /// Removes this element from the XML.
    /// </summary>
    public void Remove()
    {
        if (_element == null) return;
        _element.ParentNode.RemoveChild(_element);
    }

    /// <summary>
    /// Removes child element from the XML.
    /// </summary>
    /// <param name="childName">Name of child element.</param>
    public void Remove(string childName)
    {
        if (_element == null || !_element.HasChildNodes) return;
        _element.RemoveChild(_element[childName]);
    }

    /// <summary>
    /// Removes child element from the XML.
    /// </summary>
    /// <param name="childIndex">Index of child element.</param>
    public void Remove(int childIndex)
    {
        if (_element == null || !_element.HasChildNodes) return;
        if (childIndex < 0 || childIndex >= _element.ChildNodes.Count) return;
        _element.RemoveChild(_element.ChildNodes[childIndex]);
    }

    /// <summary>
    /// Sets the value of the element.
    /// </summary>
    /// <param name="value">Value of element.</param>
    private void SetChild(string value)
    {
        if (_element == null || _document == null) return;
        _element.AppendChild(_document.CreateTextNode(value));
    }

    /// <summary>
    /// Gets the value of the element as string.
    /// </summary>
    /// <returns>Value of the element as string.</returns>
    public string Value()
    {
        return Value<string>();
    }

    /// <summary>
    /// Gets the value of the element as the specified type.
    /// </summary>
    /// <typeparam name="T">Type of element.</typeparam>
    /// <returns>Value as the specified type.</returns>
    public T Value<T>()
    {
        return Get<T>(_element);
    }
}