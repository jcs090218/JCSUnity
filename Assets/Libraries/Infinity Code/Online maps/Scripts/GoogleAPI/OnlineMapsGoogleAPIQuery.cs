/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// The base class of queries to Google API.
/// </summary>
[Serializable]
public class OnlineMapsGoogleAPIQuery
{
    /// <summary>
    /// Event that occurs when a response is received from Google API.
    /// </summary>
    public Action<string> OnComplete;

    /// <summary>
    /// Event that occurs when the current request instance is disposed.
    /// </summary>
    public Action<OnlineMapsGoogleAPIQuery> OnDispose;

    /// <summary>
    /// Event that occurs after OnComplete, when the response from Google API processed.
    /// </summary>
    public Action<OnlineMapsGoogleAPIQuery> OnFinish;

    /// <summary>
    /// In this variable you can put any data that you need to work with requests.
    /// </summary>
    public object customData;

    protected OnlineMapsQueryStatus _status;
    protected WWW www;

    private string _response;

    /// <summary>
    /// Gets a response from Google API.
    /// </summary>
    /// <value>
    /// The response.
    /// </value>
    public string response
    {
        get { return _response; }
    }

    /// <summary>
    /// Gets the current status of the request to the Google API.
    /// </summary>
    /// <value>
    /// The status.
    /// </value>
    public OnlineMapsQueryStatus status
    {
        get { return _status; }
    }

    /// <summary>
    /// Gets the type of query to Google API.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    public virtual OnlineMapsQueryType type
    {
        get
        {
            return OnlineMapsQueryType.none;
        }
    }

    /// <summary>
    /// Checks whether the response from Google API.
    /// </summary>
    public void CheckComplete()
    {
        if (www != null && www.isDone)
        {
            _status = string.IsNullOrEmpty(www.error) ? OnlineMapsQueryStatus.success : OnlineMapsQueryStatus.error;
            _response = _status == OnlineMapsQueryStatus.success ? www.text : www.error;

            if (OnComplete != null) OnComplete(_response);
            if (OnFinish != null) OnFinish(this);
            www.Dispose();
            www = null;
            customData = null;
        }
    }

    /// <summary>
    /// Converts Polyline to point list.
    /// </summary>
    /// <param name="encodedPoints">
    /// The encoded polyline.
    /// </param>
    /// <returns>
    /// A List of Vector2 points;
    /// </returns>
    public static List<Vector2> DecodePolylinePoints(string encodedPoints)
    {
        if (string.IsNullOrEmpty(encodedPoints)) return null;

        List<Vector2> poly = new List<Vector2>();
        char[] polylinechars = encodedPoints.ToCharArray();
        int index = 0;

        int currentLat = 0;
        int currentLng = 0;
        int next5bits;

        try
        {
            while (index < polylinechars.Length)
            {
                int sum = 0;
                int shifter = 0;
                do
                {
                    next5bits = polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length && next5bits >= 32) break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                Vector2 p = new Vector2(Convert.ToSingle(currentLng) / 100000.0f, Convert.ToSingle(currentLat) / 100000.0f);
                poly.Add(p);
            }
        }
        catch {}
        return poly;
    }

    /// <summary>
    /// Destroys the current request to Google API.
    /// </summary>
    public void Destroy()
    {
        if (OnDispose != null) OnDispose(this);
        if (www != null)
        {
            www.Dispose();
            www = null;
        }
        _response = string.Empty;
        _status = OnlineMapsQueryStatus.disposed;
        customData = null;
        OnComplete = null;
        OnFinish = null;
    }

    /// <summary>
    /// Converts XMLNode coordinates from Google Maps into Vector2.
    /// </summary>
    /// <param name="node">XMLNode coordinates from Google Maps.</param>
    /// <returns>Coordinates as Vector2.</returns>
    public static Vector2 GetVector2FromNode(OnlineMapsXML node)
    {
        float lng = node.Get<float>("lng");
        float lat = node.Get<float>("lat");
        return new Vector2(lng, lat);
    }
}