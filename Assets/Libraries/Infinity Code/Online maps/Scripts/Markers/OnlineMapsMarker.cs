/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;

#if !UNITY_WEBGL
using System.Threading;
#endif

/// <summary>
/// 2D marker class.\n
/// To add 2D marker use OnlineMaps.AddMarker.\n
/// To remove a marker use OnlineMaps.RemoveMarker.
/// </summary>
[Serializable]
public class OnlineMapsMarker : OnlineMapsMarkerBase
{
    /// <summary>
    /// Marker texture align.
    /// </summary>
    public OnlineMapsAlign align = OnlineMapsAlign.Bottom;

    /// <summary>
    /// Specifies to OnlineMapsBuffer that the marker is available for drawing.\n
    /// <strong>Please do not use.</strong>
    /// </summary>
    public bool locked;

    /// <summary>
    /// Relative area of ​​activity of the marker.
    /// </summary>
    public Rect markerColliderRect = new Rect(-0.5f, -0.5f, 1, 1);

    /// <summary>
    /// Texture marker. \n
    /// Texture format: ARGB32.\n
    /// <strong>Must enable "Read / Write enabled".</strong>\n
    /// After changing the texture you need to call OnlineMapsMarker.Init.
    /// </summary>
    public Texture2D texture;

    private Color[] _colors;
    private int _height;

    [SerializeField]
    private float _rotation = 0;

    private Color[] _rotatedColors;
    private int _textureHeight;
    private int _textureWidth;
    private int _width;

    /// <summary>
    /// Gets the marker colors.
    /// </summary>
    /// <value>
    /// The colors.
    /// </value>
    public Color[] colors
    {
        get { return (_rotation == 0 || OnlineMaps.instance.target == OnlineMapsTarget.tileset) ? _colors : _rotatedColors; }
    }

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
                if (OnEnabledChange != null) OnEnabledChange(this);
                OnlineMaps.instance.Redraw();
            }
        }
    }

    /// <summary>
    /// Gets the marker height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public int height
    {
        get { return _height; }
    }

    /// <summary>
    /// Gets or sets the rotation.
    /// </summary>
    /// <value>
    /// The rotation.
    /// </value>
    public float rotation
    {
        get { return _rotation; }
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                if (Application.isPlaying)
                {
                    if (!(OnlineMapsControlBase.instance is OnlineMapsTileSetControl)) UpdateRotatedBuffer();
                    OnlineMaps.instance.Redraw();
                }
            }
        }
    }

    /// <summary>
    /// Area of ​​the screen, which is a marker at the current map display.
    /// Note: When used as a source display Texture or Tileset, is not returned the correct value.
    /// </summary>
    /// <value>
    /// Marker rectangle.
    /// </value>
    public Rect screenRect
    {
        get
        {
            return GetRect();
        }
    }

    /// <summary>
    /// Area of ​​the screen, which is a marker after map will be redrawed.
    /// Note: When used as a source display Texture or Tileset, is not returned the correct value.
    /// </summary>
    /// <value>
    /// Marker rectangle.
    /// </value>
    public Rect realScreenRect
    {
        get
        {
            OnlineMaps api = OnlineMaps.instance;
            Rect controlRect = api.control.GetRect();
            Rect uvRect = api.control.uvRect;
            controlRect.width /= uvRect.width;
            controlRect.height /= uvRect.height;
            controlRect.x -= controlRect.width * uvRect.x;
            controlRect.y -= controlRect.height * uvRect.y;
            Vector2 topLeft = OnlineMapsUtils.LatLongToTilef(api.topLeftPosition, api.zoom) * OnlineMapsUtils.tileSize;
            Vector2 pos = GetAlignedPosition(OnlineMapsUtils.LatLongToTilef(position, api.zoom) * OnlineMapsUtils.tileSize);
            float scaleX = (controlRect.width / api.width);
            float scaleY = (controlRect.height / api.height);
            pos -= topLeft;
            pos.x = Mathf.RoundToInt(pos.x * scaleX + controlRect.x);
            pos.y = Mathf.RoundToInt(controlRect.yMax - (pos.y + height) * scaleY);

            return new Rect(pos.x, pos.y, width * scaleX, height * scaleY);
        }
    }

    /// <summary>
    /// Gets the marker width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public int width
    {
        get
        {
            return _width;
        }
    }

    public OnlineMapsMarker()
    {
        range = new OnlineMapsRange(3, 20);
    }

    /// <summary>
    /// Gets aligned position.
    /// </summary>
    /// <param name="pos">
    /// Buffer position.
    /// </param>
    /// <returns>
    /// The aligned buffer position.
    /// </returns>
    public OnlineMapsVector2i GetAlignedPosition(OnlineMapsVector2i pos)
    {
        OnlineMapsVector2i offset = GetAlignOffset();

        if (_rotation == 0) return pos - offset;
        
        float angle = 1 - Mathf.Repeat(_rotation * 360, 360);
        Matrix4x4 matrix = new Matrix4x4();

        matrix.SetTRS(new Vector3(_width / 2, 0, _height / 2), Quaternion.Euler(0, angle, 0), Vector3.one);
        Vector3 off = matrix.MultiplyPoint(new Vector3(offset.x - _textureWidth / 2, 0, offset.y - _textureHeight / 2));
        pos.x -= (int)off.x;
        pos.y -= (int)off.z;

        return pos;
    }

    /// <summary>
    /// Gets aligned offset (in pixels).
    /// </summary>
    /// <returns>Aligned offset.</returns>
    public OnlineMapsVector2i GetAlignOffset()
    {
        OnlineMapsVector2i offset = new OnlineMapsVector2i();
        if (align == OnlineMapsAlign.BottomRight || align == OnlineMapsAlign.Right ||
            align == OnlineMapsAlign.TopRight) offset.x = _textureWidth;
        else if (align == OnlineMapsAlign.Bottom || align == OnlineMapsAlign.Center ||
                 align == OnlineMapsAlign.Top) offset.x = _textureWidth / 2;

        if (align == OnlineMapsAlign.BottomRight || align == OnlineMapsAlign.Bottom ||
            align == OnlineMapsAlign.BottomLeft) offset.y = _textureHeight;
        else if (align == OnlineMapsAlign.Left || align == OnlineMapsAlign.Center ||
                 align == OnlineMapsAlign.Right) offset.y = _textureHeight / 2;
        return offset;
    }

    private Rect GetRect()
    {
        OnlineMaps api = OnlineMaps.instance;
        Rect controlRect = api.control.GetRect();
        Rect uvRect = api.control.uvRect;
        controlRect.width /= uvRect.width;
        controlRect.height /= uvRect.height;
        controlRect.x -= controlRect.width * uvRect.x;
        controlRect.y -= controlRect.height * uvRect.y;
        Vector2 topLeft = OnlineMapsUtils.LatLongToTilef(api.buffer.topLeftPosition, api.buffer.apiZoom) * OnlineMapsUtils.tileSize;
        Vector2 pos = GetAlignedPosition(OnlineMapsUtils.LatLongToTilef(position, api.buffer.apiZoom) * OnlineMapsUtils.tileSize);
        float scaleX = (controlRect.width / api.width);
        float scaleY = (controlRect.height / api.height);
        pos -= topLeft;
        pos.x = Mathf.RoundToInt(pos.x * scaleX + controlRect.x);
        pos.y = Mathf.RoundToInt(controlRect.yMax - (pos.y + height) * scaleY);

        return new Rect(pos.x, pos.y, width * scaleX, height * scaleY);
    }

    /// <summary>
    /// Determines if the marker at the specified coordinates.
    /// </summary>
    /// <param name="positionLatLng">
    /// Position.
    /// </param>
    /// <param name="zoom">
    /// The zoom.
    /// </param>
    /// <returns>
    /// True if the marker in position, false if not.
    /// </returns>
    public bool HitTest(Vector2 positionLatLng, int zoom)
    {
        if (OnlineMapsControlBase.instance is OnlineMapsTileSetControl)
        {
            OnlineMapsVector2i pos = OnlineMapsUtils.LatLongToTilef(position, zoom) * OnlineMapsUtils.tileSize;
            float w = width * scale;
            float h = height * scale;

            pos = GetAlignedPosition(pos);
            Vector2 mPos = OnlineMapsUtils.LatLongToTilef(positionLatLng, zoom) * OnlineMapsUtils.tileSize;

            return mPos.x >= pos.x + w * (markerColliderRect.x + 0.5f) && mPos.x <= pos.x + w * (markerColliderRect.xMax + 0.5f) && mPos.y >= pos.y + w * (markerColliderRect.y + 0.5f) && mPos.y <= pos.y + h * (markerColliderRect.yMax + 0.5f);
        }
        else
        {
            OnlineMapsVector2i pos = OnlineMapsUtils.LatLongToTilef(position, zoom) * OnlineMapsUtils.tileSize;
            int w = width;
            int h = height;

            pos = GetAlignedPosition(pos);
            Vector2 mPos = OnlineMapsUtils.LatLongToTilef(positionLatLng, zoom) * OnlineMapsUtils.tileSize;

            return mPos.x >= pos.x + w * (markerColliderRect.x + 0.5f) && mPos.x <= pos.x + w * (markerColliderRect.xMax + 0.5f) && mPos.y >= pos.y + w * (markerColliderRect.y + 0.5f) && mPos.y <= pos.y + h * (markerColliderRect.yMax + 0.5f);
        }
    }

    /// <summary>
    /// Initialises this marker.
    /// </summary>
    public void Init()
    {
        if (texture != null)
        {
            if (OnlineMaps.instance.target == OnlineMapsTarget.texture) _colors = texture.GetPixels();
            _width = _textureWidth = texture.width;
            _height = _textureHeight = texture.height;
        }
        else
        {
            Texture2D defaultTexture = OnlineMaps.instance.defaultMarkerTexture;
            if (defaultTexture != null)
            {
                if (OnlineMaps.instance.target == OnlineMapsTarget.texture) _colors = defaultTexture.GetPixels();
                _width = _textureWidth = defaultTexture.width;
                _height = _textureHeight = defaultTexture.height;
            }
        }
        if (_rotation != 0) UpdateRotatedBuffer();
    }

    public override void LookToCoordinates(Vector2 coordinates)
    {
        rotation = 1.25f - OnlineMapsUtils.Angle2D(coordinates, position) / 360;
    }

    public override OnlineMapsXML Save(OnlineMapsXML parent)
    {
        OnlineMapsXML element = base.Save(parent);
        element.Create("Texture", texture);
        element.Create("Align", (int) align);
        element.Create("Rotation", rotation);
        return element;
    }

    private void UpdateRotatedBuffer()
    {
        if (_rotation == 0 || OnlineMaps.instance.target == OnlineMapsTarget.tileset)
        {
            _width = _textureWidth;
            _height = _textureHeight;
            return;
        }

#if !UNITY_WEBGL
        int maxLocked = 20;
        while (locked && maxLocked > 0)
        {
            Thread.Sleep(1);
            maxLocked--;
        }
#endif

        locked = true;

        float angle = Mathf.Repeat(_rotation * 360, 360);
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetTRS(Vector3.zero, Quaternion.Euler(0, angle, 0), Vector3.one);
        Matrix4x4 inv = matrix.inverse;
        Vector3 p1 = matrix.MultiplyPoint3x4(new Vector3(_textureWidth, 0, 0));
        Vector3 p2 = matrix.MultiplyPoint3x4(new Vector3(0, 0, _textureHeight));
        Vector3 p3 = matrix.MultiplyPoint3x4(new Vector3(_textureWidth, 0, _textureHeight));

        float minX = Mathf.Min(0, p1.x, p2.x, p3.x);
        float minZ = Mathf.Min(0, p1.z, p2.z, p3.z);
        float maxX = Mathf.Max(0, p1.x, p2.x, p3.x);
        float maxZ = Mathf.Max(0, p1.z, p2.z, p3.z);

        _width = Mathf.RoundToInt(maxX - minX + 0.5f);
        _height = Mathf.RoundToInt(maxZ - minZ + 0.5f);

        Color emptyColor = new Color(0, 0, 0, 0);

        _rotatedColors = new Color[_width * _height];

        int tw = _textureWidth;
        int th = _textureHeight;

        for (int y = 0; y < _height; y++)
        {
            float ry = minZ + y;
            int cy = y * _width;
            for (int x = 0; x < _width; x++)
            {
                float rx = minX + x;
                Vector3 p = inv.MultiplyPoint3x4(new Vector3(rx, 0, ry));
                int iz = (int)p.z;
                int ix = (int)p.x;
                float fx = p.x - ix;
                float fz = p.z - iz;

                if (ix + 1 >= 0 && ix < tw && iz + 1 >= 0 && iz < th)
                {
                    Color[] clrs = { emptyColor, emptyColor, emptyColor, emptyColor };
                    if (ix >= 0 && iz >= 0) clrs[0] = _colors[iz * tw + ix];
                    if (ix + 1 < tw && iz >= 0) clrs[1] = _colors[iz * tw + ix + 1];
                    if (ix >= 0 && iz + 1 < th) clrs[2] = _colors[(iz + 1) * tw + ix];
                    if (ix + 1 < tw && iz + 1 < th) clrs[3] = _colors[(iz + 1) * tw + ix + 1];

                    clrs[0] = Color.Lerp(clrs[0], clrs[1], fx);
                    clrs[2] = Color.Lerp(clrs[2], clrs[3], fx);

                    _rotatedColors[cy + x] = Color.Lerp(clrs[0], clrs[2], fz);
                }
                else _rotatedColors[cy + x] = emptyColor;
            }
        }

        locked = false;
    }
}