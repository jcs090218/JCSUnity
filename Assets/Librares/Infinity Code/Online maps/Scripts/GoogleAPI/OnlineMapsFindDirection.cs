/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// This class is used to search for a route by address or coordinates.\n
/// You can create a new instance using OnlineMapsFindDirection.Find.\n
/// https://developers.google.com/maps/documentation/directions/intro
/// </summary>
[System.Serializable]
public class OnlineMapsFindDirection : OnlineMapsGoogleAPIQuery
{
    /// <summary>
    /// Gets the type of query to Google API.
    /// </summary>
    /// <value>
    /// OnlineMapsQueryType.direction
    /// </value>
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.direction; }
    }

    private OnlineMapsFindDirection(string origin, string destination, bool alternatives = false)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://maps.google.com/maps/api/directions/xml?origin={0}&destination={1}&sensor=false";
        if (alternatives) url += "&alternatives=true";
        url = string.Format(url, origin.Replace(" ", "+"), destination.Replace(" ", "+"));
        www = OnlineMapsUtils.GetWWW(url);
        OnlineMaps.instance.AddGoogleAPIQuery(this);
    }

    /// <summary>Creates a new request for a route search.</summary>
    /// <param name="origin">Title of the route begins.</param>
    /// <param name="destination">Title of the route ends.</param>
    /// <param name="alternatives">Search for alternative routes.</param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery Find(string origin, string destination, bool alternatives = false)
    {
        return new OnlineMapsFindDirection(origin, destination, alternatives);
    }

    /// <summary>Creates a new request for a route search.</summary>
    /// <param name="origin">Title of the route begins.</param>
    /// <param name="destination">Coordinates of the route ends.</param>
    /// <param name="alternatives">Search for alternative routes.</param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery Find(string origin, Vector2 destination, bool alternatives = false)
    {
        return Find(origin, destination.y + "," + destination.x, alternatives);
    }

    /// <summary>Creates a new request for a route search.</summary>
    /// <param name="origin">Coordinates of the route begins.</param>
    /// <param name="destination">Title of the route ends.</param>
    /// <param name="alternatives">Search for alternative routes.</param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery Find(Vector2 origin, string destination, bool alternatives = false)
    {
        return Find(origin.y + "," + origin.x, destination, alternatives);
    }

    /// <summary>Creates a new request for a route search.</summary>
    /// <param name="origin">Coordinates of the route begins.</param>
    /// <param name="destination">Coordinates of the route ends.</param>
    /// <param name="alternatives">Search for alternative routes.</param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery Find(Vector2 origin, Vector2 destination, bool alternatives = false)
    {
        return Find(origin.y + "," + origin.x, destination.y + "," + destination.x, alternatives);
    }
}