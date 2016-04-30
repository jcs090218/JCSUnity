/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;

/// <summary>
/// Instance of Billboard marker.
/// </summary>
[Serializable]
[AddComponentMenu("")]
public class OnlineMapsMarkerBillboard : OnlineMapsMarkerInstanceBase
{
    /// <summary>
    /// Indicates whether to display the marker.
    /// </summary>
    public bool used;

    /// <summary>
    /// Creates a new instance of the billboard marker.
    /// </summary>
    /// <param name="marker">Marker</param>
    /// <returns>Instance of billboard marker</returns>
    public static OnlineMapsMarkerBillboard Create(OnlineMapsMarker marker)
    {
        GameObject billboardGO = new GameObject("Marker");
        SpriteRenderer spriteRenderer = billboardGO.AddComponent<SpriteRenderer>();
        OnlineMapsMarkerBillboard billboard = billboardGO.AddComponent<OnlineMapsMarkerBillboard>();
        
        billboard.marker = marker;
        Texture2D texture = marker.texture;
        if (marker.texture == null) texture = OnlineMaps.instance.defaultMarkerTexture;
        if (texture != null) spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0));
        
        return billboard;
    }

    /// <summary>
    /// Dispose billboard instance
    /// </summary>
    public void Dispose()
    {
        DestroyImmediate(gameObject);
        marker = null;
    }

#if !UNITY_ANDROID
    // ReSharper disable once UnusedMember.Global
    protected void OnMouseDown()
    {
        OnlineMapsControlBase.instance.InvokeBasePress();
    }

    // ReSharper disable once UnusedMember.Global
    protected void OnMouseUp()
    {
        OnlineMapsControlBase.instance.InvokeBaseRelease();
    }
#endif

    private void Start()
    {
        gameObject.AddComponent<BoxCollider>();
        Update();
    }

    private void Update()
    {
        transform.LookAt(OnlineMapsControlBase3D.instance.activeCamera.transform.position);
        Vector3 euler = transform.rotation.eulerAngles;
        euler.y = 0;
        transform.rotation = Quaternion.Euler(euler);
    }
}