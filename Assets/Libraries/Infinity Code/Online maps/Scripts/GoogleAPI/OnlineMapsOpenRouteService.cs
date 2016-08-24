/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;

/// <summary>
/// This class is used to search for a route by coordinates using Open Route Service.\n
/// You can create a new instance using OnlineMapsOpenRouteService.Find.\n
/// http://wiki.openstreetmap.org/wiki/OpenRouteService
/// </summary>
[Serializable]
public class OnlineMapsOpenRouteService: OnlineMapsGoogleAPIQuery
{
    /// <summary>
    /// Gets the type of query to API.
    /// </summary>
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.ors; }
    }

    private OnlineMapsOpenRouteService(Vector2 start, Vector2 end, string lang, OnlineMapsOpenRouteServicePref pref, bool noMotorways, bool noTollways, Vector2[] via)
    {
        _status = OnlineMapsQueryStatus.downloading;
        string url = "https://openls.geog.uni-heidelberg.de/testing2015/route?Start={0}&End={1}&Via={2}&lang={3}&distunit=KM&routepref={4}&avoidAreas=&useTMC=false&noMotorways={5}&noTollways={6}&instructions=true";
        string viaStr = "";
        if (via != null && via.Length > 0)
        {
            string[] vias = new string[via.Length];
            for (int i = 0; i < via.Length; i++) vias[i] = via[i].x + "," + via[i].y;
            viaStr = string.Join(" ", vias);
        }
        url = string.Format(url, start.x + "," + start.y, end.x + "," + end.y, viaStr, lang, Enum.GetName(typeof(OnlineMapsOpenRouteServicePref), pref), noMotorways, noTollways);
        www = OnlineMapsUtils.GetWWW(url);
    }

    /// <summary>
    /// Creates a new request for a route search.
    /// </summary>
    /// <param name="start">Coordinates of the route begins.</param>
    /// <param name="end">Coordinates of the route ends.</param>
    /// <param name="lang">Language of intructions.</param>
    /// <param name="pref">The preference of the routing.</param>
    /// <param name="noMotorways">No Motorways.</param>
    /// <param name="noTollways">No Tollways.</param>
    /// <param name="via">Coordinates of the via positions.</param>
    /// <returns>Query instance.</returns>
    public static OnlineMapsOpenRouteService Find(Vector2 start, Vector2 end, string lang, OnlineMapsOpenRouteServicePref pref = OnlineMapsOpenRouteServicePref.Fastest, bool noMotorways = false, bool noTollways = false, Vector2[] via = null)
    {
        OnlineMapsOpenRouteService query = new OnlineMapsOpenRouteService(start, end, lang, pref, noMotorways, noTollways, via);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// The preference of the routing.
    /// </summary>
    public enum OnlineMapsOpenRouteServicePref
    {
        Fastest,
        Shortest,
        Pedestrian,
        Bicycle
    }
}