/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to search OSM data by name and address and to generate synthetic addresses of OSM points (reverse geocoding).\n
/// http://wiki.openstreetmap.org/wiki/Nominatim
///  </summary>
public class OnlineMapsOSMNominatim:OnlineMapsGoogleAPIQuery
{
    /// <summary>
    /// Gets the type of query to third-party API.
    /// </summary>
    /// <value>
    /// OnlineMapsQueryType.nominatim
    /// </value>
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.nominatim; }
    }

    private OnlineMapsOSMNominatim(string query, string acceptlanguage, int limit, bool addressdetails)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://nominatim.openstreetmap.org/search?format=xml&q={0}";
        if (addressdetails) url += "&addressdetails=1";
        if (limit > 0) url += "&limit=" + limit;
        if (string.IsNullOrEmpty(acceptlanguage)) url += "&accept-language=" + acceptlanguage;
        url = string.Format(url, query.Replace(" ", "+"));
        www = OnlineMapsUtils.GetWWW(url);
        OnlineMaps.instance.AddGoogleAPIQuery(this);
    }

    private OnlineMapsOSMNominatim(Vector2 location, string acceptlanguage, bool addressdetails)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://nominatim.openstreetmap.org/reverse?format=xml&lat={0}&lon={1}";
        if (addressdetails) url += "&addressdetails=1";
        if (string.IsNullOrEmpty(acceptlanguage)) url += "&accept-language=" + acceptlanguage;
        url = string.Format(url, location.y, location.x);
        www = OnlineMapsUtils.GetWWW(url);
        OnlineMaps.instance.AddGoogleAPIQuery(this);
    }

    /// <summary>
    /// Nominatim indexes named (or numbered) features with the OSM data set and a subset of other unnamed features (pubs, hotels, churches, etc).
    /// </summary>
    /// <param name="query">Query string to search for.</param>
    /// <param name="acceptlanguage">
    /// Preferred language order for showing search results, overrides the value specified in the "Accept-Language" HTTP header.\n
    /// Either uses standard rfc2616 accept-language string or a simple comma separated list of language codes.
    /// </param>
    /// <param name="limit">Limit the number of returned results.</param>
    /// <param name="addressdetails">Include a breakdown of the address into elements.</param>
    /// <returns>Instance of query</returns>
    public static OnlineMapsGoogleAPIQuery Search(string query, string acceptlanguage = "en", int limit = 0, bool addressdetails = true)
    {
        return new OnlineMapsOSMNominatim(query, acceptlanguage, limit, addressdetails);
    }

    /// <summary>
    /// Reverse geocoding generates an address from a latitude and longitude. 
    /// </summary>
    /// <param name="location">The location to generate an address for.</param>
    /// <param name="acceptlanguage">
    /// Preferred language order for showing search results, overrides the value specified in the "Accept-Language" HTTP header.\n
    /// Either uses standard rfc2616 accept-language string or a simple comma separated list of language codes.
    /// </param>
    /// <param name="addressdetails">Include a breakdown of the address into elements.</param>
    /// <returns>Instance of query</returns>
    public static OnlineMapsGoogleAPIQuery Reverse(Vector2 location, string acceptlanguage = "en", bool addressdetails = true)
    {
        return new OnlineMapsOSMNominatim(location, acceptlanguage, addressdetails);
    }

    /// <summary>
    /// Converts response into an array of results.
    /// </summary>
    /// <param name="response">Response of query.</param>
    /// <returns>Array of result.</returns>
    public static OnlineMapsOSMNominatimResult[] GetResults(string response)
    {
        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(response);
            bool isReverse = xml.name == "reversegeocode";

            OnlineMapsXMLList resNodes = xml.FindAll(isReverse? "//result" : "//place");
            if (resNodes.count == 0) return null;

            List<OnlineMapsOSMNominatimResult> results = new List<OnlineMapsOSMNominatimResult>();
            foreach (OnlineMapsXML node in resNodes)
            {
                OnlineMapsOSMNominatimResult result = new OnlineMapsOSMNominatimResult(node, isReverse);

                OnlineMapsXML adNode = isReverse ? xml["addressparts"] : node;
                if (!adNode.isNull) result.LoadAddressDetails(adNode);
                results.Add(result);
            }
            return results.ToArray();
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message + "\n" + exception.StackTrace);
        }

        return null;
    }
}

/// <summary>
/// Result of Nominatim query.
/// </summary>
public class OnlineMapsOSMNominatimResult
{
    /// <summary>
    /// XML node
    /// </summary>
    public OnlineMapsXML node;

    /// <summary>
    /// Place ID
    /// </summary>
    public int place_id;

    /// <summary>
    /// OSM Type
    /// </summary>
    public string osm_type;

    /// <summary>
    /// OSM ID
    /// </summary>
    public int osm_id;

    /// <summary>
    /// Place rank
    /// </summary>
    public int place_rank;

    /// <summary>
    /// Bounding box
    /// </summary>
    public Rect boundingbox;

    /// <summary>
    /// Latitude
    /// </summary>
    public double latitude;

    /// <summary>
    /// Longitude
    /// </summary>
    public double longitude;

    /// <summary>
    /// Сoordinates of location (X - longitude, Y - latitude).
    /// </summary>
    public Vector2 location;

    /// <summary>
    /// Display name
    /// </summary>
    public string display_name;

    /// <summary>
    /// Type of object
    /// </summary>
    public string type;

    /// <summary>
    /// Importance
    /// </summary>
    public double importance;

    /// <summary>
    /// Dictionary of address details
    /// </summary>
    public Dictionary<string, string> addressdetails;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="node">XML Node</param>
    /// <param name="isReverse">Indicates reverse geocoding result.</param>
    public OnlineMapsOSMNominatimResult(OnlineMapsXML node, bool isReverse)
    {
        this.node = node;

        place_id = node.A<int>("place_id");
        osm_type = node.A("osm_type");
        osm_id = node.A<int>("osm_id");
        place_rank = node.A<int>("place_rank");
        latitude = node.A<double>("lat");
        longitude = node.A<double>("lon");
        location = new Vector2((float)longitude, (float)latitude);
        display_name = isReverse? node.Value(): node.A("display_name");
        type = node.A("type");
        importance = node.A<double>("importance");

        string bb = node.A("boundingbox");
        if (!string.IsNullOrEmpty(bb))
        {
            string[] bbParts = bb.Split(',');
            double w = Double.Parse(bbParts[0]);
            double e = Double.Parse(bbParts[1]);
            double s = Double.Parse(bbParts[2]);
            double n = Double.Parse(bbParts[3]);
            boundingbox = new Rect((float)w, (float)n, (float)(e - w), (float)(s - n));
        }
        
        addressdetails = new Dictionary<string, string>();
    }

    /// <summary>
    /// Load address details.
    /// </summary>
    /// <param name="adNode">Address details XML node.</param>
    public void LoadAddressDetails(OnlineMapsXML adNode)
    {
        foreach (OnlineMapsXML n in adNode)
        {
            if (!n.isNull) addressdetails.Add(n.name, n.Value());
        }
    }
}