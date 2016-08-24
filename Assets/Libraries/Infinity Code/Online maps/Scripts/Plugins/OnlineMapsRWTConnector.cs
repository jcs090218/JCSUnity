/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// Real World Terrain Connector
/// </summary>
[AddComponentMenu("Infinity Code/Online Maps/Plugins/Real World Terrain Connector")]
public class OnlineMapsRWTConnector : MonoBehaviour 
{
    /// <summary>
    /// Connection mode
    /// </summary>
    public OnlineMapsRWTConnectorMode mode = OnlineMapsRWTConnectorMode.markerOnPosition;

    /// <summary>
    /// Which object should be displayed on the map.
    /// </summary>
    public OnlineMapsRWTConnectorPositionMode positionMode = OnlineMapsRWTConnectorPositionMode.transform;

    /// <summary>
    /// The texture of the marker to be displayed on the map.
    /// </summary>
    public Texture2D markerTexture;

    /// <summary>
    /// The label of the marker to be displayed on the map.
    /// </summary>
    public string markerLabel;

    /// <summary>
    /// Position, which must be shown on the map.
    /// </summary>
    public Vector3 scenePosition;

    /// <summary>
    /// Coordinates, which must be shown on the map.
    /// </summary>
    public Vector2 coordinates;

    /// <summary>
    /// Transform, which must be shown on the map.
    /// </summary>
    public Transform targetTransform;

#if RWT

    private RealWorldTerrainContainer rwt;
    private OnlineMapsMarker marker;
    private OnlineMaps api;

    /// <summary>
    /// Coordinates of selecteted object
    /// </summary>
    public Vector2 currentPosition
    {
        get
        {
            if (positionMode == OnlineMapsRWTConnectorPositionMode.coordinates) return coordinates;
            if (positionMode == OnlineMapsRWTConnectorPositionMode.scenePosition)
                return WorldPositionToCoordinates(scenePosition);
            if (positionMode == OnlineMapsRWTConnectorPositionMode.transform)
                return WorldPositionToCoordinates(targetTransform.position);
            return Vector2.zero;
        }
    }

    /// <summary>
    /// Converts position to geographic coordinates.
    /// </summary>
    /// <param name="position">World position</param>
    /// <returns>Geographic coordinates</returns>
    public Vector2 WorldPositionToCoordinates(Vector3 position)
    {
        RealWorldTerrainItem[,] terrains = rwt.terrains;

        foreach (RealWorldTerrainItem item in terrains)
        {
            Vector3 tp = item.terrain.transform.position;
            Vector3 size = item.terrainData.size;

            if (tp.x <= position.x && tp.z <= position.z && tp.x + size.x >= position.x && tp.z + size.z >= position.z)
            {
                float dx = item.bottomRight.x - item.topLeft.x;
                float dy = item.topLeft.y - item.bottomRight.y;

                float rx = (position.x - tp.x) / size.x;
                float rz = (position.z - tp.z) / size.z;

                float px = dx * rx;
                float py = dy * rz;

                return new Vector2(px + item.topLeft.x, py + item.bottomRight.y);
            }
        }

        return Vector2.zero;
    }

    private void Start()
    {
        rwt = GetComponent<RealWorldTerrainContainer>();
        if (rwt == null)
        {
            Debug.LogError("Real World Terrain Connector should be together c Real World Terrain Container.");
            DestroyImmediate(this);
            return;
        }

        if (positionMode == OnlineMapsRWTConnectorPositionMode.transform && targetTransform == null)
        {
            Debug.LogError("Target Transform is not specified.");
            DestroyImmediate(this);
            return;
        }

        api = OnlineMaps.instance;

        if (mode == OnlineMapsRWTConnectorMode.centerOnPosition)
        {
            api.position = currentPosition;
        }
        else if (mode == OnlineMapsRWTConnectorMode.markerOnPosition)
        {
            marker = api.AddMarker(currentPosition, markerTexture, markerLabel);
        }
    }

    private void Update()
    {
        if (mode == OnlineMapsRWTConnectorMode.markerOnPosition) marker.position = currentPosition;
        else if (mode == OnlineMapsRWTConnectorMode.centerOnPosition) api.position = currentPosition;
    }
#endif
}

/// <summary>
/// Connection mode
/// </summary>
public enum OnlineMapsRWTConnectorMode
{
    markerOnPosition,
    centerOnPosition
}

/// <summary>
/// Which object should be displayed on the map.
/// </summary>
public enum OnlineMapsRWTConnectorPositionMode
{
    transform,
    coordinates,
    scenePosition
}