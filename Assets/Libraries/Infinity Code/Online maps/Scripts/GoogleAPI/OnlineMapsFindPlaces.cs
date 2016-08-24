/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Google Places API allows you to query for place information on a variety of categories, such as: establishments, prominent points of interest, geographic locations, and more. \n
/// You can search for places either by proximity or a text string. \n
/// A Place Search returns a list of places along with summary information about each place.\n
/// https://developers.google.com/places/web-service/search
/// </summary>
[Serializable]
public class OnlineMapsFindPlaces: OnlineMapsGoogleAPIQuery
{
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.places; }
    }

    private OnlineMapsFindPlaces(OnlineMapsFindPlacesType reqType, string key, Vector2 latlng = default(Vector2), int radius = -1, string keyword = null, string name = null, string types = null, string query = null, string language = null, int minprice = -1, int maxprice = -1, bool opennow = false, OnlineMapsFindPlacesRankBy rankBy = OnlineMapsFindPlacesRankBy.prominence)
    {
        _status = OnlineMapsQueryStatus.downloading;

        string typePath = string.Empty;

        if (reqType == OnlineMapsFindPlacesType.nearby) typePath = "nearbysearch";
        else if (reqType == OnlineMapsFindPlacesType.text) typePath = "textsearch";
        else if (reqType == OnlineMapsFindPlacesType.radar) typePath = "radarsearch";

        string url = "https://maps.googleapis.com/maps/api/place/{0}/xml?sensor=false";
        url = string.Format(url, typePath);

        if (latlng != default(Vector2)) url += string.Format("&location={0},{1}", latlng.y, latlng.x);
        if (radius != -1) url += "&radius=" + radius;
        if (!string.IsNullOrEmpty(key)) url += "&key=" + key;
        if (!string.IsNullOrEmpty(keyword)) url += "&keyword=" + keyword;
        if (!string.IsNullOrEmpty(name)) url += "&name=" + name;
        if (!string.IsNullOrEmpty(types)) url += "&types=" + types;
        if (!string.IsNullOrEmpty(query)) url += "&query=" + query.Replace(" ", "+");
        if (!string.IsNullOrEmpty(language)) url += "&language=" + language;
        if (minprice != -1) url += "&minprice=" + minprice;
        if (maxprice != -1) url += "&maxprice=" + maxprice;
        if (opennow) url += "&opennow";
        if (rankBy == OnlineMapsFindPlacesRankBy.distance) url += "&rankby=distance";

        www = OnlineMapsUtils.GetWWW(url);
    }

    /// <summary>
    /// A Nearby Search lets you search for places within a specified area. \n
    /// You can refine your search request by supplying keywords or specifying the type of place you are searching for.
    /// </summary>
    /// <param name="latlng">The latitude/longitude around which to retrieve place information. </param>
    /// <param name="radius">
    /// Defines the distance (in meters) within which to return place results. \n
    /// The maximum allowed radius is 50 000 meters.
    /// </param>
    /// <param name="key">
    /// Your application's API key. \n
    /// This key identifies your application for purposes of quota management and so that places added from your application are made immediately available to your app. \n
    /// Visit the Google APIs Console to create an API Project and obtain your key.
    /// </param>
    /// <param name="keyword">A term to be matched against all content that Google has indexed for this place, including but not limited to name, type, and address, as well as customer reviews and other third-party content.</param>
    /// <param name="name">
    /// One or more terms to be matched against the names of places, separated with a space character. \n
    /// Results will be restricted to those containing the passed name values. \n
    /// Note that a place may have additional names associated with it, beyond its listed name. \n
    /// The API will try to match the passed name value against all of these names. \n
    /// As a result, places may be returned in the results whose listed names do not match the search term, but whose associated names do.
    /// </param>
    /// <param name="types">
    /// Restricts the results to places matching at least one of the specified types. \n
    /// Types should be separated with a pipe symbol (type1|type2|etc).\n
    /// See the list of supported types:\n
    /// https://developers.google.com/places/documentation/supported_types
    /// </param>
    /// <param name="minprice">
    /// Restricts results to only those places within the specified range. \n
    /// Valid values range between 0 (most affordable) to 4 (most expensive), inclusive. \n
    /// The exact amount indicated by a specific value will vary from region to region.
    /// </param>
    /// <param name="maxprice">
    /// Restricts results to only those places within the specified range. \n
    /// Valid values range between 0 (most affordable) to 4 (most expensive), inclusive. \n
    /// The exact amount indicated by a specific value will vary from region to region.
    /// </param>
    /// <param name="opennow">
    /// Returns only those places that are open for business at the time the query is sent. \n
    /// Places that do not specify opening hours in the Google Places database will not be returned if you include this parameter in your query.
    /// </param>
    /// <param name="rankBy">Specifies the order in which results are listed.</param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery FindNearby(Vector2 latlng, int radius, string key, string keyword = null, string name = null, string types = null, int minprice = -1, int maxprice = -1, bool opennow = false, OnlineMapsFindPlacesRankBy rankBy = OnlineMapsFindPlacesRankBy.prominence)
    {
        OnlineMapsFindPlaces query = new OnlineMapsFindPlaces(
            OnlineMapsFindPlacesType.nearby, 
            key,
            latlng, 
            radius, 
            keyword, 
            name, 
            types, 
            null, 
            null,
            minprice, 
            maxprice, 
            opennow, 
            rankBy);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// Returns information about a set of places based on a string — for example "pizza in New York" or "shoe stores near Ottawa". \n
    /// The service responds with a list of places matching the text string and any location bias that has been set. \n
    /// The search response will include a list of places.
    /// </summary>
    /// <param name="query">
    /// The text string on which to search, for example: "restaurant". \n
    /// The Google Places service will return candidate matches based on this string and order the results based on their perceived relevance.
    /// </param>
    /// <param name="key">
    /// Your application's API key. \n
    /// This key identifies your application for purposes of quota management and so that places added from your application are made immediately available to your app. \n
    /// Visit the Google APIs Console to create an API Project and obtain your key.
    /// </param>
    /// <param name="latlng">The latitude/longitude around which to retrieve place information.</param>
    /// <param name="radius">
    /// Defines the distance (in meters) within which to bias place results. \n
    /// The maximum allowed radius is 50 000 meters. \n
    /// Results inside of this region will be ranked higher than results outside of the search circle; however, prominent results from outside of the search radius may be included.
    /// </param>
    /// <param name="language">The language code, indicating in which language the results should be returned, if possible. </param>
    /// <param name="types">
    /// Restricts the results to places matching at least one of the specified types. \n
    /// Types should be separated with a pipe symbol (type1|type2|etc). \n
    /// See the list of supported types:\n
    /// https://developers.google.com/maps/documentation/places/supported_types
    /// </param>
    /// <param name="minprice">
    /// Restricts results to only those places within the specified price level. \n
    /// Valid values are in the range from 0 (most affordable) to 4 (most expensive), inclusive. \n
    /// The exact amount indicated by a specific value will vary from region to region.
    /// </param>
    /// <param name="maxprice">
    /// Restricts results to only those places within the specified price level. \n
    /// Valid values are in the range from 0 (most affordable) to 4 (most expensive), inclusive. \n
    /// The exact amount indicated by a specific value will vary from region to region.
    /// </param>
    /// <param name="opennow">
    /// Returns only those places that are open for business at the time the query is sent. \n
    /// Places that do not specify opening hours in the Google Places database will not be returned if you include this parameter in your query.
    /// </param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery FindText(string query, string key, Vector2 latlng = default(Vector2), int radius = -1, string language = null, string types = null, int minprice = -1, int maxprice = -1, bool opennow = false)
    {
        OnlineMapsFindPlaces q = new OnlineMapsFindPlaces(
            OnlineMapsFindPlacesType.text, 
            key,
            latlng, 
            radius,
            null,
            null,
            types,
            query,
            language,
            minprice,
            maxprice,
            opennow); 
        OnlineMaps.instance.AddGoogleAPIQuery(q);
        return q;
    }

    /// <summary>
    /// The Google Places API Radar Search Service allows you to search for up to 200 places at once, but with less detail than is typically returned from a Text Search or Nearby Search request. \n
    /// With Radar Search, you can create applications that help users identify specific areas of interest within a geographic area.
    /// </summary>
    /// <param name="latlng">The latitude/longitude around which to retrieve place information.</param>
    /// <param name="radius">
    /// Defines the distance (in meters) within which to return place results. \n
    /// The maximum allowed radius is 50 000 meters.
    /// </param>
    /// <param name="key">
    /// Your application's API key. \n
    /// This key identifies your application for purposes of quota management and so that places added from your application are made immediately available to your app. \n
    /// Visit the Google APIs Console to create an API Project and obtain your key.
    /// </param>
    /// <param name="keyword">A term to be matched against all content that Google has indexed for this place, including but not limited to name, type, and address, as well as customer reviews and other third-party content.</param>
    /// <param name="name">
    /// One or more terms to be matched against the names of places, separated by a space character. \n
    /// Results will be restricted to those containing the passed name values. \n
    /// Note that a place may have additional names associated with it, beyond its listed name. \n
    /// The API will try to match the passed name value against all of these names. \n
    /// As a result, places may be returned in the results whose listed names do not match the search term, but whose associated names do.
    /// </param>
    /// <param name="types">
    /// Restricts the results to places matching at least one of the specified types. \n
    /// Types should be separated with a pipe symbol (type1|type2|etc). \n
    /// See the list of supported types:\n
    /// https://developers.google.com/maps/documentation/places/supported_types
    /// </param>
    /// <param name="minprice">
    /// Restricts results to only those places within the specified price level. \n
    /// Valid values are in the range from 0 (most affordable) to 4 (most expensive), inclusive. \n
    /// The exact amount indicated by a specific value will vary from region to region.
    /// </param>
    /// <param name="maxprice">
    /// Restricts results to only those places within the specified price level. \n
    /// Valid values are in the range from 0 (most affordable) to 4 (most expensive), inclusive. \n
    /// The exact amount indicated by a specific value will vary from region to region.
    /// </param>
    /// <param name="opennow">
    /// Returns only those places that are open for business at the time the query is sent. \n
    /// Places that do not specify opening hours in the Google Places database will not be returned if you include this parameter in your query.
    /// </param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsGoogleAPIQuery FindRadar(Vector2 latlng, int radius, string key, string keyword = null, string name = null, string types = null, int minprice = -1, int maxprice = -1, bool opennow = false)
    {
        OnlineMapsFindPlaces query = new OnlineMapsFindPlaces(
            OnlineMapsFindPlacesType.radar, 
            key,
            latlng, 
            radius, 
            keyword, 
            name, 
            types, 
            null,
            null,
            minprice, 
            maxprice, 
            opennow);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// Converts response into an array of results.
    /// </summary>
    /// <param name="response">Response of Google API.</param>
    /// <returns>Array of result.</returns>
    public static OnlineMapsFindPlacesResult[] GetResults(string response)
    {
        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(response);
            string status = xml.Find<string>("//status");
            if (status != "OK") return null;

            List<OnlineMapsFindPlacesResult> results = new List<OnlineMapsFindPlacesResult>();

            OnlineMapsXMLList resNodes = xml.FindAll("//result");

            foreach (OnlineMapsXML node in resNodes)
            {
                results.Add(new OnlineMapsFindPlacesResult(node));
            }

            return results.ToArray();
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message + "\n" + exception.StackTrace);
        }

        return null;
    }

    /// <summary>
    /// Type of Google Maps Places search.
    /// </summary>
    public enum OnlineMapsFindPlacesType
    {
        nearby,
        text,
        radar,
    }

    /// <summary>
    /// Specifies the order in which results are listed.
    /// </summary>
    public enum OnlineMapsFindPlacesRankBy
    {
        /// <summary>
        /// This option sorts results based on their importance. \n
        /// Ranking will favor prominent places within the specified area. \n
        /// Prominence can be affected by a place's ranking in Google's index, global popularity, and other factors. 
        /// </summary>
        prominence,

        /// <summary>
        /// This option sorts results in ascending order by their distance from the specified location. \n
        /// When distance is specified, one or more of keyword, name, or types is required.
        /// </summary>
        distance
    }
}

/// <summary>
/// Result of Google Maps Places query.
/// </summary>
public class OnlineMapsFindPlacesResult
{
    /// <summary>
    /// Coordinates of the place.
    /// </summary>
    public Vector2 location;

    /// <summary>
    /// URL of a recommended icon which may be displayed to the user when indicating this result.
    /// </summary>
    public string icon;

    /// <summary>
    /// Unique stable identifier denoting this place. \n
    /// This identifier may not be used to retrieve information about this place, but is guaranteed to be valid across sessions. \n
    /// It can be used to consolidate data about this place, and to verify the identity of a place across separate searches. \n
    /// Note: The id is now deprecated in favor of place_id.
    /// </summary>
    public string id;

    /// <summary>
    /// Human-readable address of this place. \n
    /// Often this address is equivalent to the "postal address". \n
    /// The formatted_address property is only returned for a Text Search.
    /// </summary>
    public string formatted_address;

    /// <summary>
    /// Human-readable name for the returned result. \n
    /// For establishment results, this is usually the business name.
    /// </summary>
    public string name;

    /// <summary>
    /// Unique identifier for a place.
    /// </summary>
    public string place_id;

    /// <summary>
    /// Unique token that you can use to retrieve additional information about this place in a Place Details request. \n
    /// Although this token uniquely identifies the place, the converse is not true. \n
    /// A place may have many valid reference tokens. \n
    /// It's not guaranteed that the same token will be returned for any given place across different searches. \n
    /// Note: The reference is now deprecated in favor of place_id.
    /// </summary>
    public string reference;

    /// <summary>
    /// Array of feature types describing the given result. \n
    /// XML responses include multiple type elements if more than one type is assigned to the result.
    /// </summary>
    public string[] types;

    /// <summary>
    /// Feature name of a nearby location. \n
    /// Often this feature refers to a street or neighborhood within the given results. \n
    /// The vicinity property is only returned for a Nearby Search.
    /// </summary>
    public string vicinity;

    /// <summary>
    ///  The price level of the place, on a scale of 0 to 4. 
    /// The exact amount indicated by a specific value will vary from region to region. 
    /// Price levels are interpreted as follows:
    /// -1 - Unknown
    /// 0 — Free
    /// 1 — Inexpensive
    /// 2 — Moderate
    /// 3 — Expensive
    /// 4 — Very Expensive
    /// </summary>
    public int price_level = -1;

    /// <summary>
    /// Place's rating, from 1.0 to 5.0, based on aggregated user reviews.
    /// </summary>
    public float rating;

    /// <summary>
    /// Value indicating if the place is open at the current time.
    /// </summary>
    public bool open_now;

    /// <summary>
    /// Indicates the scope of the place_id. 
    /// </summary>
    public string scope;

    /// <summary>
    /// Undocumented in Google Maps Places API.
    /// </summary>
    public string[] weekday_text;

    /// <summary>
    /// Array of photo objects, each containing a reference to an image. \n
    /// A Place Search will return at most one photo object. \n
    /// Performing a Place Details request on the place may return up to ten photos.
    /// </summary>
    public OnlineMapsFindPlacesResultPhoto[] photos;

    /// <summary>
    /// Constructor of OnlineMapsFindPlacesResult.
    /// </summary>
    /// <param name="node">Place node from response</param>
    public OnlineMapsFindPlacesResult(OnlineMapsXML node)
    {
        List<OnlineMapsFindPlacesResultPhoto> photos = new List<OnlineMapsFindPlacesResultPhoto>();
        List<string> types = new List<string>();
        List<string> weekday_text = new List<string>();

        foreach (OnlineMapsXML n in node)
        {
            if (n.name == "name") name = n.Value();
            else if (n.name == "id") id = n.Value();
            else if (n.name == "vicinity") vicinity = n.Value();
            else if (n.name == "type") types.Add(n.Value());
            else if (n.name == "geometry") location = OnlineMapsGoogleAPIQuery.GetVector2FromNode(n[0]);
            else if (n.name == "rating") rating = n.Value<float>();
            else if (n.name == "icon") icon = n.Value();
            else if (n.name == "reference") reference = n.Value();
            else if (n.name == "place_id") place_id = n.Value();
            else if (n.name == "scope") scope = n.Value();
            else if (n.name == "price_level") price_level = n.Value<int>();
            else if (n.name == "formatted_address") formatted_address = n.Value();
            else if (n.name == "opening_hours")
            {
                open_now = n.Get<string>("open_now") == "true";
                foreach (OnlineMapsXML wdt in n.FindAll("weekday_text")) weekday_text.Add(wdt.Value());
            }
            else if (n.name == "photo")
            {
                photos.Add(new OnlineMapsFindPlacesResultPhoto(n));
            }
            else Debug.Log(n.name);
        }

        this.photos = photos.ToArray();
        this.types = types.ToArray();
        this.weekday_text = weekday_text.ToArray();
    }
}

/// <summary>
/// Photo objects, contains a reference to an image.
/// </summary>
public class OnlineMapsFindPlacesResultPhoto
{
    /// <summary>
    /// The maximum width of the image.
    /// </summary>
    public int width;

    /// <summary>
    /// The maximum height of the image.
    /// </summary>
    public int height;

    /// <summary>
    /// String used to identify the photo when you perform a Photo request.
    /// </summary>
    public string photo_reference;

    /// <summary>
    /// Contains any required attributions. This field will always be present, but may be empty.
    /// </summary>
    public string[] html_attributions;

    /// <summary>
    /// Constructor of OnlineMapsFindPlacesResultPhoto.
    /// </summary>
    /// <param name="node">Photo node from response</param>
    public OnlineMapsFindPlacesResultPhoto(OnlineMapsXML node)
    {
        try
        {
            width = node.Get<int>("width");
            height = node.Get<int>("height");
            photo_reference = node["photo_reference"].Value();

            List<string> html_attributions = new List<string>();
            foreach (OnlineMapsXML ha in node.FindAll("html_attributions")) html_attributions.Add(ha.Value());
            this.html_attributions = html_attributions.ToArray();
        }
        catch (Exception)
        {
        }
    }
}