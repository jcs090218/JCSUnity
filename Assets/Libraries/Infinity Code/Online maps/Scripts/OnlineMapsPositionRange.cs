/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// Class limiting the coordinates of the map.
/// </summary>
public class OnlineMapsPositionRange
{
    /// <summary>
    /// Minimum latitude value
    /// </summary>
    public float minLat = -90;

    /// <summary>
    /// Minimum longitude value
    /// </summary>
    public float minLng = -180;

    /// <summary>
    /// Maximum latitude value
    /// </summary>
    public float maxLat = 90;

    /// <summary>
    /// Maximum longitude value
    /// </summary>
    public float maxLng = 180;

    /// <summary>
    /// Type of limitation position map.
    /// </summary>
    public OnlineMapsPositionRangeType type = OnlineMapsPositionRangeType.center;

    /// <summary>
    /// Center of range.
    /// </summary>
    public Vector2 center
    {
        get
        {
            return new Vector2((maxLng - minLng) / 2 + minLng, (maxLat - minLat) / 2 + minLat);
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="minLat">Minimum latitude value</param>
    /// <param name="minLng">Minimum longitude value</param>
    /// <param name="maxLat">Maximum latitude value</param>
    /// <param name="maxLng">Maximum longitude value</param>
    /// <param name="type">Type of position lock</param>
    public OnlineMapsPositionRange(float minLat, float minLng, float maxLat, float maxLng, OnlineMapsPositionRangeType type = OnlineMapsPositionRangeType.center)
    {
        this.minLat = minLat;
        this.minLng = minLng;
        this.maxLat = maxLat;
        this.maxLng = maxLng;
        this.type = type;
    }

    /// <summary>
    /// Checks and limits geographic coordinates.
    /// </summary>
    /// <param name="position">Geographic coordinates</param>
    /// <returns>Geographical coordinates corresponding to the specified range.</returns>
    public Vector2 CheckAndFix(Vector2 position)
    {
        if (position.x < minLng) position.x = minLng;
        if (position.x > maxLng) position.x = maxLng;
        if (position.y < minLat) position.y = minLat;
        if (position.y > maxLat) position.y = maxLat;
        return position;
    }

    /// <summary>
    /// Checks and limits geographic coordinates.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    /// <returns>True - coordinate changed, False - in other cases.</returns>
    public bool CheckAndFix(ref double lng, ref double lat)
    {
        bool changed = false;
        if (lng < minLng)
        {
            lng = minLng;
            changed = true;
        }
        if (lng > maxLng)
        {
            lng = maxLng;
            changed = true;
        }
        if (lat < minLat)
        {
            lat = minLat;
            changed = true;
        }
        if (lat > maxLat)
        {
            lat = maxLat;
            changed = true;
        }
        return changed;
    }

    /// <summary>
    /// Checks whether the specified geographic coordinates in the range.
    /// </summary>
    /// <param name="position">Geographic coordinates</param>
    /// <returns>True - coordinates are in the range, False - in other cases.</returns>
    public bool InRange(Vector2 position)
    {
        if (position.x < minLng || position.x > maxLng) return false;
        if (position.y < minLat || position.y > maxLat) return false;
        return true;
    }
}