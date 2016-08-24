/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.Collections;
using UnityEngine;

/// <summary>
/// Class control the map for the NGUI.
/// </summary>
[System.Serializable]
[AddComponentMenu("Infinity Code/Online Maps/Controls/NGUI Texture")]
// ReSharper disable once UnusedMember.Global
public class OnlineMapsNGUITextureControl : OnlineMapsControlBase2D
{
#if NGUI
    private UITexture uiTexture;

    /// <summary>
    /// Singleton instance of OnlineMapsNGUITextureControl control.
    /// </summary>
    public new static OnlineMapsNGUITextureControl instance
    {
        get { return OnlineMapsControlBase.instance as OnlineMapsNGUITextureControl; }
    }

    protected override bool allowTouchZoom
    {
        get { return false; }
    }

    public override Rect uvRect
    {
        get { return uiTexture.uvRect; }
    }

    public override Vector2 GetCoords(Vector2 position)
    {
        Rect rect = GetRect();
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
        Rect rect = GetRect();
        int countX = api.texture.width / OnlineMapsUtils.tileSize;
        int countY = api.texture.height / OnlineMapsUtils.tileSize;
    
        double px, py;
        api.GetPosition(out px, out py);
        OnlineMapsUtils.LatLongToTiled(px, py, api.zoom, out px, out py);

        float rx = (rect.center.x - position.x) / rect.width * 2;
        float ry = (rect.center.y - position.y) / rect.height * 2;
        px -= countX / 2f * rx;
        py += countY / 2f * ry;
        
        OnlineMapsUtils.TileToLatLong(px, py, api.zoom, out lng, out lat);
        return true;
    }

    protected override bool HitTest()
    {
        return UICamera.hoveredObject == gameObject;
    }

    protected override void OnEnableLate()
    {
        uiTexture = GetComponent<UITexture>();
        if (uiTexture == null)
        {
            Debug.LogError("Can not find UITexture.");
            Destroy(this);
        }
    }

// ReSharper disable once UnusedMember.Local
    private void OnPress(bool state)
    {
        if (state) OnMapBasePress();
        else OnMapBaseRelease();
    }

    public override Rect GetRect()
    {
        int w = Screen.width / 2;
        int h = Screen.height / 2;

        Bounds b = NGUIMath.CalculateAbsoluteWidgetBounds(uiTexture.transform);

        int rx = Mathf.RoundToInt(b.min.x * h + w);
        int ry = Mathf.RoundToInt((b.min.y + 1) * h);
        int rz = Mathf.RoundToInt(b.size.x * h);
        int rw = Mathf.RoundToInt(b.size.y * h);

        return new Rect(rx, ry, rz, rw);
    }

    public override void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        StartCoroutine(OnFrameEnd(texture));
    }

    public IEnumerator OnFrameEnd(Texture2D texture)
    {
        yield return new WaitForEndOfFrame();
        uiTexture.mainTexture = texture;
    }
#endif
}