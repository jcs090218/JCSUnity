/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Elevation API provides elevation data for all locations on the surface of the earth, including depth locations on the ocean floor (which return negative values). \n
/// In those cases where Google does not possess exact elevation measurements at the precise location you request, the service will interpolate and return an averaged value using the four nearest locations.\n
/// With the Elevation API, you can develop hiking and biking applications, mobile positioning applications, or low resolution surveying applications. \n
/// https://developers.google.com/maps/documentation/elevation/
/// </summary>
public class OnlineMapsGetElevation:OnlineMapsGoogleAPIQuery
{
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.elevation; }
    }

    private OnlineMapsGetElevation(Vector2 location, string key, string client, string signature)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://maps.googleapis.com/maps/api/elevation/xml?sensor=false&locations=" + location.y + "," + location.x;
        if (!string.IsNullOrEmpty(key)) url += "&key=" + key;
        if (!string.IsNullOrEmpty(client)) url += "&client=" + client;
        if (!string.IsNullOrEmpty(signature)) url += "&signature=" + signature;
        www = OnlineMapsUtils.GetWWW(url);
    }

    private OnlineMapsGetElevation(Vector2[] locations, string key, string client, string signature)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://maps.googleapis.com/maps/api/elevation/xml?sensor=false&locations=";

        string[] locationsStr = new string[locations.Length];
        for (int i = 0; i < locations.Length; i++) locationsStr[i] = locations[i].y + "," + locations[i].x;

        url += string.Join("|", locationsStr);
        if (!string.IsNullOrEmpty(key)) url += "&key=" + key;
        if (!string.IsNullOrEmpty(client)) url += "&client=" + client;
        if (!string.IsNullOrEmpty(signature)) url += "&signature=" + signature;
        www = OnlineMapsUtils.GetWWW(url);
    }

    private OnlineMapsGetElevation(Vector2[] path, int samples, string key, string client, string signature)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://maps.googleapis.com/maps/api/elevation/xml?sensor=false&path={0}&samples={1}";

        string[] pathStr = new string[path.Length];
        for (int i = 0; i < path.Length; i++) pathStr[i] = path[i].y + "," + path[i].x;

        url = string.Format(url, string.Join("|", pathStr), samples);
        if (!string.IsNullOrEmpty(key)) url += "&key=" + key;
        if (!string.IsNullOrEmpty(client)) url += "&client=" + client;
        if (!string.IsNullOrEmpty(signature)) url += "&signature=" + signature;
        www = OnlineMapsUtils.GetWWW(url);
    }

    /// <summary>
    /// Get elevation value for single location.
    /// </summary>
    /// <param name="location">
    /// Location on the earth from which to return elevation data.
    /// </param>
    /// <param name="key">
    /// Your application's API key. \n
    /// This key identifies your application for purposes of quota management.
    /// </param>
    /// <param name="client">
    /// Client ID identifies you as a Maps API for Work customer and enables support and purchased quota for your application. \n
    /// Requests made without a client ID are not eligible for Maps API for Work benefits.
    /// </param>
    /// <param name="signature">
    /// Your welcome letter includes a cryptographic signing key, which you must use to generate unique signatures for your web service requests.
    /// </param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery Find(Vector2 location, string key = null, string client = null, string signature = null)
    {
        OnlineMapsGetElevation query = new OnlineMapsGetElevation(location, key, client, signature);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// Get elevation values for several locations.
    /// </summary>
    /// <param name="locations">
    /// Locations on the earth from which to return elevation data.
    /// </param>
    /// <param name="key">
    /// Your application's API key.\n
    /// This key identifies your application for purposes of quota management.
    /// </param>
    /// <param name="client">
    /// Client ID identifies you as a Maps API for Work customer and enables support and purchased quota for your application.\n
    /// Requests made without a client ID are not eligible for Maps API for Work benefits.
    /// </param>
    /// <param name="signature">
    /// Your welcome letter includes a cryptographic signing key, which you must use to generate unique signatures for your web service requests.
    /// </param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery Find(Vector2[] locations, string key = null, string client = null, string signature = null)
    {
        OnlineMapsGetElevation query = new OnlineMapsGetElevation(locations, key, client, signature);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// Get elevation values for path.
    /// </summary>
    /// <param name="path">Path on the earth for which to return elevation data. </param>
    /// <param name="samples">
    /// Specifies the number of sample points along a path for which to return elevation data. \n
    /// The samples parameter divides the given path into an ordered set of equidistant points along the path.
    /// </param>
    /// <param name="key">
    /// Your application's API key. \n
    /// This key identifies your application for purposes of quota management.
    /// </param>
    /// <param name="client">
    /// Client ID identifies you as a Maps API for Work customer and enables support and purchased quota for your application. \n
    /// Requests made without a client ID are not eligible for Maps API for Work benefits.
    /// </param>
    /// <param name="signature">
    /// Your welcome letter includes a cryptographic signing key, which you must use to generate unique signatures for your web service requests.
    /// </param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery Find(Vector2[] path, int samples, string key = null, string client = null, string signature = null)
    {
        OnlineMapsGetElevation query = new OnlineMapsGetElevation(path, samples, key, client, signature);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// Converts response into an array of results.
    /// </summary>
    /// <param name="response">Response of Google API.</param>
    /// <returns>Array of result.</returns>
    public static OnlineMapsGetElevationResult[] GetResults(string response)
    {
        OnlineMapsXML xml = OnlineMapsXML.Load(response);
        if (xml.isNull || xml.Get<string>("status") != "OK") return null;

        List<OnlineMapsGetElevationResult> rList = new List<OnlineMapsGetElevationResult>();
        foreach (OnlineMapsXML node in xml.FindAll("result")) rList.Add(new OnlineMapsGetElevationResult(node));

        return rList.ToArray();
    }
}

/// <summary>
/// Result of Google Maps Elevation query.
/// </summary>
public class OnlineMapsGetElevationResult
{
    /// <summary>
    /// Elevation of the location in meters.
    /// </summary>
    public float elevation;

    /// <summary>
    /// Position for which elevation data is being computed. \n
    /// Note that for path requests, the set of location elements will contain the sampled points along the path.
    /// </summary>
    public Vector2 location;

    /// <summary>
    /// Maximum distance between data points from which the elevation was interpolated, in meters. \n
    /// This property will be missing if the resolution is not known. \n
    /// Note that elevation data becomes more coarse (larger resolution values) when multiple points are passed. \n
    /// To obtain the most accurate elevation value for a point, it should be queried independently.
    /// </summary>
    public float resolution;

    public OnlineMapsGetElevationResult(OnlineMapsXML node)
    {
        elevation = node.Get<float>("elevation");
        resolution = node.Get<float>("resolution");

        OnlineMapsXML locationNode = node["location"];
        location = new Vector2(locationNode.Get<float>("lng"), locationNode.Get<float>("lat"));
    }
}