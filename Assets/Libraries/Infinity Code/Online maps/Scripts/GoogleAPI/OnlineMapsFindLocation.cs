/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// This class is used to search for a location by address.\n
/// You can create a new instance using OnlineMaps.FindLocation.\n
/// https://developers.google.com/maps/documentation/geocoding/intro
/// </summary>
[System.Serializable]
public class OnlineMapsFindLocation : OnlineMapsGoogleAPIQuery
{
    /// <summary>
    /// Gets the type of query to Google API.
    /// </summary>
    /// <value>
    /// OnlineMapsQueryType.location
    /// </value>
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.location; }
    }

    /// <summary>
    /// Constructor. \n
    /// <strong>Please do not use. </strong>\n
    /// Use OnlineMapsFindLocation.Find.
    /// </summary>
    /// <param name="address">Location title</param>
    /// <param name="latlng">Location coordinates</param>
    public OnlineMapsFindLocation(string address = null, string latlng = null)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://maps.googleapis.com/maps/api/geocode/xml?sensor=false";
        if (!string.IsNullOrEmpty(address)) url += "&address=" + address.Replace(" ", "+");
        if (!string.IsNullOrEmpty(latlng)) url += "&latlng=" + latlng.Replace(" ", "");
        www = OnlineMapsUtils.GetWWW(url);
    }

    /// <summary>
    /// Creates a new request for a location search.\n
    /// This method is used for Reverse Geocoding.\n
    /// https://developers.google.com/maps/documentation/geocoding/intro#Geocoding
    /// </summary>
    /// <param name="address">Location title</param>
    /// <param name="latlng">Location coordinates</param>
    /// <returns>Instance of the search query.</returns>
    public static OnlineMapsGoogleAPIQuery Find(string address = null, string latlng = null)
    {
        OnlineMapsFindLocation query = new OnlineMapsFindLocation(address, latlng);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// Creates a new request for a location search.\n
    /// This method is used for Reverse Geocoding.\n
    /// https://developers.google.com/maps/documentation/geocoding/intro#ReverseGeocoding
    /// </summary>
    /// <param name="latlng">Location coordinates</param>
    /// <returns>Instance of the search query.</returns>
    public static OnlineMapsGoogleAPIQuery Find(Vector2 latlng)
    {
        OnlineMapsFindLocation query = new OnlineMapsFindLocation(null, string.Format("{0},{1}", latlng.y, latlng.x));
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// Gets the coordinates of the first results from OnlineMapsFindLocation result.
    /// </summary>
    /// <param name="result">Coordinates - if successful, Vector2.zero - if failed.</param>
    /// <returns>Vector2 coordinates</returns>
    public static Vector2 GetCoordinatesFromResult(string result)
    {
        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(result);

            OnlineMapsXML location = xml.Find("//geometry/location");
            if (location.isNull) return Vector2.zero;

            return GetVector2FromNode(location);
        }
        catch { }
        return Vector2.zero;
    }

    /// <summary>
    /// Centers the map on the result of the search location.
    /// </summary>
    /// <param name="result">XML string. The result of the search location.</param>
    public static void MovePositionToResult(string result)
    {
        Vector2 position = GetCoordinatesFromResult(result);
        if (position != Vector2.zero) OnlineMaps.instance.position = position;
    }
}