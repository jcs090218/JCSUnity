/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

/// <summary>
/// Alignment of marker.
/// </summary>
public enum OnlineMapsAlign
{
    TopLeft,
    Top,
    TopRight,
    Left,
    Center,
    Right,
    BottomLeft,
    Bottom,
    BottomRight
}

/// <summary>
/// Buffer state
/// </summary>
public enum OnlineMapsBufferStatus
{
    wait,
    working,
    complete,
    start,
    disposed
}

/// <summary>
/// OnlineMaps events.
/// </summary>
public enum OnlineMapsEvents
{
    changedPosition,
    changedZoom
}

public enum OnlineMapsMarker2DMode
{
    flat,
    billboard
}

public enum OnlineMapsLocationServiceMarkerType
{
    twoD = 0,
    threeD = 1
}

/// <summary>
/// OnlineMaps provider.
/// </summary>
public enum OnlineMapsProviderEnum
{
    arcGis,
    google,
    nokia,
    mapQuest,
    virtualEarth,
    openStreetMap,
    sputnik,
    aMap,
    custom = 999
}

public enum OnlineMapsPositionRangeType
{
    center,
    border
}

/// <summary>
/// Status of the request to the Google Maps API.
/// </summary>
public enum OnlineMapsQueryStatus
{
    downloading,
    success,
    error,
    disposed
}

/// <summary>
/// Type of the request to the Google Maps API.
/// </summary>
public enum OnlineMapsQueryType
{
    none,
    location,
    direction,
    osm,
    places,
    autocomplete,
    elevation,
    geonames,
    placeDetails,
    ors,
    bingMapsLocation,
    nominatim
}

/// <summary>
/// Map redraw type.
/// </summary>
public enum OnlineMapsRedrawType
{
    full,
    area,
    move,
    none
}

/// <summary>
/// Where will draw the map.
/// </summary>
public enum OnlineMapsTarget
{
    texture,
    tileset
}

/// <summary>
/// Type of checking 2D markers on visibility.
/// </summary>
public enum OnlineMapsTilesetCheckMarker2DVisibility
{
    /// <summary>
    /// Will be checked only coordinates of markers. Faster.
    /// </summary>
    pivot,

    /// <summary>
    /// Will be checked all the border of marker. If the marker is located on the map at least one point, then it will be shown.
    /// </summary>
    bounds
}

/// <summary>
/// When need to show marker tooltip.
/// </summary>
public enum OnlineMapsShowMarkerTooltip
{
    onHover,
    onPress,
    always,
    none
}

/// <summary>
/// Source of map tiles.
/// </summary>
public enum OnlineMapsSource
{
    Online,
    Resources,
    ResourcesAndOnline
}

/// <summary>
/// Tile state
/// </summary>
public enum OnlineMapsTileStatus
{
    none,
    loading,
    loaded,
    loadedFromCache,
    error,
    disposed
}