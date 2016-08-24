/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// 3D marker class.\n
/// <strong>Can be used only when the source display - Texture or Tileset.</strong>\n
/// To create a new 3D marker use OnlineMapsControlBase3D.AddMarker3D.
/// </summary>
[Serializable]
public class OnlineMapsMarker3D : OnlineMapsMarkerBase
{
    /// <summary>
    /// Event that occurs when the marker position changed.
    /// </summary>
    public Action<OnlineMapsMarker3D> OnPositionChanged;

    /// <summary>
    /// Specifies whether to use a marker event for 3D markers. \n
    /// Otherwise you will have to create their own events using MonoBehaviour.
    /// </summary>
    public bool allowDefaultMarkerEvents;

    /// <summary>
    /// Reference of 3D control.
    /// </summary>
    public OnlineMapsControlBase3D control;

    /// <summary>
    /// Specifies whether the marker is initialized.
    /// </summary>
    [HideInInspector] 
    public bool inited = false;

    /// <summary>
    /// The instance.
    /// </summary>
    [HideInInspector]
    public GameObject instance;

    /// <summary>
    /// Marker prefab GameObject.
    /// </summary>
    public GameObject prefab;

    private GameObject _prefab;
    private Vector3 _relativePosition;

    /// <summary>
    /// Gets or sets marker enabled.
    /// </summary>
    /// <value>
    /// true if enabled, false if not.
    /// </value>
    public override bool enabled
    {
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                instance.SetActive(value);
                if (OnEnabledChange != null) OnEnabledChange(this);
            }
        }
    }

    /// <summary>
    /// Returns the position of the marker relative to Texture.
    /// </summary>
    /// <value>
    /// The relative position.
    /// </value>
    public Vector3 relativePosition
    {
        get
        {
            return enabled ? _relativePosition : Vector3.zero;
        }
    }

    /// <summary>
    /// Gets or sets rotation of 3D marker.
    /// </summary>
    public Quaternion rotation
    {
        get { return (transform != null)? transform.rotation: new Quaternion(); }
        set { if (transform != null) transform.rotation = value; }
    }

    /// <summary>
    /// Gets the instance transform.
    /// </summary>
    /// <value>
    /// The transform.
    /// </value>
    public Transform transform
    {
        get
        {
            return (instance != null)? instance.transform: null;
        }
    }

    /// <summary>
    /// Constructor of 3D marker
    /// </summary>
    public OnlineMapsMarker3D()
    {
        range = new OnlineMapsRange(3, 20);
    }

    /// <summary>
    /// Create 3D marker from an existing GameObject.
    /// </summary>
    /// <param name="instance">GameObject to be used as a 3D marker.</param>
    public OnlineMapsMarker3D(GameObject instance):this()
    {
        prefab = _prefab = instance;
        this.instance = instance;
        instance.AddComponent<OnlineMapsMarker3DInstance>().marker = this;

        OnlineMaps api = OnlineMaps.instance;

        double tlx, tly, brx, bry;
        api.GetTopLeftPosition(out tlx, out tly);
        api.GetBottomRightPosition(out brx, out bry);

        Update(tlx, tly, brx, bry, api.zoom);
    }

    /// <summary>
    /// Initialises this object.
    /// </summary>
    /// <param name="parent">
    /// The parent transform.
    /// </param>
    public void Init(Transform parent)
    {
        if (instance != null) Object.DestroyImmediate(instance);

        if (prefab == null)
        {
            instance = GameObject.CreatePrimitive(PrimitiveType.Cube);
            instance.transform.localScale = Vector3.one;
        }
        else instance = (GameObject)Object.Instantiate(prefab);

        _prefab = prefab;
        
        instance.transform.parent = parent;
        instance.AddComponent<OnlineMapsMarker3DInstance>().marker = this;
        enabled = false;
        inited = true;
        OnlineMaps api = OnlineMaps.instance;

        double tlx, tly, brx, bry;
        api.GetTopLeftPosition(out tlx, out tly);
        api.GetBottomRightPosition(out brx, out bry);

        Update(tlx, tly, brx, bry, api.zoom);
    }

    public override void LookToCoordinates(Vector2 coordinates)
    {
        
    }

    /// <summary>
    /// Reinitialises this object.
    /// </summary>
    /// <param name="topLeft">
    /// The top left.
    /// </param>
    /// <param name="bottomRight">
    /// The bottom right.
    /// </param>
    /// <param name="zoom">
    /// The zoom.
    /// </param>
    public void Reinit(Vector2 topLeft, Vector2 bottomRight, int zoom)
    {
        Reinit(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y, zoom);
    }

    /// <summary>
    /// Reinitialises this object.
    /// </summary>
    /// <param name="tlx">Top-left longitude of map</param>
    /// <param name="tly">Top-left latitude of map</param>
    /// <param name="brx">Bottom-right longitude of map</param>
    /// <param name="bry">Bottom-right latitude of map</param>
    /// <param name="zoom">Map zoom</param>
    public void Reinit(double tlx, double tly, double brx, double bry, int zoom)
    {
        if (instance)
        {
            Transform parent = instance.transform.parent;
            Object.Destroy(instance);
            Init(parent);
        }
        Update(tlx, tly, brx, bry, zoom);
    }

    public override OnlineMapsXML Save(OnlineMapsXML parent)
    {
        OnlineMapsXML element = base.Save(parent);
        element.Create("Prefab", prefab);
        element.Create("Rotation", rotation.eulerAngles);
        return element;
    }

    /// <summary>
    /// Updates this object.
    /// </summary>
    /// <param name="topLeft">
    /// The top left coordinates.
    /// </param>
    /// <param name="bottomRight">
    /// The bottom right coordinates.
    /// </param>
    /// <param name="zoom">
    /// The zoom.
    /// </param>
    public override void Update(Vector2 topLeft, Vector2 bottomRight, int zoom)
    {
        Update(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y, zoom);
    }

    public override void Update(double tlx, double tly, double brx, double bry, int zoom)
    {
        if (instance == null)
        {
            Debug.Log("No instance");
            return;
        }

        if (!range.InRange(zoom)) enabled = false;
        else if (position.y > tly || position.y < bry) enabled = false;
        else if (tlx < brx && (position.x < tlx || position.x > brx)) enabled = false;
        else if (tlx > brx && position.x < tlx && position.x > brx) enabled = false;
        else enabled = true;

        if (!enabled) return;

        if (_prefab != prefab) Reinit(tlx, tly, brx, bry, zoom);

        double mx, my;
        OnlineMapsUtils.LatLongToTiled(position.x, position.y, zoom, out mx, out my);

        double ttlx, ttly, tbrx, tbry;
        OnlineMapsUtils.LatLongToTiled(tlx, tly, zoom, out ttlx, out ttly);
        OnlineMapsUtils.LatLongToTiled(brx, bry, zoom, out tbrx, out tbry);

        int maxX = (2 << zoom) / 2;

        OnlineMaps api = OnlineMaps.instance;
        Bounds bounds = api.GetComponent<Collider>().bounds;

        double sx = tbrx - ttlx;
        if (sx < 0) sx += maxX;

        double mpx = mx - ttlx;
        if (mpx < 0) mpx += maxX;

        double px = mpx / sx;
        double pz = (ttly - my) / (ttly - tbry);

        _relativePosition = new Vector3((float)px, 0, (float)pz);

        if (OnlineMapsControlBase.instance is OnlineMapsTileSetControl)
        {
            px = -api.tilesetSize.x / 2 - (px - 0.5) * api.tilesetSize.x;
            pz = api.tilesetSize.y / 2 + (pz - 0.5) * api.tilesetSize.y;
        }
        else
        {
            Vector3 center = bounds.center;
            Vector3 size = bounds.size;
            px = center.x - (px - 0.5) * size.x - api.transform.position.x;
            pz = center.z + (pz - 0.5) * size.z - api.transform.position.z;
        }

        Vector3 oldPosition = instance.transform.localPosition;
        float y = 0;

        if (OnlineMapsControlBase.instance is OnlineMapsTileSetControl)
        {
            OnlineMapsTileSetControl control = OnlineMapsTileSetControl.instance;
            y = control.GetElevationValue((float)px, (float)pz, control.GetBestElevationYScale(tlx, tly, brx, bry), tlx, tly, brx, bry);
        }

        Vector3 newPosition = new Vector3((float)px, y, (float)pz);

        instance.transform.localPosition = newPosition;
        if (oldPosition != newPosition && OnPositionChanged != null) OnPositionChanged(this);
    }
}
