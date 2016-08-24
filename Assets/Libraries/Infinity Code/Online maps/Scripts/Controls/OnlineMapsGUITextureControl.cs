/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// Class control the map for the GUITexture.
/// </summary>
[AddComponentMenu("Infinity Code/Online Maps/Controls/GUITexture")]
[RequireComponent(typeof(GUITexture))]
// ReSharper disable once UnusedMember.Global
public class OnlineMapsGUITextureControl : OnlineMapsControlBase2D
{
    private GUITexture _gTexture;

    /// <summary>
    /// Singleton instance of OnlineMapsGUITextureControl control.
    /// </summary>
    public new static OnlineMapsGUITextureControl instance
    {
        get { return OnlineMapsControlBase.instance as OnlineMapsGUITextureControl; }
    }

    public GUITexture gTexture
    {
        get
        {
            if (_gTexture == null)
            {
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
                _gTexture = guiTexture;
#else
                _gTexture = GetComponent<GUITexture>();
#endif
            }
            return _gTexture;
        }
    }

    public override Vector2 GetCoords(Vector2 position)
    {
        Rect rect = screenRect;
        int countX = api.texture.width / OnlineMapsUtils.tileSize;
        int countY = api.texture.height / OnlineMapsUtils.tileSize;
        double px, py;
        api.GetPosition(out px, out py);
        OnlineMapsUtils.LatLongToTiled(px, py, api.zoom, out px, out py);
        float rx = (rect.center.x - position.x) / rect.width * 2;
        float ry = (rect.center.y - position.y) / rect.height * 2;
        px -= countX / 2f * rx;
        py += countY / 2f * ry;
        OnlineMapsUtils.TileToLatLong(px, py, api.zoom, out px, out py);
        return new Vector2((float)px, (float)py);
    }

    public override bool GetCoords(out double lng, out double lat, Vector2 position)
    {
        Rect rect = screenRect;
        int countX = api.texture.width / OnlineMapsUtils.tileSize;
        int countY = api.texture.height / OnlineMapsUtils.tileSize;
        double px, py;
        api.GetPosition(out px, out py);
        OnlineMapsUtils.LatLongToTiled(px, py, api.zoom, out px, out py);
        double rx = (rect.center.x - position.x) / rect.width * 2;
        double ry = (rect.center.y - position.y) / rect.height * 2;
        px -= countX / 2f * rx;
        py += countY / 2f * ry;
        OnlineMapsUtils.TileToLatLong(px, py, api.zoom, out lng, out lat);
        return true;
    }

    public override Rect GetRect()
    {
        return gTexture.GetScreenRect();
    }

    protected override bool HitTest()
    {
        return gTexture.HitTest(Input.mousePosition);
    }

    protected override void OnEnableLate()
    {
        if (gTexture == null)
        {
            Debug.LogError("Can not find GUITexture.");
            Destroy(this);
        }
    }

    public override void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        gTexture.texture = texture;
    }
}