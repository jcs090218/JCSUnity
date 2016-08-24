/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;
using Sprite = UnityEngine.Sprite;

/// <summary>
/// Class control the map for the SpriteRenderer.
/// </summary>
[AddComponentMenu("Infinity Code/Online Maps/Controls/SpriteRenderer")]
[RequireComponent(typeof(SpriteRenderer))]
// ReSharper disable once UnusedMember.Global
public class OnlineMapsSpriteRendererControl:OnlineMapsControlBase2D
{
    private Collider _cl;
    private Collider2D _cl2D;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Singleton instance of OnlineMapsSpriteRendererControl control.
    /// </summary>
    public new static OnlineMapsSpriteRendererControl instance
    {
        get { return OnlineMapsControlBase.instance as OnlineMapsSpriteRendererControl; }
    }

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

    public Collider2D cl2D
    {
        get
        {
            if (_cl2D == null)
            {
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
                _cl2D = collider2D;
#else
                _cl2D = GetComponent<Collider2D>();
#endif
            }
            return _cl2D;
        }
    }

#if !UNITY_4_3
    public override Vector2 GetCoords(Vector2 position)
    {
        Vector2 coords2D = GetCoords2D(position);
        return (coords2D != Vector2.zero)? coords2D: GetCoords3D(position);
    }

    private Vector2 GetCoords2D(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(position), Mathf.Infinity);
        if (hit.collider == null || hit.collider.gameObject != gameObject) return Vector2.zero;
        if (cl2D == null) return Vector2.zero;

        Vector3 size = (cl2D.bounds.max - new Vector3(hit.point.x, hit.point.y));
        size.x = size.x / cl2D.bounds.size.x;
        size.y = size.y / cl2D.bounds.size.y;

        Vector2 r = new Vector3((size.x - .5f), (size.y - .5f));

        int countX = api.width / OnlineMapsUtils.tileSize;
        int countY = api.height / OnlineMapsUtils.tileSize;

        Vector2 p = OnlineMapsUtils.LatLongToTilef(api.position, api.zoom);
        p.x -= countX * r.x;
        p.y += countY * r.y;

        return OnlineMapsUtils.TileToLatLong(p, api.zoom);
    }
#else
    public override Vector2 GetCoords(Vector2 position)
    {
        return GetCoords3D(position);
    }
#endif

    private Vector2 GetCoords3D(Vector2 position)
    {
        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(position), out hit))
            return Vector2.zero;

        if (hit.collider.gameObject != gameObject) return Vector2.zero;

        Vector3 size = (cl.bounds.max - hit.point);
        size.x = size.x / cl.bounds.size.x;
        size.y = size.y / cl.bounds.size.y;

        Vector2 r = new Vector3((size.x - .5f), (size.y - .5f));

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
        RaycastHit hit;
        lng = lat = 0;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(position), out hit)) return false;

        if (hit.collider.gameObject != gameObject) return false;

        Vector3 size = (cl.bounds.max - hit.point);
        size.x = size.x / cl.bounds.size.x;
        size.y = size.y / cl.bounds.size.y;

        Vector2 r = new Vector3((size.x - .5f), (size.y - .5f));

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

    protected override void OnEnableLate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("Can not find SpriteRenderer.");
            Destroy(this);
        }
    }

    public override void SetTexture(Texture2D texture)
    {
        base.SetTexture(texture);
        MaterialPropertyBlock props = new MaterialPropertyBlock();
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        props.AddTexture("_MainTex", texture);
#else
        props.SetTexture("_MainTex", texture);
#endif
        spriteRenderer.SetPropertyBlock(props);
    }
}