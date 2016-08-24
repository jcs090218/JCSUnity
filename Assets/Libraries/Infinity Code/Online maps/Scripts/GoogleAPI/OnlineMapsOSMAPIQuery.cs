/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class is used to request to Open Street Map Overpass API.\n
/// You can create a new instance using OnlineMapsOSMAPIQuery.Find.\n
/// Open Street Map Overpass API documentation: http://wiki.openstreetmap.org/wiki/Overpass_API/Language_Guide \n
/// You can test your queries using: http://overpass-turbo.eu/ \n
/// </summary>
[System.Serializable]
public class OnlineMapsOSMAPIQuery: OnlineMapsGoogleAPIQuery
{
    /// <summary>
    /// Gets the type of query to API.
    /// </summary>
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.osm; }
    }

    /// <summary>
    /// Constructor.
    /// Use OnlineMapsOSMAPIQuery.Find for create request.
    /// </summary>
    /// <param name="data">Overpass QL request</param>
    private OnlineMapsOSMAPIQuery(string data)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://overpass-api.de/api/interpreter?data=" + WWW.EscapeURL(data);
        www = OnlineMapsUtils.GetWWW(url);
    }

    /// <summary>
    /// Query the Open Street Map Overpass API.
    /// </summary>
    /// <param name="data">Overpass QL request</param>
    /// <returns>Instance of the query</returns>
    public static OnlineMapsOSMAPIQuery Find(string data)
    {
        OnlineMapsOSMAPIQuery query = new OnlineMapsOSMAPIQuery(data);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// Get data from the response Open Street Map Overpass API.
    /// </summary>
    /// <param name="response">Response from Overpass API</param>
    /// <param name="nodes">List of nodes</param>
    /// <param name="ways">List of ways</param>
    /// <param name="relations">List of relations</param>
    public static void ParseOSMResponse(string response, out List<OnlineMapsOSMNode> nodes, out List<OnlineMapsOSMWay> ways, out List<OnlineMapsOSMRelation> relations)
    {
        nodes = new List<OnlineMapsOSMNode>();
        ways = new List<OnlineMapsOSMWay>();
        relations = new List<OnlineMapsOSMRelation>();

        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(response);

            foreach (OnlineMapsXML node in xml)
            {
                if (node.name == "node") nodes.Add(new OnlineMapsOSMNode(node));
                else if (node.name == "way") ways.Add(new OnlineMapsOSMWay(node));
                else if (node.name == "relation") relations.Add(new OnlineMapsOSMRelation(node));
            }
        }
        catch {
            Debug.Log(response);
        }
    }

    /// <summary>
    /// Get data from the response Open Street Map Overpass API.
    /// </summary>
    /// <param name="response">Response from Overpass API</param>
    /// <param name="nodes">Dictionary of nodes</param>
    /// <param name="ways">List of ways</param>
    /// <param name="relations">List of relations</param>
    public static void ParseOSMResponse(string response, out Dictionary<string, OnlineMapsOSMNode> nodes, out List<OnlineMapsOSMWay> ways, out List<OnlineMapsOSMRelation> relations)
    {
        nodes = new Dictionary<string, OnlineMapsOSMNode>();
        ways = new List<OnlineMapsOSMWay>();
        relations = new List<OnlineMapsOSMRelation>();

        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(response);

            foreach (OnlineMapsXML node in xml)
            {
                if (node.name == "node")
                {
                    OnlineMapsOSMNode osmNode = new OnlineMapsOSMNode(node);
                    nodes.Add(osmNode.id, osmNode);
                }
                else if (node.name == "way") ways.Add(new OnlineMapsOSMWay(node));
                else if (node.name == "relation") relations.Add(new OnlineMapsOSMRelation(node));
            }
        }
        catch
        {
            Debug.Log(response);
        }
    }
}

/// <summary>
/// The base class of Open Streen Map element.
/// </summary>
public class OnlineMapsOSMBase
{
    /// <summary>
    /// Element ID
    /// </summary>
    public string id;

    /// <summary>
    /// Element tags
    /// </summary>
    public List<OnlineMapsOSMTag> tags;

    public bool Equals(OnlineMapsOSMBase other)
    {
        if (ReferenceEquals(other, null)) return false;
        if (ReferenceEquals(this, other)) return true;
        return id == other.id;
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }

    /// <summary>
    /// Get tag value for the key.
    /// </summary>
    /// <param name="key">Tag key</param>
    /// <returns>Tag value</returns>
    public string GetTagValue(string key)
    {
        List<OnlineMapsOSMTag> curTags = tags.Where(tag => tag.key == key).ToList();
        if (curTags.Count > 0) return curTags[0].value;
        return string.Empty;
    }

    /// <summary>
    /// Checks for the tag with the specified key and value.
    /// </summary>
    /// <param name="key">Tag key</param>
    /// <param name="value">Tag value</param>
    /// <returns>True - if successful, False - otherwise.</returns>
    public bool HasTag(string key, string value)
    {
        return tags.Any(t => t.key == key && t.value == value);
    }

    /// <summary>
    /// Checks for the tag with the specified keys.
    /// </summary>
    /// <param name="keys">Tag keys.</param>
    /// <returns>True - if successful, False - otherwise.</returns>
    public bool HasTagKey(params string[] keys)
    {
        return keys.Any(key => tags.Any(t => t.key == key));
    }

    /// <summary>
    /// Checks for the tag with the specified values.
    /// </summary>
    /// <param name="values">Tag values</param>
    /// <returns>True - if successful, False - otherwise.</returns>
    public bool HasTagValue(params string[] values)
    {
        return values.Any(val => tags.Any(t => t.value == val));
    }

    /// <summary>
    /// Checks for the tag with the specified key and values.
    /// </summary>
    /// <param name="key">Tag key</param>
    /// <param name="values">Tag values</param>
    /// <returns>True - if successful, False - otherwise.</returns>
    public bool HasTags(string key, params string[] values)
    {
        return tags.Any(tag => tag.key == key && values.Any(v => v == tag.value));
    }
}

/// <summary>
/// Open Street Map node element class
/// </summary>
public class OnlineMapsOSMNode : OnlineMapsOSMBase
{
    /// <summary>
    /// Latitude
    /// </summary>
    public readonly float lat;

    /// <summary>
    /// Longitude
    /// </summary>
    public readonly float lon;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node">Node</param>
    public OnlineMapsOSMNode(OnlineMapsXML node)
    {
        id = node.A("id");
        lat = node.A<float>("lat");
        lon = node.A<float>("lon");

        tags = new List<OnlineMapsOSMTag>();

        foreach (OnlineMapsXML subNode in node) tags.Add(new OnlineMapsOSMTag(subNode));
    }

    public static implicit operator Vector2(OnlineMapsOSMNode val)
    {
        return new Vector2(val.lon, val.lat);
    }
}

/// <summary>
/// Open Street Map way element class
/// </summary>
public class OnlineMapsOSMWay : OnlineMapsOSMBase
{
    /// <summary>
    /// List of node id;
    /// </summary>
    public readonly List<string> nodeRefs;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node">Node</param>
    public OnlineMapsOSMWay(OnlineMapsXML node)
    {
        id = node.A("id");
        nodeRefs = new List<string>();
        tags = new List<OnlineMapsOSMTag>();

        foreach (OnlineMapsXML subNode in node)
        {
            if (subNode.name == "nd") nodeRefs.Add(subNode.A("ref"));
            else if (subNode.name == "tag") tags.Add(new OnlineMapsOSMTag(subNode));
        }
    }

    /// <summary>
    /// Returns a list of nodes related to that way.
    /// </summary>
    /// <param name="nodes">General list of nodes</param>
    /// <returns>List of nodes related to that way</returns>
    public List<OnlineMapsOSMNode> GetNodes(List<OnlineMapsOSMNode> nodes)
    {
        List<OnlineMapsOSMNode> _nodes = new List<OnlineMapsOSMNode>();
        foreach (string nRef in nodeRefs)
        {
            OnlineMapsOSMNode node = nodes.FirstOrDefault(n => n.id == nRef);
            if (node != null) _nodes.Add(node);
        }
        return _nodes;
    }

    /// <summary>
    /// Returns a list of nodes related to that way.
    /// </summary>
    /// <param name="nodes">General dictionary of nodes</param>
    /// <returns>List of nodes related to that way</returns>
    public List<OnlineMapsOSMNode> GetNodes(Dictionary<string, OnlineMapsOSMNode> nodes)
    {
        List<OnlineMapsOSMNode> _nodes = new List<OnlineMapsOSMNode>();
        foreach (string nRef in nodeRefs)
        {
            if (nodes.ContainsKey(nRef)) _nodes.Add(nodes[nRef]);
        }
        return _nodes;
    }
}

/// <summary>
/// Open Street Map relation element class
/// </summary>
public class OnlineMapsOSMRelation : OnlineMapsOSMBase
{
    /// <summary>
    /// List members of relation
    /// </summary>
    public readonly List<OnlineMapsOSMRelationMember> members;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node">Node</param>
    public OnlineMapsOSMRelation(OnlineMapsXML node)
    {
        id = node.A("id");
        members = new List<OnlineMapsOSMRelationMember>();
        tags = new List<OnlineMapsOSMTag>();

        foreach (OnlineMapsXML subNode in node)
        {
            if (subNode.name == "member") members.Add(new OnlineMapsOSMRelationMember(subNode));
            else if (subNode.name == "tag") tags.Add(new OnlineMapsOSMTag(subNode));
        }
    }
}

/// <summary>
/// Open Street Map relation member class
/// </summary>
public class OnlineMapsOSMRelationMember
{
    /// <summary>
    /// ID of reference element
    /// </summary>
    public readonly string reference;

    /// <summary>
    /// Member role
    /// </summary>
    public readonly string role;

    /// <summary>
    /// Member type
    /// </summary>
    public readonly string type;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node">Node</param>
    public OnlineMapsOSMRelationMember(OnlineMapsXML node)
    {
        type = node.A("type");
        reference = node.A("ref");
        role = node.A("role");
    }
}

/// <summary>
/// Open Street Map element tag class
/// </summary>
public class OnlineMapsOSMTag
{
    /// <summary>
    /// Tag key
    /// </summary>
    public readonly string key;

    /// <summary>
    /// Tag value
    /// </summary>
    public readonly string value;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node">Node</param>
    public OnlineMapsOSMTag(OnlineMapsXML node)
    {
        key = node.A("k");
        value = node.A("v");
    }
}