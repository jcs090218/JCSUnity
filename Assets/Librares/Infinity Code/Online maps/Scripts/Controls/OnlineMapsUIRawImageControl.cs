/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

#if !UNITY_4_3 && !UNITY_4_5
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

using RawImage = UnityEngine.UI.RawImage;
using RenderMode = UnityEngine.RenderMode;

/// <summary>
/// Class control the map for the uGUI UI RawImage.
/// </summary>
[AddComponentMenu("Infinity Code/Online Maps/Controls/UI RawImage")]
// ReSharper disable once UnusedMember.Global
public class OnlineMapsUIRawImageControl : OnlineMapsControlBase2D
{
    private RawImage image;

    /// <summary>
    /// Singleton instance of OnlineMapsUIRawImageControl control.
    /// </summary>
    public new static OnlineMapsUIRawImageControl instance
    {
        get { return OnlineMapsControlBase.instance as OnlineMapsUIRawImageControl; }
    }

    private Camera worldCamera
    {
        get
        {
            if (image.canvas.renderMode == UnityEngine.RenderMode.ScreenSpaceOverlay) return null;
            return image.canvas.worldCamera;
        }
    }

    protected override void BeforeUpdate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
        int touchCount = Input.GetMouseButton(0) ? 1 : 0;
        if (touchCount != lastTouchCount)
        {
            if (touchCount == 1) OnMapBasePress();
            else if (touchCount == 0) OnMapBaseRelease();
        }
        lastTouchCount = touchCount;
#else
        if (Input.touchCount != lastTouchCount)
        {
            if (Input.touchCount == 1) OnMapBasePress();
            else if (Input.touchCount == 0) OnMapBaseRelease();
        }
        lastTouchCount = Input.touchCount;
#endif
    }

    public override Vector2 GetCoords(Vector2 position)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, position, worldCamera)) return Vector2.zero;

        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, position, worldCamera, out point);

        Rect rect = image.GetPixelAdjustedRect();

        Vector2 size = (rect.max - point);
        size.x = size.x / rect.size.x;
        size.y = size.y / rect.size.y;

        Vector2 r = new Vector2((size.x - .5f), (size.y - .5f));

        int countX = api.width / OnlineMapsUtils.tileSize;
        int countY = api.height / OnlineMapsUtils.tileSize;

        double px, py;
        api.GetPosition(out px, out py);
        OnlineMapsUtils.LatLongToTiled(px, py, api.zoom, out px, out py);

        px -= countX * r.x;
        py += countY * r.y;

        OnlineMapsUtils.TileToLatLong(px, py, api.zoom, out px, out py);
        return new Vector2((float)px, (float)py);
    }

    public override bool GetCoords(out double lng, out double lat, Vector2 position)
    {
        lng = lat = 0;
        if (!RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, position, worldCamera)) return false;

        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, position, worldCamera, out point);

        Rect rect = image.GetPixelAdjustedRect();

        Vector2 size = (rect.max - point);
        size.x = size.x / rect.size.x;
        size.y = size.y / rect.size.y;

        Vector2 r = new Vector2((size.x - .5f), (size.y - .5f));

        int countX = api.width / OnlineMapsUtils.tileSize;
        int countY = api.height / OnlineMapsUtils.tileSize;

        double px, py;
        api.GetPosition(out px, out py);
        OnlineMapsUtils.LatLongToTiled(px, py, api.zoom, out px, out py);

        px -= countX * r.x;
        py += countY * r.y;

        OnlineMapsUtils.TileToLatLong(px, py, api.zoom, out lng, out lat);
        return true;
    }

    public override Rect GetRect()
    {
        RectTransform rectTransform = (RectTransform)transform;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        float xMin = float.PositiveInfinity, xMax = float.NegativeInfinity, yMin = float.PositiveInfinity, yMax = float.NegativeInfinity;
        for (int i = 0; i < 4; ++i)
        {
            Vector3 screenCoord = RectTransformUtility.WorldToScreenPoint(worldCamera, corners[i]);
            if (screenCoord.x < xMin) xMin = screenCoord.x;
            if (screenCoord.x > xMax) xMax = screenCoord.x;
            if (screenCoord.y < yMin) yMin = screenCoord.y;
            if (screenCoord.y > yMax) yMax = screenCoord.y;
            corners[i] = screenCoord;
        }
        Rect result = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        return result;
    }

    protected override bool HitTest()
    {
        PointerEventData pe = new PointerEventData(EventSystem.current);
        pe.position = Input.mousePosition;
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pe, hits);

        if (hits.Count > 0 && hits[0].gameObject != gameObject) return false;
        return RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, Input.mousePosition, worldCamera);
    }

    protected override void OnEnableLate()
    {
        image = GetComponent<RawImage>();
        if (image == null)
        {
            Debug.LogError("Can not find Image.");
            Destroy(this);
        }
    }

    public override void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        image.texture = texture;
    }
}
#endif