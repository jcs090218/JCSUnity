/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;

/// <summary>
/// The base class for markers.
/// </summary>
[Serializable]
public class OnlineMapsMarkerBase
{
    /// <summary>
    /// Default event caused to draw tooltip.
    /// </summary>
    public static Action<OnlineMapsMarkerBase> OnMarkerDrawTooltip;

    /// <summary>
    /// Events that occur when user click on the marker.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnClick;

    /// <summary>
    /// Events that occur when user double click on the marker.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnDoubleClick;

    /// <summary>
    /// Events that occur when user drag the marker.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnDrag;

    /// <summary>
    /// Event caused to draw tooltip.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnDrawTooltip;

    /// <summary>
    /// Event occurs when the marker enabled change.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnEnabledChange;

    /// <summary>
    /// Events that occur when user long press on the marker.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnLongPress;

    /// <summary>
    /// Events that occur when user press on the marker.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnPress;

    /// <summary>
    /// Events that occur when user release on the marker.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnRelease;

    /// <summary>
    /// Events that occur when user roll out marker.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnRollOut;

    /// <summary>
    /// Events that occur when user roll over marker.
    /// </summary>
    public Action<OnlineMapsMarkerBase> OnRollOver;

    /// <summary>
    /// In this variable you can put any data that you need to work with markers.
    /// </summary>
    public object customData;

    /// <summary>
    /// Marker label.
    /// </summary>
    public string label = "";

    /// <summary>
    /// Marker coordinates.
    /// </summary>
    public Vector2 position;

    /// <summary>
    /// Zoom range, in which the marker will be displayed.
    /// </summary>
    public OnlineMapsRange range;

    /// <summary>
    /// Scale of marker.\n
    /// <strong>Note: </strong>It's works for 3D markers and Billboard markers in 3D controls, and Flat markers in tileset.
    /// </summary>
    public float scale = 1;

    protected bool _enabled = true;

    /// <summary>
    /// Gets or sets marker enabled.
    /// </summary>
    /// <value>
    /// true if enabled, false if not.
    /// </value>
    public virtual bool enabled
    {
        get { return _enabled; }
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                if (OnEnabledChange != null) OnEnabledChange(this);
            }
        }
    }

    /// <summary>
    /// Checks to display marker in current map view.
    /// </summary>
    public virtual bool inMapView
    {
        get
        {
            if (!enabled) return false;

            OnlineMaps api = OnlineMaps.instance;

            if (!range.InRange(api.zoom)) return false;

            double tlx, tly, brx, bry;
            api.GetTopLeftPosition(out tlx, out tly);
            api.GetBottomRightPosition(out brx, out bry);

            if (position.x >= tlx && position.x <= brx && position.y >= bry && position.y <= tly) return true;
            return false;
        }
    }

    /// <summary>
    /// Turns the marker in the direction specified coordinates.
    /// </summary>
    /// <param name="coordinates">
    /// The coordinates.
    /// </param>
    public virtual void LookToCoordinates(Vector2 coordinates)
    {
        
    }

    private void OnMarkerPress(OnlineMapsMarkerBase onlineMapsMarkerBase)
    {
        OnlineMapsControlBase.instance.dragMarker = this;
    }

    public virtual OnlineMapsXML Save(OnlineMapsXML parent)
    {
        OnlineMapsXML element = parent.Create("Marker");
        element.Create("Position", position);
        element.Create("Range", range);
        element.Create("Label", label);
        return element;
    }

    /// <summary>
    /// Makes the marker dragable.
    /// </summary>
    public void SetDragable()
    {
        OnPress += OnMarkerPress;
    }

    /// <summary>
    /// Method that called when need update marker.
    /// </summary>
    /// <param name="topLeft">Coordinates of top-Left corner of map.</param>
    /// <param name="bottomRight">Coordinates of bottom-right corner of map.</param>
    /// <param name="zoom">Map zoom.</param>
    public virtual void Update(Vector2 topLeft, Vector2 bottomRight, int zoom)
    {
        
    }

    /// <summary>
    /// Method that called when need update marker.
    /// </summary>
    /// <param name="tlx">Top-left longitude.</param>
    /// <param name="tly">Top-left latutude.</param>
    /// <param name="brx">Bottom-right longitude.</param>
    /// <param name="bry">Bottom-right latitude.</param>
    /// <param name="zoom">Map zoom.</param>
    public virtual void Update(double tlx, double tly, double brx, double bry, int zoom)
    {

    }
}