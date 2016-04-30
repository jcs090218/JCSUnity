/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// Class control the map for the Texture.
/// </summary>
[System.Serializable]
[AddComponentMenu("Infinity Code/Online Maps/Controls/Texture")]
[RequireComponent(typeof(MeshRenderer))]
// ReSharper disable once UnusedMember.Global
public class OnlineMapsTextureControl : OnlineMapsControlBase3D
{
    /// <summary>
    /// Singleton instance of OnlineMapsTextureControl control.
    /// </summary>
    public new static OnlineMapsTextureControl instance
    {
        get { return OnlineMapsControlBase.instance as OnlineMapsTextureControl; }
    }

    public override Vector2 GetCoords(Vector2 position)
    {
        RaycastHit hit;
        if (!Physics.Raycast(activeCamera.ScreenPointToRay(position), out hit))
            return Vector2.zero;

        if (hit.collider.gameObject != gameObject) return Vector2.zero;

        Renderer render = hit.collider.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (render == null || render.sharedMaterial == null || render.sharedMaterial.mainTexture == null ||
            meshCollider == null)
            return Vector2.zero;

        Vector2 r = hit.textureCoord;

        r.x = r.x - 0.5f;
        r.y = r.y - 0.5f;

        int countX = api.width / OnlineMapsUtils.tileSize;
        int countY = api.height / OnlineMapsUtils.tileSize;

        double px, py;
        api.GetPosition(out px, out py);
        OnlineMapsUtils.LatLongToTiled(px, py, api.zoom, out px, out py);

        px += countX * r.x;
        py -= countY * r.y;
        OnlineMapsUtils.TileToLatLong(px, py, api.zoom, out px, out py);
        return new Vector2((float)px, (float)py);
    }

    public override bool GetCoords(out double lng, out double lat, Vector2 position)
    {
        RaycastHit hit;

        lng = lat = 0;
        if (!Physics.Raycast(activeCamera.ScreenPointToRay(position), out hit)) return false;

        if (hit.collider.gameObject != gameObject) return false;

        Renderer render = hit.collider.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (render == null || render.sharedMaterial == null || render.sharedMaterial.mainTexture == null || meshCollider == null) return false;

        Vector2 r = hit.textureCoord;

        r.x = r.x - 0.5f;
        r.y = r.y - 0.5f;

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

    public override void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
    }
}