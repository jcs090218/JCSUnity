/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to search for a location by address using Bing Maps Location API.\n
/// https://msdn.microsoft.com/en-us/library/ff701715.aspx
/// </summary>
public class OnlineMapsBingMapsLocation: OnlineMapsGoogleAPIQuery
{
    /// <summary>
    /// Gets the type of query to API.
    /// </summary>
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.bingMapsLocation; }
    }

    private OnlineMapsBingMapsLocation(string query, string key, int maxResults, bool includeNeighborhood)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://dev.virtualearth.net/REST/v1/Locations/{0}?key={1}&o=xml";
        if (includeNeighborhood) url += "&inclnb=1";
        if (maxResults > 0 && maxResults != 5) url += "&maxRes=" + maxResults;
        url = string.Format(url, query.Replace(" ", "+"), key);
        www = OnlineMapsUtils.GetWWW(url);
        OnlineMaps.instance.AddGoogleAPIQuery(this);
    }

    private OnlineMapsBingMapsLocation(Vector2 point, string key, bool includeNeighborhood)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://dev.virtualearth.net/REST/v1/Locations/{0}?key={1}&o=xml";
        if (includeNeighborhood) url += "&inclnb=1";
        url = string.Format(url, point.y + "," + point.x, key);
        www = OnlineMapsUtils.GetWWW(url);
        OnlineMaps.instance.AddGoogleAPIQuery(this);
    }

    /// <summary>
    /// Get latitude and longitude coordinates that correspond to location information provided as a query string.
    /// </summary>
    /// <param name="query">A string that contains information about a location, such as an address or landmark name.</param>
    /// <param name="key">Bing Maps API Key</param>
    /// <param name="maxResults">Specifies the maximum number (1-20) of locations to return in the response.</param>
    /// <param name="includeNeighborhood">Specifies to include the neighborhood with the address information the response when it is available. </param>
    /// <returns>Instance of query</returns>
    public static OnlineMapsGoogleAPIQuery FindByQuery(string query, string key, int maxResults = 5, bool includeNeighborhood = false)
    {
        return new OnlineMapsBingMapsLocation(query, key, maxResults, includeNeighborhood);
    }

    /// <summary>
    /// Get the location information associated with latitude and longitude coordinates (reverse geocoding). 
    /// </summary>
    /// <param name="point">The coordinates of the location that you want to reverse geocode (X - Longitude, Y - Latitude).</param>
    /// <param name="key">Bing Maps API Key.</param>
    /// <param name="includeNeighborhood">Specifies to include the neighborhood in the response when it is available. </param>
    /// <returns>Instance of query</returns>
    public static OnlineMapsGoogleAPIQuery FindByPoint(Vector2 point, string key, bool includeNeighborhood = false)
    {
        return new OnlineMapsBingMapsLocation(point, key, includeNeighborhood);
    }

    /// <summary>
    /// Converts response into an array of results.
    /// </summary>
    /// <param name="response">Response of query.</param>
    /// <returns>Array of result.</returns>
    public static OnlineMapsBingMapsLocationResult[] GetResults(string response)
    {
        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(response.Substring(1));
            OnlineMapsXMLNamespaceManager nsmgr = xml.GetNamespaceManager("x");
            string status = xml.Find<string>("//x:StatusDescription", nsmgr);

            if (status != "OK") return null;

            List<OnlineMapsBingMapsLocationResult> results = new List<OnlineMapsBingMapsLocationResult>();
            OnlineMapsXMLList resNodes = xml.FindAll("//x:Location", nsmgr);

            foreach (OnlineMapsXML node in resNodes)
            {
                results.Add(new OnlineMapsBingMapsLocationResult(node));
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
/// Result of Bing Maps Location API query.
/// </summary>
public class OnlineMapsBingMapsLocationResult
{
    /// <summary>
    /// Location name
    /// </summary>
    public string name;

    /// <summary>
    /// Coordinates of location (X - Longitude, Y - Latitude).
    /// </summary>
    public Vector2 location;

    /// <summary>
    /// Latitude
    /// </summary>
    public double latitude;

    /// <summary>
    /// Longitude
    /// </summary>
    public double longitude;

    /// <summary>
    /// Bounding box of location
    /// </summary>
    public Rect boundingBox;

    /// <summary>
    /// Entity type
    /// </summary>
    public string entityType;

    /// <summary>
    /// Dictonary of address parts.
    /// </summary>
    public Dictionary<string, string> address;

    /// <summary>
    /// Formatted address.
    /// </summary>
    public string formattedAddress;

    /// <summary>
    /// Confidence
    /// </summary>
    public string confidence;

    /// <summary>
    /// Match code
    /// </summary>
    public string matchCode;

    /// <summary>
    /// XML Node
    /// </summary>
    public OnlineMapsXML node;

    /// <summary>
    /// Constuctor
    /// </summary>
    /// <param name="node">Node of result</param>
    public OnlineMapsBingMapsLocationResult(OnlineMapsXML node)
    {
        this.node = node;
        address = new Dictionary<string, string>();
        foreach (OnlineMapsXML n in node)
        {
            if (n.name == "Name") name = n.Value();
            else if (n.name == "Point")
            {
                latitude = n.Get<double>("Latitude");
                longitude = n.Get<double>("Longitude");
                location = new Vector2((float)longitude, (float)latitude);
            }
            else if (n.name == "BoundingBox")
            {
                double slat = n.Get<double>("SouthLatitude");
                double wlng = n.Get<double>("WestLongitude");
                double nlat = n.Get<double>("NorthLatitude");
                double elng = n.Get<double>("EastLongitude");

                boundingBox = new Rect((float)wlng, (float)nlat, (float)(wlng - elng), (float)(nlat - slat));
            }
            else if (n.name == "EntityType") entityType = n.Value();
            else if (n.name == "Address")
            {
                foreach (OnlineMapsXML an in n)
                {
                    if (an.name == "FormattedAddress") formattedAddress = an.Value();
                    else address.Add(an.name, an.Value());
                }
            }
            else if (n.name == "Confidence") confidence = n.Value();
            else if (n.name == "MatchCode") matchCode = n.Value();
        }
    }
}