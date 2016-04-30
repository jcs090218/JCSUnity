/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class implements the basic functionality control of the 3D map.
/// </summary>
[Serializable]
[AddComponentMenu("")]
public class OnlineMapsControlBase3D: OnlineMapsControlBase
{
    /// <summary>
    /// The event, which occurs when controls the camera.
    /// </summary>
    public Action OnCameraControl;

    /// <summary>
    /// The camera you are using to display the map.
    /// </summary>
    public Camera activeCamera;

    /// <summary>
    /// Specifies whether to create a 3D marker by pressing N under the cursor.
    /// </summary>
    public bool allowAddMarker3DByN = true;

    /// <summary>
    /// Specifies whether to use markers event for 3D markers.
    /// </summary>
    public bool allowDefaultMarkerEvents = true;

    /// <summary>
    /// Specifies controls the camera.\n
    /// Use the right mouse button or two-finger swipe to control the camera.
    /// </summary>
    public bool allowCameraControl = false;

    /// <summary>
    /// The distance from the camera to the center point of the map.\n
    /// Used when the allowCameraControl.
    /// </summary>
    public float cameraDistance = 100;

    /// <summary>
    /// Rotation angle of the camera.\n
    /// Used when the allowCameraControl.
    /// </summary>
    public Vector2 cameraRotation = Vector2.zero;

    /// <summary>
    /// Camera movement speed.\n
    /// Used when the allowCameraControl.
    /// </summary>
    public Vector2 cameraSpeed = Vector2.one;

    /// <summary>
    /// Mode of 2D markers. Bake in texture or Billboard.
    /// </summary>
    public OnlineMapsMarker2DMode marker2DMode = OnlineMapsMarker2DMode.flat;

    /// <summary>
    /// Size of billboard markers.
    /// </summary>
    public float marker2DSize = 100;

    /// <summary>
    /// List of 3D markers.
    /// </summary>
    public OnlineMapsMarker3D[] markers3D;

    /// <summary>
    /// Scaling of 3D markers by default.
    /// </summary>
    public float marker3DScale = 1;

    protected GameObject markersGameObject;
    protected Mesh markersMesh;
    protected Renderer markersRenderer;

#if (!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR
    private bool isCameraControl = false;
#endif
    private Dictionary<int, OnlineMapsMarkerBillboard> markerBillboards;

    private Collider _cl;
    private Renderer _renderer;

    public Collider cl
    {
        get
        {
            if (_cl == null)
            {
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
                _cl = collider;
#else
                _cl = GetComponent<Collider>();
#endif
            }
            return _cl;
        }
    }

    public Renderer rendererInstance
    {
        get
        {
            if (_renderer == null)
            {
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
                _renderer = renderer;
#else
                _renderer = GetComponent<Renderer>();
#endif
            }
            return _renderer;
        }
    }

    /// <summary>
    /// Singleton instance of OnlineMapsControlBase3D control.
    /// </summary>
    public new static OnlineMapsControlBase3D instance
    {
        get { return OnlineMapsControlBase.instance as OnlineMapsControlBase3D; }
    }

    /// <summary>
    /// Adds a new 3D marker on the map.
    /// </summary>
    /// <param name="markerPosition">Vector2. X - Longituge, Y - Latitude.</param>
    /// <param name="prefab">Marker prefab.</param>
    /// <returns>Marker instance.</returns>
    public OnlineMapsMarker3D AddMarker3D(Vector2 markerPosition, GameObject prefab)
    {
        List<OnlineMapsMarker3D> ms = markers3D.ToList();
        OnlineMapsMarker3D marker = new OnlineMapsMarker3D
        {
            position = markerPosition, 
            prefab = prefab, 
            control = this,
            scale = marker3DScale,
            allowDefaultMarkerEvents = allowDefaultMarkerEvents
        };
        marker.Init(transform);
        ms.Add(marker);
        markers3D = ms.ToArray();
        return marker;
    }

    /// <summary>
    /// Adds a existing 3D marker on the map.
    /// </summary>
    /// <param name="marker">Instance of 3D marker.</param>
    /// <returns>Instance of 3D marker.</returns>
    public OnlineMapsMarker3D AddMarker3D(OnlineMapsMarker3D marker)
    {
        List<OnlineMapsMarker3D> ms = markers3D.ToList();
        ms.Add(marker);
        markers3D = ms.ToArray();
        return marker;
    }

    protected override void AfterUpdate()
    {
        base.AfterUpdate();

        if (api.showMarkerTooltip == OnlineMapsShowMarkerTooltip.onHover)
        {
            OnlineMapsMarkerInstanceBase markerInstance = GetBillboardMarkerFromScreen(Input.mousePosition);
            if (markerInstance != null)
            {
                api.tooltip = markerInstance.marker.label;
                api.tooltipMarker = markerInstance.marker;
            }
        }

        if (allowAddMarker3DByN && Input.GetKeyUp(KeyCode.N))
        {
            OnlineMapsMarker3D m = new OnlineMapsMarker3D
            {
                position = GetCoords(),
                scale = marker3DScale,
                control = this
            };
            m.Init(transform);
            double tlx, tly, brx, bry;
            api.GetTopLeftPosition(out tlx, out tly);
            api.GetBottomRightPosition(out brx, out bry);
            m.Update(tlx, tly, brx, bry, api.zoom);
            List<OnlineMapsMarker3D> markerList = markers3D.ToList();
            markerList.Add(m);
            markers3D = markerList.ToArray();
        }
    }

    protected override void BeforeUpdate()
    {
        base.BeforeUpdate();

        if (allowCameraControl)
        {
#if (!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR
            if (Input.GetMouseButton(1))
            {
                isCameraControl = true;
                Vector2 mousePosition = Input.mousePosition;
                if (lastMousePosition != mousePosition && lastMousePosition != Vector2.zero)
                {
                    Vector2 offset = lastMousePosition - mousePosition;
                    cameraRotation.x -= offset.y / 10f * cameraSpeed.x;
                    cameraRotation.y -= offset.x / 10f * cameraSpeed.y;
                }
                lastMousePosition = mousePosition;
            }
            else if (isCameraControl)
            {
                lastMousePosition = Vector2.zero;
                isCameraControl = false;
            }
#else
            if (!allowZoom && allowCameraControl)
            {
                if (Input.touchCount == 2)
                {
                    Vector2 p1 = Input.GetTouch(0).position;
                    Vector2 p2 = Input.GetTouch(1).position;

                    Vector2 center = Vector2.Lerp(p1, p2, 0.5f);
                    if (lastGestureCenter == Vector2.zero) lastGestureCenter = center;
                    else if (lastGestureCenter != center)
                    {
                        Vector2 offset = lastGestureCenter - center;
                        cameraRotation.x -= offset.y / 10f * cameraSpeed.x;
                        cameraRotation.y -= offset.x / 10f * cameraSpeed.y;
                        lastGestureCenter = center;
                    }

                    lastMousePosition = center;
                }
                else
                {
                    lastGestureCenter = Vector2.zero;
                }
            }
#endif

            UpdateCameraPosition();
        }
    }

    protected void Clear2DMarkerBillboards()
    {
        if (markerBillboards != null)
        {
            foreach (KeyValuePair<int, OnlineMapsMarkerBillboard> pair in markerBillboards)
                DestroyImmediate(pair.Value.gameObject);
        }
        
        markerBillboards = null;
        DestroyImmediate(markersGameObject);
        markersGameObject = null;
    }

    public virtual void Clear2DMarkerInstances()
    {
        if (marker2DMode == OnlineMapsMarker2DMode.billboard) OnlineMaps.instance.Redraw();
        else
        {
            Clear2DMarkerBillboards();
        }
    }

    /// <summary>
    /// Returns best value yScale, based on the coordinates corners of map.
    /// </summary>
    /// <param name="topLeftPosition">Top-Left corner coordinates.</param>
    /// <param name="bottomRightPosition">Bottom-Right corner coordinates.</param>
    /// <returns>Best yScale</returns>
    public virtual float GetBestElevationYScale(Vector2 topLeftPosition, Vector2 bottomRightPosition)
    {
        return 0;
    }

    /// <summary>
    /// Returns best value yScale, based on the coordinates corners of map.
    /// </summary>
    /// <param name="tlx">Top-left longitude.</param>
    /// <param name="tly">Top-left latitude.</param>
    /// <param name="brx">Bottom-right longitude.</param>
    /// <param name="bry">Bottom-right latitude.</param>
    /// <returns></returns>
    public virtual float GetBestElevationYScale(double tlx, double tly, double brx, double bry)
    {
        return 0;
    }

    /// <summary>
    /// Gets billboard marker on the screen position.
    /// </summary>
    /// <param name="screenPosition">Screen position.</param>
    /// <returns>Marker instance or null.</returns>
    public OnlineMapsMarkerInstanceBase GetBillboardMarkerFromScreen(Vector2 screenPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(activeCamera.ScreenPointToRay(screenPosition), out hit, OnlineMapsUtils.maxRaycastDistance))
        {
            return hit.collider.gameObject.GetComponent<OnlineMapsMarkerInstanceBase>();
        }
        return null;
    }

    /// <summary>
    /// Returns the elevation value, based on the coordinates of the scene.
    /// </summary>
    /// <param name="x">Scene X.</param>
    /// <param name="z">Scene Z.</param>
    /// <param name="yScale">Scale factor for evevation value.</param>
    /// <param name="topLeftPosition">Top-Left corner coordinates of map.</param>
    /// <param name="bottomRightPosition">Bottom-Right corner coordinates of map.</param>
    /// <returns>Elevation value.</returns>
    public virtual float GetElevationValue(float x, float z, float yScale, Vector2 topLeftPosition, Vector2 bottomRightPosition)
    {
        return 0;
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
    public virtual float GetElevationValue(float x, float z, float yScale, double tlx, double tly, double brx,
        double bry)
    {
        return 0;
    }

    public override Vector2 GetScreenPosition(Vector2 coords)
    {
        Vector2 mapPos = GetPosition(coords);
        mapPos.x /= api.width;
        mapPos.y /= api.height;
        Vector3 worldPos = new Vector3();

        if (this is OnlineMapsTileSetControl)
        {
            worldPos = transform.position + transform.rotation * new Vector3(-api.tilesetSize.x * mapPos.x * transform.localScale.x, 0, api.tilesetSize.y * mapPos.y * transform.localScale.z);
        }
        else
        {
            Bounds bounds = cl.bounds;
            worldPos.x = bounds.max.x - bounds.size.x * mapPos.x;
            worldPos.y = bounds.min.y;
            worldPos.z = bounds.min.z + bounds.size.z * mapPos.y;
        }

        Camera cam = activeCamera ?? Camera.main;
        return cam.WorldToScreenPoint(worldPos);
    }

    protected void InitMarkersMesh()
    {
        markersGameObject = new GameObject("Markers");
        markersGameObject.transform.parent = transform;
        if (this is OnlineMapsTileSetControl)
        {
            markersGameObject.transform.localPosition = new Vector3(0, OnlineMaps.instance.tilesetSize.magnitude / 2896, 0);
            markersGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        else markersGameObject.transform.localPosition = new Vector3(0, 0.2f, 0);

        markersRenderer = markersGameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = markersGameObject.AddComponent<MeshFilter>();
        markersMesh = new Mesh();
        markersMesh.name = "MarkersMesh";
        markersMesh.MarkDynamic();
        meshFilter.mesh = markersMesh;
    }

    protected override void OnDestroyLate()
    {
        base.OnDestroyLate();

        if (markersGameObject != null) DestroyImmediate(markersGameObject);
        markersGameObject = null;

        markers3D = null;
        markersMesh = null;
        markersRenderer = null;
    }

    protected override void OnEnableLate()
    {
        base.OnEnableLate();

        if (activeCamera == null) activeCamera = Camera.main;
    }

    protected override void OnGestureZoom(Vector2 p1, Vector2 p2)
    {
        base.OnGestureZoom(p1, p2);
        if (allowCameraControl)
        {
            Vector2 center = Vector2.Lerp(p1, p2, 0.5f);
            if (lastGestureCenter != center)
            {
                Vector2 offset = lastGestureCenter - center;
                cameraRotation.x -= offset.y / 10f * cameraSpeed.x;
                cameraRotation.y -= offset.x / 10f * cameraSpeed.y;
            }
            lastMousePosition = center;
        }
    }

    /// <summary>
    /// Remove all 3D markers from map.
    /// </summary>
    public void RemoveAllMarker3D()
    {
        foreach (OnlineMapsMarker3D marker in markers3D) if (marker.instance != null) Destroy(marker.instance);
        markers3D = new OnlineMapsMarker3D[0];
    }

    /// <summary>
    /// Removes the specified 3D marker.
    /// </summary>
    /// <param name="marker">3D marker</param>
    public void RemoveMarker3D(OnlineMapsMarker3D marker)
    {
        List<OnlineMapsMarker3D> ms = markers3D.ToList();
        ms.Remove(marker);
        markers3D = ms.ToArray();
        if (marker.instance != null) Destroy(marker.instance);
    }

    /// <summary>
    /// Removes 3D marker by index.
    /// </summary>
    /// <param name="markerIndex">Marker index.</param>
    public void RemoveMarker3DAt(int markerIndex)
    {
        if (markerIndex < 0 || markerIndex >= markers3D.Length) return;

        OnlineMapsMarker3D marker = markers3D[markerIndex];
        List<OnlineMapsMarker3D> ms = markers3D.ToList();
        ms.Remove(marker);
        markers3D = ms.ToArray();
        if (marker.instance != null) Destroy(marker.instance);
    }

    public OnlineMapsXML SaveMarkers3D(OnlineMapsXML parent)
    {
        if (markers3D == null || markers3D.Length == 0) return null;
        OnlineMapsXML element = parent.Create("Markers3D");
        foreach (OnlineMapsMarker3D marker in markers3D) marker.Save(element);
        return element;
    }

    public override OnlineMapsXML SaveSettings(OnlineMapsXML parent)
    {
        OnlineMapsXML element = base.SaveSettings(parent);
        element.Create("AllowAddMarker3DByN", allowAddMarker3DByN);
        element.Create("AllowCameraControl", allowCameraControl);

        if (allowCameraControl)
        {
            element.Create("CameraDistance", cameraDistance);
            element.Create("CameraRotation", cameraRotation);
            element.Create("CameraSpeed", cameraSpeed);
        }

        element.Create("Marker2DMode", (int) marker2DMode);
        element.Create("Marker2DSize", marker2DSize);
        element.Create("Marker3DScale", marker3DScale);
        element.Create("Camera", activeCamera);

        return element;
    }

    private void Start()
    {
        foreach (OnlineMapsMarker3D marker in markers3D.Where(m => !m.inited))
        {
            marker.control = this;
            marker.Init(transform);
        }
        UpdateMarkers3D();
    }

    private void UpdateCameraPosition()
    {
        if (cameraRotation.x > 80) cameraRotation.x = 80f;
        else if (cameraRotation.x < 0) cameraRotation.x = 0;

        float rx = 90 - cameraRotation.x;
        if (rx > 89.9) rx = 89.9f;

        double px = Math.Cos(rx * Mathf.Deg2Rad) * cameraDistance;
        double py = Math.Sin(rx * Mathf.Deg2Rad) * cameraDistance;
        double pz = Math.Cos(cameraRotation.y * Mathf.Deg2Rad) * px;
        px = Math.Sin(cameraRotation.y * Mathf.Deg2Rad) * px;

        Vector3 targetPosition = transform.position;
        if (this is OnlineMapsTileSetControl)
        {
            Vector3 offset = new Vector3(api.tilesetSize.x / -2, 0, api.tilesetSize.y / 2);
            OnlineMapsTileSetControl control = OnlineMapsTileSetControl.instance;

            if (control.smoothZoom && control.smoothZoomStarted)
            {
                targetPosition = control.originalPosition;
            }
            
            if (control.useElevation && control.elevationZoomRange.InRange(api.zoom))
            {
                double tlx, tly, brx, bry;
                api.GetTopLeftPosition(out tlx, out tly);
                api.GetBottomRightPosition(out brx, out bry);

                float yScale = control.GetBestElevationYScale(tlx, tly, brx, bry);
                offset.y = control.GetMaxElevationValue(yScale);
            }
            
            targetPosition += transform.rotation * offset;
        }

        Vector3 oldPosition = activeCamera.transform.position;
        Vector3 newPosition = transform.rotation * new Vector3((float)px, (float)py, (float)pz) + targetPosition;

        activeCamera.transform.position = newPosition;
        activeCamera.transform.LookAt(targetPosition);

        if (oldPosition != newPosition && OnCameraControl != null) OnCameraControl();
    }

    /// <summary>
    /// Updates the current control.
    /// </summary>
    public virtual void UpdateControl()
    {
        if (marker2DMode == OnlineMapsMarker2DMode.billboard) UpdateMarkersBillboard();
        UpdateMarkers3D();
    }

    protected void UpdateMarkers3D()
    {
        double tlx, tly, brx, bry;

        api.GetTopLeftPosition(out tlx, out tly);
        api.GetBottomRightPosition(out brx, out bry);

        int zoom = api.zoom;

        foreach (OnlineMapsMarker3D marker in markers3D) marker.Update(tlx, tly, brx, bry, zoom);
    }

    /// <summary>
    /// Updates billboard markers.
    /// </summary>
    protected void UpdateMarkersBillboard()
    {
        if (markersGameObject == null) InitMarkersMesh();
        if (markerBillboards == null) markerBillboards = new Dictionary<int, OnlineMapsMarkerBillboard>();

        double tlx, tly, brx, bry;
        api.GetTopLeftPosition(out tlx, out tly);
        api.GetBottomRightPosition(out brx, out bry);
        if (brx < tlx) brx += 360;

        int maxX = (2 << api.buffer.apiZoom) / 2;

        double px, py;
        OnlineMapsUtils.LatLongToTiled(tlx, tly, api.zoom, out px, out py);

        float yScale = GetBestElevationYScale(tlx, tly, brx, bry);

        Bounds mapBounds = cl.bounds;
        Vector3 positionOffset = transform.position - mapBounds.min;
        Vector3 size = mapBounds.size;
        size = transform.rotation * size;
        if (api.target == OnlineMapsTarget.tileset) positionOffset.x -= size.x;

        foreach (KeyValuePair<int, OnlineMapsMarkerBillboard> billboard in markerBillboards) billboard.Value.used = false;

        foreach (OnlineMapsMarker marker in api.markers)
        {
            if (!marker.enabled || !marker.range.InRange(api.zoom)) continue;
            float mx = marker.position.x;
            if (!(((mx > tlx && mx < brx) || (mx + 360 > tlx && mx + 360 < brx) ||
                 (mx - 360 > tlx && mx - 360 < brx)) &&
                marker.position.y < tly && marker.position.y > bry)) continue;

            int markerHashCode = marker.GetHashCode();
            OnlineMapsMarkerBillboard markerBillboard = null;

            if (!markerBillboards.ContainsKey(markerHashCode))
            {
                markerBillboard = OnlineMapsMarkerBillboard.Create(marker);
                markerBillboard.transform.parent = markersGameObject.transform;

                markerBillboards.Add(markerHashCode, markerBillboard);
            }
            else markerBillboard = markerBillboards[markerHashCode];

            float sx = size.x / api.width * marker2DSize * marker.scale;
            float sz = size.z / api.height * marker2DSize * marker.scale;
            float s = Mathf.Max(sx, sz);
            markerBillboard.transform.localScale = new Vector3(-s, s, s);

            Vector2 p = OnlineMapsUtils.LatLongToTilef(marker.position, api.buffer.apiZoom);

            p.x = Mathf.Repeat(p.x - (float)px, maxX);
            p.y -= (float) py;

            float x = -p.x / api.width * OnlineMapsUtils.tileSize *  size.x + positionOffset.x;
            float z = p.y / api.height * OnlineMapsUtils.tileSize * size.z - positionOffset.z;

            float y = GetElevationValue(x, z, yScale, tlx, tly, brx, bry);

            markerBillboard.transform.localPosition = transform.rotation * new Vector3(x, y, z);
            markerBillboard.used = true;
        }

        List<int> keysForRemove = new List<int>();

        foreach (KeyValuePair<int, OnlineMapsMarkerBillboard> billboard in markerBillboards)
        {
            if (!billboard.Value.used)
            {
                billboard.Value.Dispose();
                keysForRemove.Add(billboard.Key);
            }
        }

        foreach (int key in keysForRemove) markerBillboards.Remove(key);
    }
}