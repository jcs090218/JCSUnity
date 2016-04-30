/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;

/// <summary>
/// This class is used to search for a route with full props by address or coordinates.\n
/// You can create a new instance using OnlineMapsFindDirectionAdvanced.Find.\n
/// https://developers.google.com/maps/documentation/directions/intro
/// </summary>
public class OnlineMapsFindDirectionAdvanced:OnlineMapsGoogleAPIQuery
{
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.direction; }
    }

    private OnlineMapsFindDirectionAdvanced(string origin,
        string destination,
        OnlineMapsFindDirectionMode mode = OnlineMapsFindDirectionMode.driving,
        string[] waypoints = null,
        bool alternatives = false,
        OnlineMapsFindDirectionAvoid avoid = OnlineMapsFindDirectionAvoid.none,
        OnlineMapsFindDirectionUnits units = OnlineMapsFindDirectionUnits.metric,
        string region = null,
        long departure_time = -1,
        long arrival_time = -1,
        string language = null)
    {
        _status = OnlineMapsQueryStatus.downloading;


        string url = "https://maps.google.com/maps/api/directions/xml?origin={0}&destination={1}&sensor=false";
        url = string.Format(url, origin.Replace(" ", "+"), destination.Replace(" ", "+"));

        if (mode != OnlineMapsFindDirectionMode.driving) url += "&mode=" + Enum.GetName(typeof (OnlineMapsFindDirectionMode), mode);
        if (waypoints != null) url += "&waypoints=optimize:true|" + String.Join("|", waypoints).Replace(" ", "+");
        if (alternatives) url += "&alternatives=true";
        if (avoid != OnlineMapsFindDirectionAvoid.none) url += "&avoid=" + Enum.GetName(typeof(OnlineMapsFindDirectionAvoid), avoid);
        if (units != OnlineMapsFindDirectionUnits.metric) url += "&units" + Enum.GetName(typeof(OnlineMapsFindDirectionUnits), units);
        if (!string.IsNullOrEmpty(region)) url += "&region=" + region;
        if (departure_time != -1) url += "&departure_time=" + departure_time;
        if (arrival_time != -1) url += "&arrival_time=" + arrival_time;
        if (!string.IsNullOrEmpty(language)) url += "&language=" + language;

        www = OnlineMapsUtils.GetWWW(url);
    }

    /// <summary>
    /// Calculates directions between locations.\n
    /// You can search for directions for several modes of transportation, include transit, driving, walking or cycling. \n
    /// Directions may specify origins, destinations as latitude/longitude coordinates.
    /// </summary>
    /// <param name="origin">Latitude/longitude value from which you wish to calculate directions.</param>
    /// <param name="destination">Latitude/longitude value from which you wish to calculate directions.</param>
    /// <param name="mode">Specifies the mode of transport to use when calculating directions.</param>
    /// <param name="waypoints">
    /// Specifies an array of waypoints. \n
    /// Waypoints alter a route by routing it through the specified location(s). \n
    /// A waypoint is specified as either a latitude/longitude coordinate or as an address which will be geocoded. \n
    /// Waypoints are only supported for driving, walking and bicycling directions.
    /// </param>
    /// <param name="alternatives">
    /// If set to true, specifies that the Directions service may provide more than one route alternative in the response. \n
    /// Note that providing route alternatives may increase the response time from the server.
    /// </param>
    /// <param name="avoid">Indicates that the calculated route(s) should avoid the indicated features.</param>
    /// <param name="units">
    /// Specifies the unit system to use when displaying results.\n
    /// Note: this unit system setting only affects the text displayed within distance fields. The distance fields also contain values which are always expressed in meters.
    /// </param>
    /// <param name="region">Specifies the region code, specified as a ccTLD ("top-level domain") two-character value.</param>
    /// <param name="departure_time">
    /// Specifies the desired time of departure. \n
    /// You can specify the time as an integer in seconds since midnight, January 1, 1970 UTC. \n
    /// Alternatively, you can specify a value of now, which sets the departure time to the current time (correct to the nearest second).\n
    /// The departure time may be specified in two cases:\n
    /// For transit directions: You can optionally specify one of departure_time or arrival_time. If neither time is specified, the departure_time defaults to now (that is, the departure time defaults to the current time).\n
    /// For driving directions: Google Maps API for Work customers can specify the departure_time to receive trip duration considering current traffic conditions. The departure_time must be set to within a few minutes of the current time.
    /// </param>
    /// <param name="arrival_time">
    /// Specifies the desired time of arrival for transit directions, in seconds since midnight, January 1, 1970 UTC. \n
    /// You can specify either departure_time or arrival_time, but not both. \n
    /// Note that arrival_time must be specified as an integer.
    /// </param>
    /// <param name="language">
    /// Specifies the language in which to return results.\n
    /// Note that we often update supported languages so this list may not be exhaustive.\n
    /// If language is not supplied, the service will attempt to use the native language of the domain from which the request is sent.
    /// </param>
    /// <returns>Instance of the search query.</returns>
    public static OnlineMapsFindDirectionAdvanced Find(
        Vector2 origin, 
        Vector2 destination, 
        OnlineMapsFindDirectionMode mode = OnlineMapsFindDirectionMode.driving, 
        string[] waypoints = null, 
        bool alternatives = false, 
        OnlineMapsFindDirectionAvoid avoid = OnlineMapsFindDirectionAvoid.none, 
        OnlineMapsFindDirectionUnits units = OnlineMapsFindDirectionUnits.metric, 
        string region = null, 
        long departure_time = -1, 
        long arrival_time = -1,
        string language = null)
    {
        return Find(origin.y + "," + origin.x,
            destination.y + "," + destination.x,
            mode, waypoints, alternatives, avoid, units, region, departure_time, arrival_time, language);
    }

    /// <summary>
    /// Calculates directions between locations.\n
    /// You can search for directions for several modes of transportation, include transit, driving, walking or cycling. \n
    /// Directions may specify origins, destinations as latitude/longitude coordinates.
    /// </summary>
    /// <param name="origin">Latitude/longitude value from which you wish to calculate directions.</param>
    /// <param name="destination">The address from which you wish to calculate directions.</param>
    /// <param name="mode">Specifies the mode of transport to use when calculating directions.</param>
    /// <param name="waypoints">
    /// Specifies an array of waypoints. \n
    /// Waypoints alter a route by routing it through the specified location(s). \n
    /// A waypoint is specified as either a latitude/longitude coordinate or as an address which will be geocoded. \n
    /// Waypoints are only supported for driving, walking and bicycling directions.
    /// </param>
    /// <param name="alternatives">
    /// If set to true, specifies that the Directions service may provide more than one route alternative in the response. \n
    /// Note that providing route alternatives may increase the response time from the server.
    /// </param>
    /// <param name="avoid">Indicates that the calculated route(s) should avoid the indicated features.</param>
    /// <param name="units">
    /// Specifies the unit system to use when displaying results.\n
    /// Note: this unit system setting only affects the text displayed within distance fields. The distance fields also contain values which are always expressed in meters.
    /// </param>
    /// <param name="region">Specifies the region code, specified as a ccTLD ("top-level domain") two-character value.</param>
    /// <param name="departure_time">
    /// Specifies the desired time of departure. \n
    /// You can specify the time as an integer in seconds since midnight, January 1, 1970 UTC. \n
    /// Alternatively, you can specify a value of now, which sets the departure time to the current time (correct to the nearest second).\n
    /// The departure time may be specified in two cases:\n
    /// For transit directions: You can optionally specify one of departure_time or arrival_time. If neither time is specified, the departure_time defaults to now (that is, the departure time defaults to the current time).\n
    /// For driving directions: Google Maps API for Work customers can specify the departure_time to receive trip duration considering current traffic conditions. The departure_time must be set to within a few minutes of the current time.
    /// </param>
    /// <param name="arrival_time">
    /// Specifies the desired time of arrival for transit directions, in seconds since midnight, January 1, 1970 UTC. \n
    /// You can specify either departure_time or arrival_time, but not both. \n
    /// Note that arrival_time must be specified as an integer.
    /// </param>
    /// <param name="language">
    /// Specifies the language in which to return results.\n
    /// Note that we often update supported languages so this list may not be exhaustive.\n
    /// If language is not supplied, the service will attempt to use the native language of the domain from which the request is sent.
    /// </param>
    /// <returns>Instance of the search query.</returns>
    public static OnlineMapsFindDirectionAdvanced Find(
        Vector2 origin,
        string destination,
        OnlineMapsFindDirectionMode mode = OnlineMapsFindDirectionMode.driving,
        string[] waypoints = null,
        bool alternatives = false,
        OnlineMapsFindDirectionAvoid avoid = OnlineMapsFindDirectionAvoid.none,
        OnlineMapsFindDirectionUnits units = OnlineMapsFindDirectionUnits.metric,
        string region = null,
        long departure_time = -1,
        long arrival_time = -1,
        string language = null)
    {
        return Find(origin.y + "," + origin.x, destination, mode, waypoints,
            alternatives, avoid, units, region, departure_time, arrival_time, language);
    }

    /// <summary>
    /// Calculates directions between locations.\n
    /// You can search for directions for several modes of transportation, include transit, driving, walking or cycling. \n
    /// Directions may specify origins, destinations as latitude/longitude coordinates.
    /// </summary>
    /// <param name="origin">The address from which you wish to calculate directions.</param>
    /// <param name="destination">Latitude/longitude value from which you wish to calculate directions.</param>
    /// <param name="mode">Specifies the mode of transport to use when calculating directions.</param>
    /// <param name="waypoints">
    /// Specifies an array of waypoints. \n
    /// Waypoints alter a route by routing it through the specified location(s). \n
    /// A waypoint is specified as either a latitude/longitude coordinate or as an address which will be geocoded. \n
    /// Waypoints are only supported for driving, walking and bicycling directions.
    /// </param>
    /// <param name="alternatives">
    /// If set to true, specifies that the Directions service may provide more than one route alternative in the response. \n
    /// Note that providing route alternatives may increase the response time from the server.
    /// </param>
    /// <param name="avoid">Indicates that the calculated route(s) should avoid the indicated features.</param>
    /// <param name="units">
    /// Specifies the unit system to use when displaying results.\n
    /// Note: this unit system setting only affects the text displayed within distance fields. The distance fields also contain values which are always expressed in meters.
    /// </param>
    /// <param name="region">Specifies the region code, specified as a ccTLD ("top-level domain") two-character value.</param>
    /// <param name="departure_time">
    /// Specifies the desired time of departure. \n
    /// You can specify the time as an integer in seconds since midnight, January 1, 1970 UTC. \n
    /// Alternatively, you can specify a value of now, which sets the departure time to the current time (correct to the nearest second).\n
    /// The departure time may be specified in two cases:\n
    /// For transit directions: You can optionally specify one of departure_time or arrival_time. If neither time is specified, the departure_time defaults to now (that is, the departure time defaults to the current time).\n
    /// For driving directions: Google Maps API for Work customers can specify the departure_time to receive trip duration considering current traffic conditions. The departure_time must be set to within a few minutes of the current time.
    /// </param>
    /// <param name="arrival_time">
    /// Specifies the desired time of arrival for transit directions, in seconds since midnight, January 1, 1970 UTC. \n
    /// You can specify either departure_time or arrival_time, but not both. \n
    /// Note that arrival_time must be specified as an integer.
    /// </param>
    /// <param name="language">
    /// Specifies the language in which to return results.\n
    /// Note that we often update supported languages so this list may not be exhaustive.\n
    /// If language is not supplied, the service will attempt to use the native language of the domain from which the request is sent.
    /// </param>
    /// <returns>Instance of the search query.</returns>
    public static OnlineMapsFindDirectionAdvanced Find(
        string origin,
        Vector2 destination,
        OnlineMapsFindDirectionMode mode = OnlineMapsFindDirectionMode.driving,
        string[] waypoints = null,
        bool alternatives = false,
        OnlineMapsFindDirectionAvoid avoid = OnlineMapsFindDirectionAvoid.none,
        OnlineMapsFindDirectionUnits units = OnlineMapsFindDirectionUnits.metric,
        string region = null,
        long departure_time = -1,
        long arrival_time = -1,
        string language = null)
    {
        return Find(origin, destination.y + "," + destination.x, mode, waypoints,
            alternatives, avoid, units, region, departure_time, arrival_time, language);
    }

    /// <summary>
    /// Calculates directions between locations.\n
    /// You can search for directions for several modes of transportation, include transit, driving, walking or cycling. \n
    /// Directions may specify origins, destinations as latitude/longitude coordinates.
    /// </summary>
    /// <param name="origin">The address from which you wish to calculate directions.</param>
    /// <param name="destination">The address from which you wish to calculate directions.</param>
    /// <param name="mode">Specifies the mode of transport to use when calculating directions.</param>
    /// <param name="waypoints">
    /// Specifies an array of waypoints. \n
    /// Waypoints alter a route by routing it through the specified location(s). \n
    /// A waypoint is specified as either a latitude/longitude coordinate or as an address which will be geocoded. \n
    /// Waypoints are only supported for driving, walking and bicycling directions.
    /// </param>
    /// <param name="alternatives">
    /// If set to true, specifies that the Directions service may provide more than one route alternative in the response. \n
    /// Note that providing route alternatives may increase the response time from the server.
    /// </param>
    /// <param name="avoid">Indicates that the calculated route(s) should avoid the indicated features.</param>
    /// <param name="units">
    /// Specifies the unit system to use when displaying results.\n
    /// Note: this unit system setting only affects the text displayed within distance fields. The distance fields also contain values which are always expressed in meters.
    /// </param>
    /// <param name="region">Specifies the region code, specified as a ccTLD ("top-level domain") two-character value.</param>
    /// <param name="departure_time">
    /// Specifies the desired time of departure. \n
    /// You can specify the time as an integer in seconds since midnight, January 1, 1970 UTC. \n
    /// Alternatively, you can specify a value of now, which sets the departure time to the current time (correct to the nearest second).\n
    /// The departure time may be specified in two cases:\n
    /// For transit directions: You can optionally specify one of departure_time or arrival_time. If neither time is specified, the departure_time defaults to now (that is, the departure time defaults to the current time).\n
    /// For driving directions: Google Maps API for Work customers can specify the departure_time to receive trip duration considering current traffic conditions. The departure_time must be set to within a few minutes of the current time.
    /// </param>
    /// <param name="arrival_time">
    /// Specifies the desired time of arrival for transit directions, in seconds since midnight, January 1, 1970 UTC. \n
    /// You can specify either departure_time or arrival_time, but not both. \n
    /// Note that arrival_time must be specified as an integer.
    /// </param>
    /// <param name="language">
    /// Specifies the language in which to return results.\n
    /// Note that we often update supported languages so this list may not be exhaustive.\n
    /// If language is not supplied, the service will attempt to use the native language of the domain from which the request is sent.
    /// </param>
    /// <returns>Instance of the search query.</returns>
    public static OnlineMapsFindDirectionAdvanced Find(
        string origin,
        string destination,
        OnlineMapsFindDirectionMode mode = OnlineMapsFindDirectionMode.driving,
        string[] waypoints = null,
        bool alternatives = false,
        OnlineMapsFindDirectionAvoid avoid = OnlineMapsFindDirectionAvoid.none,
        OnlineMapsFindDirectionUnits units = OnlineMapsFindDirectionUnits.metric,
        string region = null,
        long departure_time = -1,
        long arrival_time = -1,
        string language = null)
    {
        OnlineMapsFindDirectionAdvanced query = new OnlineMapsFindDirectionAdvanced(origin, destination, mode, waypoints, alternatives, avoid, units, region, departure_time, arrival_time, language);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }
}

/// <summary>
/// Mode of transport to use when calculating directions.
/// </summary>
public enum OnlineMapsFindDirectionMode
{
    /// <summary>
    /// Indicates standard driving directions using the road network.
    /// </summary>
    driving,

    /// <summary>
    /// Requests walking directions via pedestrian paths & sidewalks (where available).
    /// </summary>
    walking,

    /// <summary>
    /// Requests bicycling directions via bicycle paths & preferred streets (where available).
    /// </summary>
    bicycling,

    /// <summary>
    /// Requests directions via public transit routes (where available). \n
    /// If you set the mode to transit, you can optionally specify either a departure_time or an arrival_time. \n
    /// If neither time is specified, the departure_time defaults to now (that is, the departure time defaults to the current time). 
    /// </summary>
    transit
}

/// <summary>
/// Indicates that the calculated route(s) should avoid the indicated features.
/// </summary>
public enum OnlineMapsFindDirectionAvoid
{
    /// <summary>
    /// None avoid.
    /// </summary>
    none,

    /// <summary>
    /// Indicates that the calculated route should avoid toll roads/bridges.
    /// </summary>
    tolls,

    /// <summary>
    /// Indicates that the calculated route should avoid highways.
    /// </summary>
    highways,

    /// <summary>
    /// Indicates that the calculated route should avoid ferries.
    /// </summary>
    ferries
}

/// <summary>
/// Specifies the unit system to use when displaying results. 
/// </summary>
public enum OnlineMapsFindDirectionUnits
{
    /// <summary>
    /// Specifies usage of the metric system. Textual distances are returned using kilometers and meters.
    /// </summary>
    metric,

    /// <summary>
    /// Specifies usage of the Imperial (English) system. Textual distances are returned using miles and feet.
    /// </summary>
    imperial
}