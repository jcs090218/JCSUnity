/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// Class control the map for the Tileset.
/// Tileset - a dynamic mesh, created at runtime.
/// </summary>
[Serializable]
[AddComponentMenu("Infinity Code/Online Maps/Controls/Tileset")]
public class OnlineMapsTileSetControl : OnlineMapsControlBase3D
{
    /// <summary>
    /// Delegate to manually control the visibility of 2D markers.
    /// </summary>
    /// <param name="marker">Marker for which you need to check visibility.</param>
    /// <returns>True - if marker is visible, False - is hidden.</returns>
    public delegate bool CheckMarker2DVisibilityDelegate(OnlineMapsMarker marker);

    /// <summary>
    /// Delegate to manually control the order of 2D markers.
    /// </summary>
    /// <param name="marker">Marker for which you need to check order.</param>
    /// <returns>Y offset</returns>
    public delegate float GetFlatMarkerOffsetYDelegate(OnlineMapsMarker marker);

    /// <summary>
    /// The event, which occurs when the changed texture tile maps.
    /// </summary>
    public Action<OnlineMapsTile, Material> OnChangeMaterialTexture;

    /// <summary>
    /// Event to manually control the visibility of 2D markers.
    /// </summary>
    public CheckMarker2DVisibilityDelegate OnCheckMarker2DVisibility;

    /// <summary>
    /// Event, which intercepts the request to BingMaps Elevation API.
    /// </summary>
    public Action<Vector2, Vector2> OnGetElevation;

    /// <summary>
    /// Event to manually control the order of 2D markers.
    /// </summary>
    public GetFlatMarkerOffsetYDelegate OnGetFlatMarkerOffsetY;

    /// <summary>
    /// Event, which occurs when the smooth zoom is started.
    /// </summary>
    public Action OnSmoothZoomBegin;

    /// <summary>
    /// Event, which occurs when the smooth zoom is finish.
    /// </summary>
    public Action OnSmoothZoomFinish;

    /// <summary>
    /// Event, which occurs when the smooth zoom is process.
    /// </summary>
    public Action OnSmoothZoomProcess;

    /// <summary>
    /// Bing Maps API key
    /// </summary>
    public string bingAPI = "";

    /// <summary>
    /// Type of checking 2D markers on visibility.
    /// </summary>
    public OnlineMapsTilesetCheckMarker2DVisibility checkMarker2DVisibility = OnlineMapsTilesetCheckMarker2DVisibility.pivot;

    /// <summary>
    /// Type of collider: box - for performance, mesh - for elevation.
    /// </summary>
    public OnlineMapsColliderType colliderType = OnlineMapsColliderType.mesh;

    /// <summary>
    /// Container for drawing elements.
    /// </summary>
    public GameObject drawingsGameObject;

    /// <summary>
    /// Shader of drawing elements.
    /// </summary>
    public Shader drawingShader;

    /// <summary>
    /// Zoom levels, which will be shown the elevations.
    /// </summary>
    public OnlineMapsRange elevationZoomRange = new OnlineMapsRange(11, 20);

    /// <summary>
    /// Scale of elevation data.
    /// </summary>
    public float elevationScale = 1;

    /// <summary>
    /// IComparer instance for manual sorting of markers.
    /// </summary>
    public IComparer<OnlineMapsMarker> markerComparer;

    /// <summary>
    /// Material that will be used for marker.
    /// </summary>
    public Material markerMaterial;

    /// <summary>
    /// Shader of markers.
    /// </summary>
    public Shader markerShader;

    /// <summary>
    /// Specifies whether to use a smooth touch zoom.
    /// </summary>
    public bool smoothZoom = false;

    /// <summary>
    /// The minimum scale at smooth zoom.
    /// </summary>
    public float smoothZoomMinScale = float.MinValue;

    /// <summary>
    /// The maximum scale at smooth zoom.
    /// </summary>
    public float smoothZoomMaxScale = float.MaxValue;

    /// <summary>
    /// Indicates smooth zoom in process.
    /// </summary>
    public bool smoothZoomStarted = false;

    public Vector3 originalPosition;

    /// <summary>
    /// Mode smooth zoom.
    /// </summary>
    public OnlineMapsSmoothZoomMode smoothZoomMode = OnlineMapsSmoothZoomMode.target;

    /// <summary>
    /// Material that will be used for tile.
    /// </summary>
    public Material tileMaterial;

    /// <summary>
    /// Shader of map.
    /// </summary>
    public Shader tilesetShader;

    /// <summary>
    /// Specifies that you want to build a map with the elevetions.
    /// </summary>
    public bool useElevation = false;

    private bool _useElevation;

    private OnlineMapsVector2i _bufferPosition;

    private WWW elevationRequest;
    private Rect elevationRequestRect;
    private short[,] elevationData;
    private Rect elevationRect;
    private MeshCollider meshCollider;
    private bool ignoreGetElevation;
    private Mesh tilesetMesh;
    private int[] triangles;
    private Vector2[] uv;
    private Vector3[] vertices;

    private OnlineMapsVector2i elevationBufferPosition;

    private Vector2 smoothZoomPoint;
    private Vector3 smoothZoomOffset;
    private Vector3 smoothZoomHitPoint;
    private bool firstUpdate = true;
    private List<TilesetFlatMarker> usedMarkers;

    /// <summary>
    /// Singleton instance of OnlineMapsTileSetControl control.
    /// </summary>
    public new static OnlineMapsTileSetControl instance
    {
        get { return OnlineMapsControlBase.instance as OnlineMapsTileSetControl; }
    }

    private OnlineMapsVector2i bufferPosition
    {
        get
        {
            if (_bufferPosition == null)
            {
                const int s = OnlineMapsUtils.tileSize;
                int countX = api.width / s + 2;
                int countY = api.height / s + 2;

                double px, py;
                api.GetPosition(out px, out py);

                _bufferPosition = OnlineMapsUtils.LatLongToTile(px, py, api.zoom);
                _bufferPosition.x -= countX / 2;
                _bufferPosition.y -= countY / 2;

                int maxY = (2 << api.zoom) / 2;

                if (_bufferPosition.y < 0) _bufferPosition.y = 0;
                if (_bufferPosition.y >= maxY - countY - 1) _bufferPosition.y = maxY - countY - 1;
            }
            return _bufferPosition;
        }
    }

    protected override void AfterUpdate()
    {
        base.AfterUpdate();
        if (elevationRequest != null) CheckElevationRequest();
    }

    private void CheckElevationRequest()
    {
        if (elevationRequest == null || !elevationRequest.isDone) return;

        if (string.IsNullOrEmpty(elevationRequest.error))
        {
            elevationRect = elevationRequestRect;
            string response = elevationRequest.text;

            Match match = Regex.Match(response, "\"elevations\":\\[(.*?)]");
            if (match.Success)
            {
                short[] heights = match.Groups[1].Value.Split(new[] {','}).Select(v => short.Parse(v)).ToArray();
                elevationData = new short[32,32];

                for (int i = 0; i < heights.Length; i++)
                {
                    int x = i % 32;
                    int y = i / 32;
                    elevationData[x, y] = heights[i];
                }
            }

            UpdateControl();
        }
        else
        {
            Debug.LogWarning(elevationRequest.error);
        }
        elevationRequest = null;

        if (ignoreGetElevation) GetElevation();
    }

    public override void Clear2DMarkerInstances()
    {
        if (marker2DMode == OnlineMapsMarker2DMode.billboard)
        {
            DestroyImmediate(markersGameObject);
            markersGameObject = null;
        }
        else Clear2DMarkerBillboards();
    }

    public override float GetBestElevationYScale(Vector2 topLeftPosition, Vector2 bottomRightPosition)
    {
        Vector2 realDistance = OnlineMapsUtils.DistanceBetweenPoints(topLeftPosition, bottomRightPosition);
        return Mathf.Min(api.width / realDistance.x, api.height / realDistance.y) / 1000;
    }

    public override float GetBestElevationYScale(double tlx, double tly, double brx, double bry)
    {
        double dx, dy;
        OnlineMapsUtils.DistanceBetweenPoints(tlx, tly, brx, bry, out dx, out dy);
        return (float)Math.Min(api.width / dx, api.height / dy) / 1000;
    }

    public override Vector2 GetCoords(Vector2 position)
    {
        if (!HitTest()) return Vector2.zero;

        RaycastHit hit;
        if (!cl.Raycast(activeCamera.ScreenPointToRay(position), out hit, OnlineMapsUtils.maxRaycastDistance))
            return Vector2.zero;

        return GetCoordsByWorldPosition(hit.point);
    }

    public override bool GetCoords(out double lng, out double lat, Vector2 position)
    {
        lat = 0;
        lng = 0;

        if (!HitTest()) return false;

        RaycastHit hit;
        if (!cl.Raycast(activeCamera.ScreenPointToRay(position), out hit, OnlineMapsUtils.maxRaycastDistance)) return false;

        return GetCoordsByWorldPosition(out lng, out lat, hit.point);
    }

    /// <summary>
    /// Returns the geographical coordinates by world position.
    /// </summary>
    /// <param name="position">World position</param>
    /// <returns>Geographical coordinates or Vector2.zero</returns>
    public Vector2 GetCoordsByWorldPosition(Vector3 position)
    {
        Vector3 boundsSize = new Vector3(api.tilesetSize.x, 0, api.tilesetSize.y);
        boundsSize.Scale(transform.localScale);
        Vector3 size = new Vector3(0, 0, api.tilesetSize.y) - Quaternion.Inverse(transform.rotation) * (position - transform.position);

        size.x = size.x / boundsSize.x;
        size.z = size.z / boundsSize.z;

        Vector2 r = new Vector3((size.x - .5f), (size.z - .5f));

        int countX = api.width / OnlineMapsUtils.tileSize;
        int countY = api.height / OnlineMapsUtils.tileSize;

        double px, py;
        api.GetPosition(out px, out py);
        OnlineMapsUtils.LatLongToTiled(px, py, api.zoom, out px, out py);
        px += countX * r.x;
        py -= countY * r.y;
        OnlineMapsUtils.TileToLatLong(px, py, api.zoom, out px, out py);
        return new Vector2((float) px, (float) py);
    }

    /// <summary>
    /// Returns the geographical coordinates by world position.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    /// <param name="position">World position</param>
    /// <returns>True - success, False - otherwise.</returns>
    public bool GetCoordsByWorldPosition(out double lng, out double lat, Vector3 position)
    {
        Vector3 boundsSize = new Vector3(api.tilesetSize.x, 0, api.tilesetSize.y);
        boundsSize.Scale(transform.localScale);
        Vector3 size = new Vector3(0, 0, api.tilesetSize.y) - Quaternion.Inverse(transform.rotation) * (position - transform.position);

        size.x = size.x / boundsSize.x;
        size.z = size.z / boundsSize.z;

        Vector2 r = new Vector3((size.x - .5f), (size.z - .5f));

        int countX = api.width / OnlineMapsUtils.tileSize;
        int countY = api.height / OnlineMapsUtils.tileSize;

        double px, py;
        api.GetPosition(out px, out py);
        OnlineMapsUtils.LatLongToTiled(px, py, api.zoom, out px, out py);
        px += countX * r.x;
        py -= countY * r.y;
        OnlineMapsUtils.TileToLatLong(px, py, api.zoom, out lng, out lat);
        return true;
    }

    private void GetElevation()
    {
        ignoreGetElevation = true;

        if (elevationRequest != null) return;

        elevationBufferPosition = bufferPosition;
        ignoreGetElevation = false;

        const int s = OnlineMapsUtils.tileSize;
        int countX = api.width / s + 2;
        int countY = api.height / s + 2;

        Vector2 startCoords = OnlineMapsUtils.TileToLatLong(bufferPosition.x, bufferPosition.y, api.zoom);
        Vector2 endCoords = OnlineMapsUtils.TileToLatLong(bufferPosition.x + countX, bufferPosition.y + countY, api.zoom);

        elevationRequestRect = new Rect(startCoords.x, startCoords.y, endCoords.x - startCoords.x, endCoords.y - startCoords.y);

        if (OnGetElevation == null)
        {
            const string urlPattern =
                "https://dev.virtualearth.net/REST/v1/Elevation/Bounds?bounds={0},{1},{2},{3}&rows=32&cols=32&key={4}";
            string url = string.Format(urlPattern, endCoords.y, startCoords.x, startCoords.y, endCoords.x, bingAPI);
            elevationRequest = OnlineMapsUtils.GetWWW(url);
        }
        else OnGetElevation(startCoords, endCoords);
    }

    public override float GetElevationValue(float x, float z, float yScale, Vector2 topLeftPosition, Vector2 bottomRightPosition)
    {
        if (elevationData == null) return 0;

        x /= -api.tilesetSize.x;
        z /= api.tilesetSize.y;

        float cx = Mathf.Lerp(topLeftPosition.x, bottomRightPosition.x, x);
        float cz = Mathf.Lerp(topLeftPosition.y, bottomRightPosition.y, z);

        float rx = (cx - elevationRect.x) / elevationRect.width * 31;
        float ry = (cz - elevationRect.y) / elevationRect.height * 31;

        rx = Mathf.Clamp(rx, 0, 31);
        ry = Mathf.Clamp(ry, 0, 31);

        int x1 = (int)rx;
        int x2 = x1 + 1;
        int y1 = (int)ry;
        int y2 = y1 + 1;
        if (x2 > 31) x2 = 31;
        if (y2 > 31) y2 = 31;

        float p1 = Mathf.Lerp(elevationData[x1, 31 - y1], elevationData[x2, 31 - y1], rx - x1);
        float p2 = Mathf.Lerp(elevationData[x1, 31 - y2], elevationData[x2, 31 - y2], rx - x1);

        return Mathf.Lerp(p1, p2, ry - y1) * yScale * elevationScale;
    }

    public override float GetElevationValue(float x, float z, float yScale, double tlx, double tly, double brx, double bry)
    {
        if (elevationData == null) return 0;

        x = Mathf.Clamp01(x / -api.tilesetSize.x);
        z = Mathf.Clamp01(z / api.tilesetSize.y);

        double cx = (brx - tlx) * x + tlx;
        double cz = (bry - tly) * z + tly;

        float rx = (float)((cx - elevationRect.x) / elevationRect.width * 31);
        float ry = (float)((cz - elevationRect.y) / elevationRect.height * 31);

        rx = Mathf.Clamp(rx, 0, 31);
        ry = Mathf.Clamp(ry, 0, 31);

        int x1 = (int)rx;
        int x2 = x1 + 1;
        int y1 = (int)ry;
        int y2 = y1 + 1;
        if (x2 > 31) x2 = 31;
        if (y2 > 31) y2 = 31;

        float p1 = Mathf.Lerp(elevationData[x1, 31 - y1], elevationData[x2, 31 - y1], rx - x1);
        float p2 = Mathf.Lerp(elevationData[x1, 31 - y2], elevationData[x2, 31 - y2], rx - x1);

        return Mathf.Lerp(p1, p2, ry - y1) * yScale * elevationScale;
    }

    /// <summary>
    /// Returns the elevation value, based on the coordinates of the scene.
    /// </summary>
    /// <param name="x">Scene X.</param>
    /// <param name="z">Scene Z.</param>
    /// <param name="yScale">Scale factor for evevation value.</param>
    /// <param name="tlx">Top-left longitude of map.</param>
    /// <param name="tly">Top-left latitude of map.</param>
    /// <param name="brx">Bottom-right longitude of map.</param>
    /// <param name="bry">Bottom-right latitude of map.</param>
    /// <returns>Elevation value.</returns>
    public float GetElevationValue(double x, double z, float yScale, double tlx, double tly, double brx, double bry)
    {
        if (elevationData == null) return 0;

        x = x / -api.tilesetSize.x;
        z = z / api.tilesetSize.y;

        if (x < 0) x = 0;
        else if (x > 1) x = 1;

        if (z < 0) z = 0;
        else if (z > 1) z = 1;

        double cx = (brx - tlx) * x + tlx;
        double cz = (bry - tly) * z + tly;

        float rx = (float)((cx - elevationRect.x) / elevationRect.width * 31);
        float ry = (float)((cz - elevationRect.y) / elevationRect.height * 31);

        rx = Mathf.Clamp(rx, 0, 31);
        ry = Mathf.Clamp(ry, 0, 31);

        int x1 = (int)rx;
        int x2 = x1 + 1;
        int y1 = (int)ry;
        int y2 = y1 + 1;
        if (x2 > 31) x2 = 31;
        if (y2 > 31) y2 = 31;

        float p1 = Mathf.Lerp(elevationData[x1, 31 - y1], elevationData[x2, 31 - y1], rx - x1);
        float p2 = Mathf.Lerp(elevationData[x1, 31 - y2], elevationData[x2, 31 - y2], rx - x1);

        return Mathf.Lerp(p1, p2, ry - y1) * yScale * elevationScale;
    }

    /// <summary>
    /// Returns the maximum elevation for the current map.
    /// </summary>
    /// <param name="yScale">Best yScale.</param>
    /// <returns>Maximum elevation value.</returns>
    public float GetMaxElevationValue(float yScale)
    {
        if (elevationData == null) return 0;

        short value = short.MinValue;

        foreach (short el in elevationData)
        {
            if (el > value) value = el;
        } 

        return value * yScale * elevationScale;
    }

    /// <summary>
    /// Gets flat marker by screen position.
    /// </summary>
    /// <param name="screenPosition">Screen position.</param>
    /// <returns>Instance of marker.</returns>
    public OnlineMapsMarker GetMarkerFromScreen(Vector2 screenPosition)
    {
        if (usedMarkers == null || usedMarkers.Count == 0) return null;

        RaycastHit hit;
        if (cl.Raycast(activeCamera.ScreenPointToRay(screenPosition), out hit, OnlineMapsUtils.maxRaycastDistance))
        {
            foreach (TilesetFlatMarker flatMarker in usedMarkers)
            {
                if (flatMarker.Contains(hit.point, transform)) return flatMarker.marker;
            }
        }
        return null;
    }

    /// <summary>
    /// Converts geographical coordinates to position in world space.
    /// </summary>
    /// <param name="coords">Geographical coordinates.</param>
    /// <returns>Position in world space.</returns>
    public Vector3 GetWorldPosition(Vector2 coords)
    {
        Vector2 mapPosition = OnlineMapsControlBase.instance.GetPosition(coords);

        float px = -mapPosition.x / api.tilesetWidth * api.tilesetSize.x;
        float pz = mapPosition.y / api.tilesetHeight * api.tilesetSize.y;

        Vector3 offset = transform.rotation * new Vector3(px, 0, pz);
        offset.Scale(api.transform.localScale);

        return api.transform.position + offset;
    }

    /// <summary>
    /// Converts geographical coordinates to position in world space.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    /// <returns></returns>
    public Vector3 GetWorldPosition(double lng, double lat)
    {
        double mx, my;
        OnlineMapsControlBase.instance.GetPosition(lng, lat, out mx, out my);

        double px = -mx / api.tilesetWidth * api.tilesetSize.x;
        double pz = my / api.tilesetHeight * api.tilesetSize.y;

        Vector3 offset = transform.rotation * new Vector3((float)px, 0, (float)pz);
        offset.Scale(api.transform.localScale);

        return api.transform.position + offset;
    }

    /// <summary>
    /// Converts geographical coordinates to position in world space with elevation.
    /// </summary>
    /// <param name="coords">Geographical coordinates.</param>
    /// <param name="topLeftPosition">Coordinates of top-left corner of map.</param>
    /// <param name="bottomRightPosition">Coordinates of bottom-right corner of map.</param>
    /// <returns>Position in world space.</returns>
    public Vector3 GetWorldPositionWithElevation(Vector2 coords, Vector2 topLeftPosition, Vector2 bottomRightPosition)
    {
        Vector2 mapPosition = OnlineMapsControlBase.instance.GetPosition(coords);

        float px = -mapPosition.x / api.tilesetWidth * api.tilesetSize.x;
        float pz = mapPosition.y / api.tilesetHeight * api.tilesetSize.y;

        float y = GetElevationValue(-mapPosition.x, mapPosition.y, GetBestElevationYScale(topLeftPosition, bottomRightPosition), topLeftPosition, bottomRightPosition);

        Vector3 offset = transform.rotation * new Vector3(px, y, pz);
        offset.Scale(api.transform.localScale);

        return api.transform.position + offset;
    }

    /// <summary>
    /// Converts geographical coordinates to position in world space with elevation.
    /// </summary>
    /// <param name="coords">Geographical coordinates.</param>
    /// <param name="tlx">Top-left longitude.</param>
    /// <param name="tly">Top-left latitude.</param>
    /// <param name="brx">Bottom-right longitude.</param>
    /// <param name="bry">Bottom-right latitude.</param>
    /// <returns>Position in world space.</returns>
    public Vector3 GetWorldPositionWithElevation(Vector2 coords, double tlx, double tly, double brx, double bry)
    {
        Vector2 mapPosition = GetPosition(coords);

        float px = -mapPosition.x / api.tilesetWidth * api.tilesetSize.x;
        float pz = mapPosition.y / api.tilesetHeight * api.tilesetSize.y;

        float y = GetElevationValue(-mapPosition.x, mapPosition.y, GetBestElevationYScale(tlx, tly, brx, bry), tlx, tly, brx, bry);

        Vector3 offset = transform.rotation * new Vector3(px, y, pz);
        offset.Scale(api.transform.localScale);

        return api.transform.position + offset;
    }

    /// <summary>
    /// Converts geographical coordinates to position in world space with elevation.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Laatitude</param>
    /// <param name="tlx">Top-left longitude.</param>
    /// <param name="tly">Top-left latitude.</param>
    /// <param name="brx">Bottom-right longitude.</param>
    /// <param name="bry">Bottom-right latitude.</param>
    /// <returns>Position in world space.</returns>
    public Vector3 GetWorldPositionWithElevation(double lng, double lat, double tlx, double tly, double brx, double bry)
    {
        double mx, my;
        GetPosition(lng, lat, out mx, out my);

        double px = -mx / api.tilesetWidth * api.tilesetSize.x;
        double pz = my / api.tilesetHeight * api.tilesetSize.y;

        float y = GetElevationValue(-mx, my, GetBestElevationYScale(tlx, tly, brx, bry), tlx, tly, brx, bry);

        Vector3 offset = transform.rotation * new Vector3((float)px, y, (float)pz);
        offset.Scale(api.transform.localScale);

        return api.transform.position + offset;
    }

    protected override bool HitTest()
    {
#if NGUI
        if (UICamera.Raycast(Input.mousePosition)) return false;
#endif
        RaycastHit hit;
        return cl.Raycast(activeCamera.ScreenPointToRay(Input.mousePosition), out hit, OnlineMapsUtils.maxRaycastDistance);
    }

    private void InitDrawingsMesh()
    {
        drawingsGameObject = new GameObject("Drawings");
        drawingsGameObject.transform.parent = transform;
        drawingsGameObject.transform.localPosition = new Vector3(0, OnlineMaps.instance.tilesetSize.magnitude / 4344, 0);
        drawingsGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void InitMapMesh()
    {
        _useElevation = useElevation;

        Shader tileShader = tilesetShader;

        MeshFilter meshFilter;
        BoxCollider boxCollider = null;

        if (tilesetMesh == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();

            if (colliderType == OnlineMapsColliderType.mesh) meshCollider = gameObject.AddComponent<MeshCollider>();
            else if (colliderType == OnlineMapsColliderType.box) boxCollider = gameObject.AddComponent<BoxCollider>();

            tilesetMesh = new Mesh {name = "Tileset"};
        }
        else
        {
            meshFilter = GetComponent<MeshFilter>();
            tilesetMesh.Clear();
            elevationData = null;
            elevationRequest = null;
            if (useElevation)
            {
                ignoreGetElevation = false;
            }
        }

        int w1 = api.tilesetWidth / OnlineMapsUtils.tileSize;
        int h1 = api.tilesetHeight / OnlineMapsUtils.tileSize;

        int subMeshVX = 1;
        int subMeshVZ = 1;

        if (useElevation)
        {
            if (w1 < 32) subMeshVX = 32 % w1 == 0 ? 32 / w1 : 32 / w1 + 1;
            if (h1 < 32) subMeshVZ = 32 % h1 == 0 ? 32 / h1 : 32 / h1 + 1;
        }

        Vector2 subMeshSize = new Vector2(api.tilesetSize.x / w1, api.tilesetSize.y / h1);

        int w = w1 + 2;
        int h = h1 + 2;

        vertices = new Vector3[w * h * subMeshVX * subMeshVZ * 4];
        uv = new Vector2[w * h * subMeshVX * subMeshVZ * 4];
        Vector3[] normals = new Vector3[w * h * subMeshVX * subMeshVZ * 4];
        Material[] materials = new Material[w * h];
        tilesetMesh.subMeshCount = w * h;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                InitMapSubMesh(ref normals, x, y, w, h, subMeshSize, subMeshVX, subMeshVZ);
            }
        }

        tilesetMesh.vertices = vertices;
        tilesetMesh.uv = uv;
        tilesetMesh.normals = normals;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                InitMapSubMeshTriangles(ref materials, x, y, w, h, subMeshVX, subMeshVZ, tileShader);
            }
        }

        triangles = null;

        gameObject.GetComponent<Renderer>().materials = materials;

        tilesetMesh.MarkDynamic();
        tilesetMesh.RecalculateBounds();
        meshFilter.sharedMesh = tilesetMesh;

        if (meshCollider != null) meshCollider.sharedMesh = Instantiate(tilesetMesh) as Mesh;
        else if (boxCollider != null)
        {
            boxCollider.center = new Vector3(-api.tilesetSize.x / 2, 0, api.tilesetSize.y / 2);
            boxCollider.size = new Vector3(api.tilesetSize.x, 0, api.tilesetSize.y);
        }

        UpdateMapMesh();
    }

    private void InitMapSubMesh(ref Vector3[] normals, int x, int y, int w, int h, Vector2 subMeshSize, int subMeshVX, int subMeshVZ)
    {
        int i = (x + y * w) * subMeshVX * subMeshVZ * 4;

        Vector2 cellSize = new Vector2(subMeshSize.x / subMeshVX, subMeshSize.y / subMeshVZ);

        float sx = (x > 0 && x < w - 1) ? cellSize.x : 0;
        float sy = (y > 0 && y < h - 1) ? cellSize.y : 0;

        float nextY = subMeshSize.y * (y - 1);

        float uvX = 1f / subMeshVX;
        float uvZ = 1f / subMeshVZ;

        for (int ty = 0; ty < subMeshVZ; ty++)
        {
            float nextX = -subMeshSize.x * (x - 1);

            for (int tx = 0; tx < subMeshVX; tx++)
            {
                int ci = (tx + ty * subMeshVX) * 4 + i;

                vertices[ci] = new Vector3(nextX, 0, nextY);
                vertices[ci + 1] = new Vector3(nextX - sx, 0, nextY);
                vertices[ci + 2] = new Vector3(nextX - sx, 0, nextY + sy);
                vertices[ci + 3] = new Vector3(nextX, 0, nextY + sy);
                
                uv[ci] = new Vector2(1 - uvX * (tx + 1), 1 - uvZ * ty);
                uv[ci + 1] = new Vector2(1 - uvX * tx, 1 - uvZ * ty);
                uv[ci + 2] = new Vector2(1 - uvX * tx, 1 - uvZ * (ty + 1));
                uv[ci + 3] = new Vector2(1 - uvX * (tx + 1), 1 - uvZ * (ty + 1));

                normals[ci] = Vector3.up;
                normals[ci + 1] = Vector3.up;
                normals[ci + 2] = Vector3.up;
                normals[ci + 3] = Vector3.up;

                nextX -= sx;
            }

            nextY += sy;
        }
    }

    private void InitMapSubMeshTriangles(ref Material[] materials, int x, int y, int w, int h, int subMeshVX, int subMeshVZ, Shader tileShader)
    {
        if (triangles == null) triangles = new int[subMeshVX * subMeshVZ * 6];
        int i = (x + y * w) * subMeshVX * subMeshVZ * 4;

        for (int ty = 0; ty < subMeshVZ; ty++)
        {
            for (int tx = 0; tx < subMeshVX; tx++)
            {
                int ci = (tx + ty * subMeshVX) * 4 + i;
                int ti = (tx + ty * subMeshVX) * 6;

                triangles[ti] = ci;
                triangles[ti + 1] = ci + 1;
                triangles[ti + 2] = ci + 2;
                triangles[ti + 3] = ci;
                triangles[ti + 4] = ci + 2;
                triangles[ti + 5] = ci + 3;
            }
        }

        tilesetMesh.SetTriangles(triangles, x + y * w);
        Material material;

        if (tileMaterial != null) material = (Material)Instantiate(tileMaterial);
        else material = new Material(tileShader);

        if (api.defaultTileTexture != null) material.mainTexture = api.defaultTileTexture;
        materials[x + y * w] = material;
    }

    public override void OnAwakeBefore()
    {
        base.OnAwakeBefore();

        api = GetComponent<OnlineMaps>();

        InitMapMesh();

        if (useElevation) GetElevation();
    }

    protected override void OnDestroyLate()
    {
        base.OnDestroyLate();

        if (drawingsGameObject != null) DestroyImmediate(drawingsGameObject);
        drawingsGameObject = null;
        elevationData = null;
        elevationRequest = null;
        meshCollider = null;
        tilesetMesh = null;
        triangles = null;
        uv = null;
        vertices = null;
    }

    public override OnlineMapsXML SaveSettings(OnlineMapsXML parent)
    {
        OnlineMapsXML element = base.SaveSettings(parent);
        element.Create("CheckMarker2DVisibility", (int) checkMarker2DVisibility);
        element.Create("SmoothZoom", smoothZoom);
        element.Create("UseElevation", useElevation);
        element.Create("TileMaterial", tileMaterial);
        element.Create("TileShader", tilesetShader);
        element.Create("DrawingShader", drawingShader);
        element.Create("MarkerMaterial", markerMaterial);
        element.Create("MarkerShader", markerShader);
        return element;
    }

    /// <summary>
    /// Allows you to set the current values ​​of elevation.
    /// </summary>
    /// <param name="data">Elevation data [32x32]</param>
    public void SetElevationData(short[,] data)
    {
        elevationData = data;
        elevationRect = elevationRequestRect;
        UpdateControl();
    }

    public override void UpdateControl()
    {
        base.UpdateControl();

        _bufferPosition = null;

        if (OnlineMapsTile.tiles == null) return;

        if (useElevation != _useElevation)
        {
            elevationBufferPosition = null;
            elevationRect = default(Rect);
            triangles = null;
            InitMapMesh();
        }
        UpdateMapMesh();

        if (drawingsGameObject == null) InitDrawingsMesh();
        foreach (OnlineMapsDrawingElement drawingElement in api.drawingElements)
        {
            drawingElement.DrawOnTileset(this);
        }

        if (marker2DMode == OnlineMapsMarker2DMode.flat) UpdateMarkersMesh();
    }

    protected override void UpdateGestureZoom()
    {
        if (!smoothZoom)
        {
            base.UpdateGestureZoom();
            return;
        }

        if (!allowUserControl) return;

        if (Input.touchCount == 2)
        {
            Vector2 p1 = Input.GetTouch(0).position;
            Vector2 p2 = Input.GetTouch(1).position;
            float distance = Vector2.Distance(p1, p2);

            Vector2 center = Vector2.Lerp(p1, p2, 0.5f);

            if (!smoothZoomStarted)
            {
                smoothZoomPoint = center;

                RaycastHit hit;
                if (!cl.Raycast(activeCamera.ScreenPointToRay(center), out hit, OnlineMapsUtils.maxRaycastDistance)) return;
                
                if (smoothZoomMode == OnlineMapsSmoothZoomMode.target)
                {
                    smoothZoomHitPoint = hit.point;
                }
                else
                {
                    smoothZoomHitPoint = transform.position + transform.rotation * new Vector3(api.tilesetSize.x / -2, 0, api.tilesetSize.y / 2);
                }

                originalPosition = transform.position;
                smoothZoomOffset = originalPosition - smoothZoomHitPoint;
                smoothZoomOffset.y = 0;
                smoothZoomOffset.x /= -api.tilesetWidth;
                smoothZoomOffset.z /= -api.tilesetHeight;

                smoothZoomStarted = true;
                isMapDrag = false;
                waitZeroTouches = true;

                if (OnSmoothZoomBegin != null) OnSmoothZoomBegin();
            }
            else
            {
                RaycastHit hit;
                if (!cl.Raycast(activeCamera.ScreenPointToRay(center), out hit, OnlineMapsUtils.maxRaycastDistance)) return;

                float scale = 1;

                if (Mathf.Abs(distance - lastGestureDistance) > 2)
                {
                    if (!invertTouchZoom) scale = distance / lastGestureDistance;
                    else scale = lastGestureDistance / distance;
                }

                transform.localScale *= scale;
                if (transform.localScale.x < smoothZoomMinScale) transform.localScale = new Vector3(smoothZoomMinScale, smoothZoomMinScale, smoothZoomMinScale);
                else if (transform.localScale.x > smoothZoomMaxScale) transform.localScale = new Vector3(smoothZoomMaxScale, smoothZoomMaxScale, smoothZoomMaxScale);

                Vector3 p = new Vector3(api.tilesetWidth * (transform.localScale.x - 1) * smoothZoomOffset.x, 0, api.tilesetHeight * (transform.localScale.z - 1) * smoothZoomOffset.z);
                transform.position = originalPosition - p;

                OnGestureZoom(p1, p2);
            }

            lastGestureDistance = distance;
            lastGestureCenter = center;

            if (OnSmoothZoomProcess != null) OnSmoothZoomProcess();
        }
        else
        {
            if (smoothZoomStarted)
            {
                float s = transform.localScale.x;
                int offset = Mathf.RoundToInt(s > 1 ? s - 1 : -1 / s + 1);

                transform.position = originalPosition;
                transform.localScale = Vector3.one;
                smoothZoomStarted = false;
                lastGestureDistance = 0;
                lastGestureCenter = Vector2.zero;

                if (offset != 0) ZoomOnPoint(offset, smoothZoomPoint);
                if (OnSmoothZoomFinish != null) OnSmoothZoomFinish();
            }
        }
    }

    private void UpdateMapMesh()
    {
        if (useElevation && !ignoreGetElevation && elevationBufferPosition != bufferPosition && api.zoom > 10) GetElevation();

        int w1 = api.tilesetWidth / OnlineMapsUtils.tileSize;
        int h1 = api.tilesetHeight / OnlineMapsUtils.tileSize;

        int subMeshVX = 1;
        int subMeshVZ = 1;

        if (useElevation)
        {
            if (w1 < 32) subMeshVX = (32 % w1 == 0) ? 32 / w1 : 32 / w1 + 1;
            if (h1 < 32) subMeshVZ = (32 % h1 == 0) ? 32 / h1 : 32 / h1 + 1;
        }

        double subMeshSizeX = (double)api.tilesetSize.x / w1;
        double subMeshSizeY = (double)api.tilesetSize.y / h1;

        double tlx, tly, brx, bry;
        api.GetTopLeftPosition(out tlx, out tly);
        api.GetBottomRightPosition(out brx, out bry);

        double tlpx, tlpy;

        OnlineMapsUtils.LatLongToTiled(tlx, tly, api.zoom, out tlpx, out tlpy);
        double posX = tlpx - bufferPosition.x;
        double posY = tlpy - bufferPosition.y;

        int maxX = 1 << api.zoom;
        if (posX >= maxX) posX -= maxX;
        
        double startPosX = subMeshSizeX * posX;
        double startPosZ = -subMeshSizeY * posY;

        float yScale = GetBestElevationYScale(tlx, tly, brx, bry);

        int w = w1 + 2;
        int h = h1 + 2;

        Material[] materials = rendererInstance.materials;

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                UpdateMapSubMesh(x, y, w, h, subMeshSizeX, subMeshSizeY, subMeshVX, subMeshVZ, startPosX, startPosZ, yScale, tlx, tly, brx, bry, materials);
            }
        }

        tilesetMesh.vertices = vertices;
        tilesetMesh.uv = uv;

        //for (int i = 0; i < tilesetMesh.subMeshCount; i++) tilesetMesh.SetTriangles(tilesetMesh.GetTriangles(i), i);

        tilesetMesh.RecalculateBounds();

        if (meshCollider != null && ((useElevation && elevationZoomRange.InRange(api.zoom)) || firstUpdate))
        {
            meshCollider.sharedMesh = Instantiate(tilesetMesh) as Mesh;
            firstUpdate = false;
        }

    }

    private void UpdateMapSubMesh(int x, int y, int w, int h, double subMeshSizeX, double subMeshSizeY, int subMeshVX, int subMeshVZ, double startPosX, double startPosZ, float yScale, double tlx, double tly, double brx, double bry, Material[] materials)
    {
        int mi = x + y * w;
        int i = mi * subMeshVX * subMeshVZ * 4;

        double cellSizeX = subMeshSizeX / subMeshVX;
        double cellSizeY = subMeshSizeY / subMeshVZ;

        double uvX = 1.0 / subMeshVX;
        double uvZ = 1.0 / subMeshVZ;

        int bx = x + bufferPosition.x;
        int by = y + bufferPosition.y;

        int maxX = 1 << api.zoom;

        if (bx >= maxX) bx -= maxX;
        if (bx < 0) bx += maxX;

        OnlineMapsTile tile = null;

        lock (OnlineMapsTile.tiles)
        {
            foreach (OnlineMapsTile t in OnlineMapsTile.tiles)
            {
                if (t.zoom == api.zoom && t.x == bx && t.y == by)
                {
                    tile = t;
                    break;
                }
            }
        }

        OnlineMapsTile currentTile = tile;
        Texture tileTexture = (tile != null)? tile.texture: null;

        bool sendEvent = true;

        Vector2 offset = Vector2.zero;
        float scale = 1;

        if (tile != null)
        {
            while (tileTexture == null && currentTile.parent != null)
            {
                int s = 2 << (tile.zoom - currentTile.zoom);
                scale = 1f / s;
                offset.x = (tile.x % s) * scale;
                offset.y = (s - tile.y % s - 1) * scale;

                currentTile = currentTile.parent;
                tileTexture = currentTile.texture;

                sendEvent = false;
            }

            if (tileTexture == null)
            {
                scale = 1;
                offset = Vector2.zero;
            }
        }

        bool needGetElevation = useElevation && elevationData != null && elevationZoomRange.InRange(api.zoom);

        for (int ty = 0; ty < subMeshVZ; ty++)
        {
            double uvY1 = 1 - uvZ * ty;
            double uvY2 = 1 - uvZ * (ty + 1);

            double z1 = startPosZ + y * subMeshSizeY + ty * cellSizeY;
            double z2 = z1 + cellSizeY;

            if (z1 < 0) z1 = 0;
            if (z1 > api.tilesetSize.y) z1 = api.tilesetSize.y;

            if (z2 < 0) z2 = 0;
            if (z2 > api.tilesetSize.y) z2 = api.tilesetSize.y;

            if (z1 == 0 && z2 > 0) uvY1 = (uvY2 - uvY1) * (1 - z2 / cellSizeY) + uvY1;
            else if (z1 < api.tilesetSize.y && z2 == api.tilesetSize.y) uvY2 = (uvY2 - uvY1) * ((api.tilesetSize.y - z1) / cellSizeY) + uvY1;

            uvY1 = uvY1 * scale + offset.y;
            uvY2 = uvY2 * scale + offset.y;

            for (int tx = 0; tx < subMeshVX; tx++)
            {
                double uvX1 = uvX * tx;
                double uvX2 = uvX * (tx + 1);

                double x1 = startPosX - x * subMeshSizeX - tx * cellSizeX;
                double x2 = x1 - cellSizeX;
                
                if (x1 > 0) x1 = 0;
                if (x1 < -api.tilesetSize.x) x1 = -api.tilesetSize.x;

                if (x2 > 0) x2 = 0;
                if (x2 < -api.tilesetSize.x) x2 = -api.tilesetSize.x;

                if (x1 == 0 && x2 < 0) uvX1 = (uvX1 - uvX2) * (-x2 / cellSizeX) + uvX2;
                else if (x1 > -api.tilesetSize.x && x2 == -api.tilesetSize.x) uvX2 = (uvX1 - uvX2) * (1 - (x1 + api.tilesetSize.x) / cellSizeX) + uvX2;

                uvX1 = uvX1 * scale + offset.x;
                uvX2 = uvX2 * scale + offset.x;

                float y1 = 0;
                float y2 = 0;
                float y3 = 0;
                float y4 = 0;

                if (needGetElevation)
                {
                    y1 = GetElevationValue(x1, z1, yScale, tlx, tly, brx, bry);
                    y2 = GetElevationValue(x2, z1, yScale, tlx, tly, brx, bry);
                    y3 = GetElevationValue(x2, z2, yScale, tlx, tly, brx, bry);
                    y4 = GetElevationValue(x1, z2, yScale, tlx, tly, brx, bry);
                }

                int ci = (tx + ty * subMeshVX) * 4 + i;

                float fx1 = (float) x1;
                float fx2 = (float) x2;
                float fz1 = (float) z1;
                float fz2 = (float) z2;

                float fux1 = (float) uvX1;
                float fux2 = (float) uvX2;
                float fuy1 = (float) uvY1;
                float fuy2 = (float) uvY2;

                vertices[ci] = new Vector3(fx1, y1, fz1);
                vertices[ci + 1] = new Vector3(fx2, y2, fz1);
                vertices[ci + 2] = new Vector3(fx2, y3, fz2);
                vertices[ci + 3] = new Vector3(fx1, y4, fz2);

                uv[ci] = new Vector2(fux1, fuy1);
                uv[ci + 1] = new Vector2(fux2, fuy1);
                uv[ci + 2] = new Vector2(fux2, fuy2);
                uv[ci + 3] = new Vector2(fux1, fuy2);
            }
        }

        Material material = materials[mi];

        if (tile != null)
        {
            if (tileTexture == null && api.defaultTileTexture != null)
            {
                tileTexture = api.defaultTileTexture;
                sendEvent = false;
            }

            if (material.mainTexture != tileTexture)
            {
                material.mainTexture = tileTexture;
                if (sendEvent && OnChangeMaterialTexture != null) OnChangeMaterialTexture(tile, material);
            }
            if (material.GetTexture("_TrafficTex") != tile.trafficTexture) material.SetTexture("_TrafficTex", tile.trafficTexture);
        }
        else
        {
            material.mainTexture = null;
            material.SetTexture("_TrafficTex", null);
        }
    }

    private void UpdateMarkersMesh()
    {
        if (markersGameObject == null) InitMarkersMesh();

        double tlx, tly, brx, bry;
        api.GetTopLeftPosition(out tlx, out tly);
        api.GetBottomRightPosition(out brx, out bry);
        if (brx < tlx) brx += 360;

        int maxX = 1 << api.zoom;

        double px, py;
        OnlineMapsUtils.LatLongToTiled(tlx, tly, api.zoom, out px, out py);

        List<Vector3> markersVerticles = new List<Vector3>();

        float yScale = GetBestElevationYScale(tlx, tly, brx, bry);

        float cx = -api.tilesetSize.x / api.tilesetWidth;
        float cy = api.tilesetSize.y / api.tilesetHeight;

        usedMarkers = new List<TilesetFlatMarker>();
        List<Texture> usedTextures = new List<Texture> { api.defaultMarkerTexture };
        List<List<int>> usedTexturesMarkerIndex = new List<List<int>> { new List<int>() };

        int usedMarkersCount = 0;

        Matrix4x4 matrix = new Matrix4x4();

        Bounds tilesetBounds = new Bounds(new Vector3(api.tilesetSize.x / -2, 0, api.tilesetSize.y / 2), new Vector3(api.tilesetSize.x, 0, api.tilesetSize.y));

        OnlineMapsMarker[] markers = api.markers.Where(m => m.enabled && m.range.InRange(api.zoom)).ToArray();
        float[] offsets = null;
        bool useOffsetY = false;

        if (markerComparer != null)
        {
            markers = markers.OrderBy(m => m, markerComparer).ToArray();
        }
        else
        {
            markers = markers.OrderBy(m => m.position.y).ToArray();
            useOffsetY = OnGetFlatMarkerOffsetY != null;
            offsets = new float[markers.Length];

            if (useOffsetY)
            {
                TilesetSortedMarker[] sortedMarkers = new TilesetSortedMarker[markers.Length];
                for (int i = 0; i < markers.Length; i++)
                {
                    sortedMarkers[i] = new TilesetSortedMarker
                    {
                        marker = markers[i],
                        offset = OnGetFlatMarkerOffsetY(markers[i])
                    };
                }
                sortedMarkers = sortedMarkers.OrderBy(m => m.offset).ToArray();

                for (int i = 0; i < sortedMarkers.Length; i++)
                {
                    markers[i] = sortedMarkers[i].marker;
                    offsets[i] = sortedMarkers[i].offset;
                }
            }
        }

        for (int i = 0; i < markers.Length; i++)
        {
            OnlineMapsMarker marker = markers[i];
            float mx = marker.position.x;

            if (OnCheckMarker2DVisibility != null)
            {
                if (!OnCheckMarker2DVisibility(marker)) continue;
            }
            else if (checkMarker2DVisibility == OnlineMapsTilesetCheckMarker2DVisibility.pivot)
            {
                if (!(((mx > tlx && mx < brx) || (mx + 360 > tlx && mx + 360 < brx) ||
                       (mx - 360 > tlx && mx - 360 < brx)) &&
                      marker.position.y < tly && marker.position.y > bry)) continue;
            }

            Vector2 offset = marker.GetAlignOffset();
            offset *= marker.scale;

            float fx, fy;
            OnlineMapsUtils.LatLongToTilef(marker.position, api.zoom, out fx, out fy);

            fx = fx - (float)px;
            if (fx < -maxX / 2) fx += maxX;
            if (fx > maxX / 2) fx -= maxX;
            fx = fx * OnlineMapsUtils.tileSize - offset.x;
            fy = (fy - (float)py) * OnlineMapsUtils.tileSize - offset.y;

            if (marker.texture == null) marker.texture = api.defaultMarkerTexture;

            float markerWidth = marker.texture.width * marker.scale;
            float markerHeight = marker.texture.height * marker.scale;

            float rx1 = fx * cx;
            float ry1 = fy * cy;
            float rx2 = (fx + markerWidth) * cx;
            float ry2 = (fy + markerHeight) * cy;

            Vector3 center = new Vector3((fx + offset.x) * cx, 0, (fy + offset.y) * cy);

            Vector3 p1 = new Vector3(rx1 - center.x, 0, ry1 - center.z);
            Vector3 p2 = new Vector3(rx2 - center.x, 0, ry1 - center.z);
            Vector3 p3 = new Vector3(rx2 - center.x, 0, ry2 - center.z);
            Vector3 p4 = new Vector3(rx1 - center.x, 0, ry2 - center.z);

            float angle = Mathf.Repeat(marker.rotation, 1) * 360;

            if (angle != 0)
            {
                matrix.SetTRS(Vector3.zero, Quaternion.Euler(0, angle, 0), Vector3.one);

                p1 = matrix.MultiplyPoint(p1) + center;
                p2 = matrix.MultiplyPoint(p2) + center;
                p3 = matrix.MultiplyPoint(p3) + center;
                p4 = matrix.MultiplyPoint(p4) + center;
            }
            else
            {
                p1 += center;
                p2 += center;
                p3 += center;
                p4 += center;
            }

            if (checkMarker2DVisibility == OnlineMapsTilesetCheckMarker2DVisibility.bounds)
            {
                Vector3 markerCenter = (p2 + p4) / 2;
                Vector3 markerSize = p4 - p2;
                if (!tilesetBounds.Intersects(new Bounds(markerCenter, markerSize))) continue;
            }

            float y = GetElevationValue((rx1 + rx2) / 2, (ry1 + ry2) / 2, yScale, tlx, tly, brx, bry);
            float yOffset = (useOffsetY) ? offsets[i] : 0;

            p1.y = p2.y = p3.y = p4.y = y + yOffset;

            markersVerticles.Add(p1);
            markersVerticles.Add(p2);
            markersVerticles.Add(p3);
            markersVerticles.Add(p4);

            usedMarkers.Add(new TilesetFlatMarker(marker, p1 + transform.position, p3 + transform.position));

            if (marker.texture == api.defaultMarkerTexture)
                usedTexturesMarkerIndex[0].Add(usedMarkersCount);
            else
            {
                int textureIndex = usedTextures.IndexOf(marker.texture);
                if (textureIndex != -1) usedTexturesMarkerIndex[textureIndex].Add(usedMarkersCount);
                else
                {
                    usedTextures.Add(marker.texture);
                    usedTexturesMarkerIndex.Add(new List<int>());
                    usedTexturesMarkerIndex[usedTexturesMarkerIndex.Count - 1].Add(usedMarkersCount);
                }
            }

            usedMarkersCount++;
        }

        Vector2[] markersUV = new Vector2[markersVerticles.Count];
        Vector3[] markersNormals = new Vector3[markersVerticles.Count];

        Vector2 uvp1 = new Vector2(1, 1);
        Vector2 uvp2 = new Vector2(0, 1);
        Vector2 uvp3 = new Vector2(0, 0);
        Vector2 uvp4 = new Vector2(1, 0);

        for (int i = 0; i < usedMarkersCount; i++)
        {
            int vi = i * 4;
            markersNormals[vi] = Vector3.up;
            markersNormals[vi + 1] = Vector3.up;
            markersNormals[vi + 2] = Vector3.up;
            markersNormals[vi + 3] = Vector3.up;

            markersUV[vi] = uvp2;
            markersUV[vi + 1] = uvp1;
            markersUV[vi + 2] = uvp4;
            markersUV[vi + 3] = uvp3;
        }

        markersMesh.Clear();
        markersMesh.vertices = markersVerticles.ToArray();
        markersMesh.uv = markersUV;
        markersMesh.normals = markersNormals;

        if (markersRenderer.materials.Length != usedTextures.Count) markersRenderer.materials = new Material[usedTextures.Count];

        markersMesh.subMeshCount = usedTextures.Count;

        for (int i = 0; i < usedTextures.Count; i++)
        {
            int markerCount = usedTexturesMarkerIndex[i].Count;
            int[] markersTriangles = new int[markerCount * 6];

            for (int j = 0; j < markerCount; j++)
            {
                int vi = usedTexturesMarkerIndex[i][j] * 4;
                int vj = j * 6;

                markersTriangles[vj + 0] = vi;
                markersTriangles[vj + 1] = vi + 1;
                markersTriangles[vj + 2] = vi + 2;
                markersTriangles[vj + 3] = vi;
                markersTriangles[vj + 4] = vi + 2;
                markersTriangles[vj + 5] = vi + 3;
            }

            markersMesh.SetTriangles(markersTriangles, i);

            Material material = markersRenderer.materials[i];
            if (material == null)
            {
                if (markerMaterial != null) material = markersRenderer.materials[i] = new Material(markerMaterial);
                else material = markersRenderer.materials[i] = new Material(markerShader);
            }

            if (material.mainTexture != usedTextures[i])
            {
                if (markerMaterial != null)
                {
                    material.shader = markerMaterial.shader;
                    material.CopyPropertiesFromMaterial(markerMaterial);
                }
                else
                {
                    material.shader = markerShader;
                    material.color = Color.white;
                }
                material.SetTexture("_MainTex", usedTextures[i]);
            }
        }
    }

    internal class TilesetFlatMarker
    {
        public OnlineMapsMarker marker;
        public Rect rect;

        public TilesetFlatMarker(OnlineMapsMarker marker, Vector3 p1, Vector3 p2)
        {
            this.marker = marker;
            rect = new Rect(p1.x, p1.z, p1.x - p2.x, p2.z - p1.z);
            rect.x -= rect.width;
        }

        public bool Contains(Vector3 point, Transform transform)
        {
            Vector3 p = Quaternion.Inverse(transform.rotation) * (point - transform.position) + transform.position;
            return rect.Contains(new Vector2(p.x, p.z));
        }
    }

    internal class TilesetSortedMarker
    {
        public OnlineMapsMarker marker;
        public float offset;
    }

    /// <summary>
    /// Type of tileset map collider.
    /// </summary>
    public enum OnlineMapsColliderType
    {
        box,
        mesh
    }

    /// <summary>
    /// Mode of smooth zoom.
    /// </summary>
    public enum OnlineMapsSmoothZoomMode
    {
        /// <summary>
        /// Zoom at touch point.
        /// </summary>
        target,

        /// <summary>
        /// Zoom at center of map.
        /// </summary>
        center
    }
}