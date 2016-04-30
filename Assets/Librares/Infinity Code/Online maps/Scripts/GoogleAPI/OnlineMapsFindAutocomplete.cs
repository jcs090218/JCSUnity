/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Retrieves information from Google Maps Place Autocomplete API.\n
/// Place Autocomplete service is a web service that returns place predictions.\n
/// The request specifies a textual search string and optional geographic bounds.\n 
/// The service can be used to provide autocomplete functionality for text-based geographic searches, by returning places such as businesses, addresses and points of interest as a user types.\n
/// <strong>Requires Google Maps API key.</strong>\n
/// https://developers.google.com/places/documentation/autocomplete
/// </summary>
public class OnlineMapsFindAutocomplete:OnlineMapsGoogleAPIQuery
{
    public override OnlineMapsQueryType type
    {
        get { return OnlineMapsQueryType.autocomplete; }
    }

    private OnlineMapsFindAutocomplete(string input, string key, string types, int offset, Vector2 latlng, int radius, string language, string components)
    {
        _status = OnlineMapsQueryStatus.downloading;

        string url = "https://maps.googleapis.com/maps/api/place/autocomplete/xml?sensor=false";
        url += "&input=" + input.Replace(" ", "+");
        url += "&key=" + key;

        if (latlng != default(Vector2)) url += string.Format("&location={0},{1}", latlng.y, latlng.x);
        if (radius != -1) url += "&radius=" + radius;
        if (offset != -1) url += "&offset=" + offset;
        if (!string.IsNullOrEmpty(types)) url += "&types=" + types;
        if (!string.IsNullOrEmpty(components)) url += "&components=" + components;
        if (!string.IsNullOrEmpty(language)) url += "&language=" + language;

        www = OnlineMapsUtils.GetWWW(url);
    }

    /// <summary>
    /// Creates a new request to the Google Maps Place Autocomplete API.
    /// </summary>
    /// <param name="input">
    /// The text string on which to search. \n
    /// The Place Autocomplete service will return candidate matches based on this string and order results based on their perceived relevance.
    /// </param>
    /// <param name="key">
    /// Your application's API key. This key identifies your application for purposes of quota management. \n
    /// Visit the Google APIs Console to select an API Project and obtain your key. 
    /// </param>
    /// <param name="types">The types of place results to return.</param>
    /// <param name="offset">
    /// The position, in the input term, of the last character that the service uses to match predictions. \n
    /// For example, if the input is 'Google' and the offset is 3, the service will match on 'Goo'. \n
    /// The string determined by the offset is matched against the first word in the input term only. \n
    /// For example, if the input term is 'Google abc' and the offset is 3, the service will attempt to match against 'Goo abc'. \n
    /// If no offset is supplied, the service will use the whole term. \n
    /// The offset should generally be set to the position of the text caret.
    /// </param>
    /// <param name="latlng">The point around which you wish to retrieve place information.</param>
    /// <param name="radius">
    /// The distance (in meters) within which to return place results. \n
    /// Note that setting a radius biases results to the indicated area, but may not fully restrict results to the specified area.
    /// </param>
    /// <param name="language">The language in which to return results.</param>
    /// <param name="components">
    /// A grouping of places to which you would like to restrict your results. \n
    /// Currently, you can use components to filter by country. \n
    /// The country must be passed as a two character, ISO 3166-1 Alpha-2 compatible country code. \n
    /// For example: components=country:fr would restrict your results to places within France.
    /// </param>
    /// <returns>Query instance to the Google API.</returns>
    public static OnlineMapsFindAutocomplete Find(string input, string key, string types = null, int offset = -1, Vector2 latlng = default(Vector2), int radius = -1, string language = null, string components = null)
    {
        OnlineMapsFindAutocomplete query = new OnlineMapsFindAutocomplete(
            input,
            key,
            types,
            offset,
            latlng, 
            radius, 
            language, 
            components);
        OnlineMaps.instance.AddGoogleAPIQuery(query);
        return query;
    }

    /// <summary>
    /// Converts response into an array of results.
    /// </summary>
    /// <param name="response">Response of Google API.</param>
    /// <returns>Array of result.</returns>
    public static OnlineMapsFindAutocompleteResult[] GetResults(string response)
    {
        try
        {
            OnlineMapsXML xml = OnlineMapsXML.Load(response);
            string status = xml.Find<string>("//status");
            if (status != "OK") return null;

            List<OnlineMapsFindAutocompleteResult> results = new List<OnlineMapsFindAutocompleteResult>();

            OnlineMapsXMLList resNodes = xml.FindAll("//prediction");

            foreach (OnlineMapsXML node in resNodes)
            {
                results.Add(new OnlineMapsFindAutocompleteResult(node));
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
/// Result of Google Maps Place Autocomplete query.
/// </summary>
public class OnlineMapsFindAutocompleteResult
{
    /// <summary>
    /// Human-readable name for the returned result. For establishment results, this is usually the business name.
    /// </summary>
    public string description;

    /// <summary>
    /// Unique token that you can use to retrieve additional information about this place in a Place Details request. \n
    /// Although this token uniquely identifies the place, the converse is not true. A place may have many valid reference tokens. \n
    /// It's not guaranteed that the same token will be returned for any given place across different searches. \n
    /// Note: The reference is deprecated in favor of place_id. 
    /// </summary>
    public string reference;

    /// <summary>
    /// Unique stable identifier denoting this place. \n
    /// This identifier may not be used to retrieve information about this place, but can be used to consolidate data about this place, and to verify the identity of a place across separate searches. \n
    /// Note: The id is deprecated in favor of place_id.
    /// </summary>
    public string id;
    
    /// <summary>
    /// Unique identifier for a place. \n
    /// To retrieve information about the place, pass this identifier in the placeId field of a Places API request.\n
    /// </summary>
    public string place_id;

    /// <summary>
    /// Array of types that apply to this place. \n
    /// For example: [ "political", "locality" ] or [ "establishment", "geocode" ].
    /// </summary>
    public string[] types;

    /// <summary>
    /// Array of terms identifying each section of the returned description (a section of the description is generally terminated with a comma).
    /// </summary>
    public OnlineMapsFindAutocompleteResultTerm[] terms;

    /// <summary>
    /// These describe the location of the entered term in the prediction result text, so that the term can be highlighted if desired.
    /// </summary>
    public OnlineMapsFindAutocompleteResultMatchedSubstring matchedSubstring;

    /// <summary>
    /// Constructor of OnlineMapsFindAutocompleteResult.
    /// </summary>
    /// <param name="node">Result node from response.</param>
    public OnlineMapsFindAutocompleteResult(OnlineMapsXML node)
    {
        List<OnlineMapsFindAutocompleteResultTerm> terms = new List<OnlineMapsFindAutocompleteResultTerm>();
        List<string> types = new List<string>();

        foreach (OnlineMapsXML n in node)
        {
            if (n.name == "description") description = n.Value();
            else if (n.name == "type") types.Add(n.Value());
            else if (n.name == "id") id = n.Value();
            else if (n.name == "place_id") place_id = n.Value();
            else if (n.name == "reference") reference = n.Value();
            else if (n.name == "term") terms.Add(new OnlineMapsFindAutocompleteResultTerm(n));
            else if (n.name == "matched_substring") matchedSubstring = new OnlineMapsFindAutocompleteResultMatchedSubstring(n);
            else Debug.Log(n.name);
        }

        this.terms = terms.ToArray();
        this.types = types.ToArray();
    }
}

/// <summary>
/// Term identifying each section of the returned description.
/// </summary>
public class OnlineMapsFindAutocompleteResultTerm
{
    /// <summary>
    /// Term value.
    /// </summary>
    public string value;

    /// <summary>
    /// Term offset
    /// </summary>
    public int offset;

    /// <summary>
    /// Constructor of OnlineMapsFindAutocompleteResultTerm.
    /// </summary>
    /// <param name="node">Term node from response.</param>
    public OnlineMapsFindAutocompleteResultTerm(OnlineMapsXML node)
    {
        try
        {
            value = node.Get<string>("value");
            offset = node.Get<int>("height");
        }
        catch (Exception)
        {
        }
    }
}

/// <summary>
/// These describe the location of the entered term in the prediction result text, so that the term can be highlighted if desired.
/// </summary>
public class OnlineMapsFindAutocompleteResultMatchedSubstring
{
    /// <summary>
    /// Substring offset.
    /// </summary>
    public int offset;

    /// <summary>
    /// Substring length.
    /// </summary>
    public int length;

    /// <summary>
    /// Constructor of OnlineMapsFindAutocompleteResultMatchedSubstring.
    /// </summary>
    /// <param name="node">MatchedSubstring node from response.</param>
    public OnlineMapsFindAutocompleteResultMatchedSubstring(OnlineMapsXML node)
    {
        try
        {
            length = node.Get<int>("length");
            offset = node.Get<int>("height");
        }
        catch (Exception)
        {
        }
    }
}