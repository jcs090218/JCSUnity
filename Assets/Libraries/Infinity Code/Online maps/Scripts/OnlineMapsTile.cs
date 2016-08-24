/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Object = UnityEngine.Object;

#if !UNITY_WEBGL
using System.Threading;
#endif

/// <summary>
/// This class of buffer tile image. \n
/// <strong>Please do not use it if you do not know what you're doing.</strong> \n
/// Perform all operations with the map through other classes.
/// </summary>
[Serializable]
public class OnlineMapsTile
{
    public delegate string OnGetResourcesPathDelegate(OnlineMapsTile tile);

    /// <summary>
    /// Buffer default colors.
    /// </summary>
    public static Color[] defaultColors;

    /// <summary>
    /// The event, which allows you to control the path of tile in Resources.
    /// </summary>
    public static OnGetResourcesPathDelegate OnGetResourcesPath;

    /// <summary>
    /// The event, which occurs after a successful download of the tile.
    /// </summary>
    public static Action<OnlineMapsTile> OnTileDownloaded;

    /// <summary>
    /// The event, which occurs after a successful download of the traffic texture.
    /// </summary>
    public static Action<OnlineMapsTile> OnTrafficDownloaded;

    [NonSerialized]
    private static List<OnlineMapsTile> _tiles;

    /// <summary>
    /// This event occurs when the tile gets colors based on parent colors.
    /// </summary>
    [NonSerialized]
    public Action<OnlineMapsTile> OnSetColor;

    public static OnlineMaps api;

    /// <summary>
    /// The coordinates of the bottom-right corner of the tile.
    /// </summary>
    public Vector2 bottomRight;

    /// <summary>
    /// In this variable you can put any data that you need to work with tile.
    /// </summary>
    public object customData;

    public byte[] data;

    /// <summary>
    /// The coordinates of the center point of the tile.
    /// </summary>
    public Vector2 globalPosition;

    public bool hasColors;
    public bool isMapTile;
    public bool labels;

    /// <summary>
    /// Language used in tile
    /// </summary>
    public string language;

    /// <summary>
    /// Reference to parent tile.
    /// </summary>
    [NonSerialized]
    public OnlineMapsTile parent;
    public short priority;

    /// <summary>
    /// Provider used in tile.
    /// </summary>
    public OnlineMapsProviderEnum provider;

    /// <summary>
    /// Status of tile.
    /// </summary>
    public OnlineMapsTileStatus status = OnlineMapsTileStatus.none;

    /// <summary>
    /// Texture of tile.
    /// </summary>
    public Texture2D texture;

    /// <summary>
    /// The coordinates of the top-left corner of the tile.
    /// </summary>
    public Vector2 topLeft;

    /// <summary>
    /// Traffic texture.
    /// </summary>
    public Texture2D trafficTexture;

    /// <summary>
    /// URL from which will be downloaded traffic texture.
    /// </summary>
    public string trafficURL;

    /// <summary>
    /// Instance of the traffic loader.
    /// </summary>
    public WWW trafficWWW;

    /// <summary>
    /// Type used in tile.
    /// </summary>
    public int type;
    public bool used = true;

    /// <summary>
    /// Instance of the texture loader.
    /// </summary>
    public WWW www;

    /// <summary>
    /// Tile X.
    /// </summary>
    public int x;

    /// <summary>
    /// Tile Y.
    /// </summary>
    public int y;

    /// <summary>
    /// Tile zoom.
    /// </summary>
    public int zoom;

    private string _cacheFilename;
    private Color[] _colors;
    private string _url;

    [NonSerialized]
    private OnlineMapsTile[] childs = new OnlineMapsTile[4];
    private bool hasChilds;
    private byte[] labelData;
    private Color[] labelColors;

    /// <summary>
    /// Array of colors of the tile.
    /// </summary>
    public Color[] colors
    {
        get
        {
            if (_colors != null) return _colors;
            return defaultColors;
        }
    }

    /// <summary>
    /// Path in Resources, from where the tile will be loaded.
    /// </summary>
    public string resourcesPath
    {
        get
        {
            if (OnGetResourcesPath != null) return OnGetResourcesPath(this);
            return Regex.Replace(api.resourcesPath, @"{\w+}", CustomProviderReplaceToken);
        }
    }

    /// <summary>
    /// List of all tiles.
    /// </summary>
    public static List<OnlineMapsTile> tiles
    {
        get { return _tiles ?? (_tiles = new List<OnlineMapsTile>()); }
        set { _tiles = value; }
    }

    /// <summary>
    /// URL from which will be downloaded texture.
    /// </summary>
    public string url
    {
        get
        {
            if (string.IsNullOrEmpty(_url))
            {
                if (provider == OnlineMapsProviderEnum.arcGis) InitArcGis();
                else if (provider == OnlineMapsProviderEnum.google) InitGoogle();
                else if (provider == OnlineMapsProviderEnum.mapQuest) InitMapQuest();
                else if (provider == OnlineMapsProviderEnum.nokia) InitNokia();
                else if (provider == OnlineMapsProviderEnum.openStreetMap) InitOpenStreetMap();
                else if (provider == OnlineMapsProviderEnum.virtualEarth) InitVirtualEarth();
                else if (provider == OnlineMapsProviderEnum.sputnik) InitSputnik();
                else if (provider == OnlineMapsProviderEnum.aMap) InitAMap();
                else if (provider == OnlineMapsProviderEnum.custom) InitCustom();
            }
            return _url;
        }
        set { _url = value; }
    }

    public OnlineMapsTile(int x, int y, int zoom, OnlineMaps api, bool isMapTile = true)
    {
        int maxX = 2 << (zoom - 1);
        if (x < 0) x += maxX;
        else if (x >= maxX) x -= maxX;

        this.x = x;
        this.y = y;
        this.zoom = zoom;

        OnlineMapsTile.api = api;
        this.isMapTile = isMapTile;

        provider = api.provider;
        type = api.type;
        labels = api.labels;
        language = api.language;

        topLeft = OnlineMapsUtils.TileToLatLong(x, y, zoom);
        bottomRight = OnlineMapsUtils.TileToLatLong(x + 1, y + 1, zoom);
        globalPosition = Vector2.Lerp(topLeft, bottomRight, 0.5f);

        trafficURL = String.Format("https://mts0.google.com/vt?pb=!1m4!1m3!1i{0}!2i{1}!3i{2}!2m3!1e0!2sm!3i301114286!2m6!1e2!2straffic!4m2!1soffset_polylines!2s0!5i1!2m12!1e2!2spsm!4m2!1sgid!2sl0t0vMkIqfb3hBb090479A!4m2!1ssp!2s1!5i1!8m2!13m1!14b1!3m25!2sru-RU!3sUS!5e18!12m1!1e50!12m3!1e37!2m1!1ssmartmaps!12m5!1e14!2m1!1ssolid!2m1!1soffset_polylines!12m4!1e52!2m2!1sentity_class!2s0S!12m4!1e26!2m2!1sstyles!2zcy5lOmx8cC52Om9mZixzLnQ6MXxwLnY6b2ZmLHMudDozfHAudjpvZmY!4e0", zoom, x, y);

        if (isMapTile) tiles.Add(this);
    }

    public OnlineMapsTile(int x, int y, int zoom, OnlineMaps api, OnlineMapsTile parent)
        : this(x, y, zoom, api)
    {
        this.parent = parent;
        if (parent != null && parent.status == OnlineMapsTileStatus.loaded) parent.SetChildColor(this);
    }

    public void ApplyColorsToChilds()
    {
        if (OnSetColor != null) OnSetColor(this);
        if (hasChilds && hasColors)
        {
            foreach (OnlineMapsTile child in childs)
                if (child != null && (child.status != OnlineMapsTileStatus.loaded)) SetChildColor(child);
        }
    }

    private void ApplyLabelTexture()
    {
        Texture2D t = new Texture2D(OnlineMapsUtils.tileSize, OnlineMapsUtils.tileSize);
        t.LoadImage(labelData);
        labelData = null;
        labelColors = t.GetPixels();
        
        if (api.target == OnlineMapsTarget.texture)
        {
#if !UNITY_WEBGL
            OnlineMapsThreadManager.AddThreadAction(MergeColors);
#else
            MergeColors();
#endif
            Object.Destroy(t);
        }
        else
        {
            _colors = texture.GetPixels();
            MergeColors();
            t.SetPixels(_colors);
            texture = t;
            _colors = null;
        }
    }

    public void ApplyTexture(Texture2D texture)
    {
        _colors = texture.GetPixels();
        status = OnlineMapsTileStatus.loaded;
        hasColors = true;

        if (parent != null && parent.status != OnlineMapsTileStatus.loaded) parent.hasColors = false;
        if (childs == null) return;

        foreach (OnlineMapsTile child in childs)
        {
            if (child != null && child.status != OnlineMapsTileStatus.loaded) child.hasColors = false;
        }
    }

    public void CheckTextureSize(Texture2D texture)
    {
        if (texture == null) return;
        if (api.provider == OnlineMapsProviderEnum.custom && (texture.width != 256 || texture.height != 256))
        {
            Debug.LogError(string.Format("Size tiles {0}x{1}. Expected to 256x256. Please check the URL.", texture.width, texture.height));
            status = OnlineMapsTileStatus.error;
        }
    }

    private string CustomProviderReplaceToken(Match match)
    {
        string v = match.Value.ToLower().Trim('{', '}');
        if (v == "zoom") return zoom.ToString();
        if (v == "x") return x.ToString();
        if (v == "y") return y.ToString();
        if (v == "quad") return OnlineMapsUtils.TileToQuadKey(x, y, zoom);
        return v;
    }

    /// <summary>
    /// Dispose of tile.
    /// </summary>
    public void Dispose()
    {
        if (status == OnlineMapsTileStatus.disposed) return;
        status = OnlineMapsTileStatus.disposed;

        if (www != null)
        {
            //www.Dispose();
            www = null;
        }
        if (trafficWWW != null)
        {
            //trafficWWW.Dispose();
            trafficWWW = null;
        }
        _colors = null;
        _url = null;
        labelData = null;
        labelColors = null;
        data = null;
        texture = null;
        trafficTexture = null;
        OnSetColor = null;
        if (hasChilds) foreach (OnlineMapsTile child in childs) if (child != null) child.parent = null;
        if (parent != null)
        {
            if (parent.childs != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (parent.childs[i] == this)
                    {
                        parent.childs[i] = null;
                        break;
                    }
                }
            }
            parent = null;
        }
        childs = null;
        hasChilds = false;
        hasColors = false;
    }

    public void GetColorsFromChilds()
    {
        if (hasColors) return;
        if (childs == null || childs.Any(c => c == null || c.status != OnlineMapsTileStatus.loaded)) return;

        const int s = OnlineMapsUtils.tileSize;
        const int hs = s / 2;
        _colors = new Color[OnlineMapsUtils.sqrTileSize];

        for (int i = 0; i < 4; i++)
        {
            int cx = i / 2;
            int cy = 1 - i % 2;
            OnlineMapsTile tile = childs[i];
            if (tile == null) OnlineMapsUtils.ApplyColorArray(ref _colors, cx * hs, cy * hs, hs, hs, ref defaultColors, cx * hs, cy * hs);
            else OnlineMapsUtils.ApplyColorArray2(ref _colors, cx * hs, cy * hs, hs, hs, ref tile._colors);
        }
        hasColors = true;
    }

    /// <summary>
    /// Gets rect of the tile.
    /// </summary>
    /// <returns>Rect of the tile.</returns>
    public Rect GetRect()
    {
        return new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
    }

    /// <summary>
    /// Checks whether the tile at the specified coordinates.
    /// </summary>
    /// <param name="tl">Coordinates of top-left corner.</param>
    /// <param name="br">Coordinates of bottom-right corner.</param>
    /// <returns>True - if the tile at the specified coordinates, False - if not.</returns>
    public bool InScreen(Vector2 tl, Vector2 br)
    {
        if (bottomRight.x < tl.x) return false;
        if (topLeft.x > br.x) return false;
        if (topLeft.y < br.y) return false;
        if (bottomRight.y > tl.y) return false;
        return true;
    }

    private void InitAMap()
    {
        string server = "https://webst02.is.autonavi.com/appmaptile?style=6&x={2}&y={1}&z={0}";
        if (type == 1) server = "https://webrd03.is.autonavi.com/appmaptile?lang=zh_cn&size=1&scale=1&style=8&x={2}&y={1}&z={0}";
        _url = String.Format(server, zoom, y, x);
    }

    private void InitArcGis()
    {
        string maptype = "World_Imagery";
        if (type == 1) maptype = "World_Topo_Map";
        const string server =
            "https://server.arcgisonline.com/ArcGIS/rest/services/{3}/MapServer/tile/{0}/{1}/{2}";
        _url = String.Format(server, zoom, y, x, maptype);
    }

    private void InitCustom()
    {
        _url = Regex.Replace(api.customProviderURL, @"{\w+}", CustomProviderReplaceToken);
    }

    private void InitGoogle()
    {
        string server = "https://khm0.google.ru/kh/v=172&src=app&hl={3}&x={0}&y={1}&z={2}&s=";
        if (type == 0)
        {
            if (labels) server = "https://mt0.googleapis.com/vt/lyrs=y&hl={3}&x={0}&y={1}&z={2}";
            else server = "https://khm0.googleapis.com/kh?v=172&hl={3}&x={0}&y={1}&z={2}";
        }
        else if (type == 1) server = "https://mts0.google.com/vt/lyrs=t@131,r@216000000&src=app&hl={3}&x={0}&y={1}&z={2}&s=";
        else if (type == 2) server = "https://mt1.googleapis.com/vt?pb=!1m4!1m3!1i{2}!2i{0}!3i{1}!2m3!1e0!2sm!3i295124088!3m9!2s{3}!3sUS!5e18!12m1!1e47!12m3!1e37!2m1!1ssmartmaps!4e0";
        _url = String.Format(server, x, y, zoom, language);
    }

    private void InitMapQuest()
    {
        string maptype = "sat";
        if (type == 1) maptype = "map";
        const string server = "https://ttiles01.mqcdn.com/tiles/1.0.0/vy/{3}/{0}/{1}/{2}.png";
        _url = String.Format(server, zoom, x, y, maptype);
    }

    private void InitNokia()
    {
        string maptype = "satellite.day";
        if (type == 0 && labels) maptype = "hybrid.day";
        else if (type == 1) maptype = "terrain.day";
        else if (type == 2) maptype = "normal.day";

        const string server =
            "https://{0}.maps.nlp.nokia.com/maptile/2.1/maptile/newest/{4}/{1}/{2}/{3}/256/png8?lg={5}&app_id=xWVIueSv6JL0aJ5xqTxb&app_code=djPZyynKsbTjIUDOBcHZ2g";
        _url = string.Format(server, 1, zoom, x, y, maptype, language);
    }

    private void InitOpenStreetMap()
    {
        const string server = "https://a.tile.openstreetmap.org/{0}/{1}/{2}.png";
        _url = String.Format(server, zoom, x, y);
    }

    private void InitSputnik()
    {
        const string server = "http://tiles.maps.sputnik.ru/tiles/kmt2/{0}/{1}/{2}.png";
        _url = String.Format(server, zoom, x, y);
    }

    private void InitVirtualEarth()
    {
        string quad = OnlineMapsUtils.TileToQuadKey(x, y, zoom);
        string server = "";
        if (type == 0 && !labels)
        {
            server = "https://ak.t0.tiles.virtualearth.net/tiles/a{0}.jpeg?mkt={1}&g=1457&n=z";
        }
        else if (type == 0 && labels)
        {
            server = "https://ak.dynamic.t0.tiles.virtualearth.net/comp/ch/{0}?mkt={1}&it=A,G,L,LA&og=30&n=z";
            
        }
        else if (type == 1)
        {
            server = "https://ak.dynamic.t0.tiles.virtualearth.net/comp/ch/{0}?mkt={1}&it=G,VE,BX,L,LA&og=30&n=z";
        }
        _url = String.Format(server, quad, language);
    }

    public void LoadTexture()
    {
        if (status == OnlineMapsTileStatus.error) return;

        Texture2D texture = new Texture2D(OnlineMapsUtils.tileSize, OnlineMapsUtils.tileSize);
        if (api.useSoftwareJPEGDecoder) LoadTexture(texture, data);
        else
        {
            texture.LoadImage(data);
            texture.wrapMode = TextureWrapMode.Clamp;
        }

        CheckTextureSize(texture);

        if (status != OnlineMapsTileStatus.error)
        {
            ApplyTexture(texture);
            if (labelData != null) ApplyLabelTexture();
        }
        Object.Destroy(texture);
    }

    public static void LoadTexture(Texture2D texture, byte[] bytes)
    {
        if (bytes[0] == 0xFF)
        {
            Color32[] colors = OnlineMapsJPEGDecoder.GetColors(bytes);
            texture.SetPixels32(colors);
            texture.Apply();
        }
        else texture.LoadImage(bytes);
    }

    private void MergeColors()
    {
        try
        {
            if (status == OnlineMapsTileStatus.error || status == OnlineMapsTileStatus.disposed) return;
            if (labelColors == null || _colors == null || labelColors.Length != _colors.Length) return;

            for (int i = 0; i < _colors.Length; i++)
            {
                float a = labelColors[i].a;
                if (a != 0)
                {
                    labelColors[i].a = 1;
                    _colors[i] = Color.Lerp(_colors[i], labelColors[i], a);
                }
            }
        }
        catch
        {
        }
    }

    public void OnDownloadComplete()
    {
        data = www.bytes;
        LoadTexture();
        data = null;
    }

    public void OnDownloadError()
    {
        status = OnlineMapsTileStatus.error;
    }

    public bool OnLabelDownloadComplete()
    {
        labelData = trafficWWW.bytes;
        if (status == OnlineMapsTileStatus.loaded)
        {
            ApplyLabelTexture();
            return true;
        }
        return false;
    }

    private void SetChild(OnlineMapsTile tile)
    {
        int cx = tile.x % 2;
        int cy = tile.y % 2;
        childs[cx * 2 + cy] = tile;
        hasChilds = true;
    }

    public void SetChildColor(OnlineMapsTile child)
    {
        if (child == null) return;

        if (api.target == OnlineMapsTarget.texture) SetChildColorTexture(child);

        if (child.hasChilds)
        {
            foreach (OnlineMapsTile tile in child.childs)
            {
                if (tile != null && tile.status != OnlineMapsTileStatus.loaded) child.SetChildColor(tile);
            }
        }
    }

    private void SetChildColorTexture(OnlineMapsTile child)
    {
        if (!hasColors) return;
        if (child.status == OnlineMapsTileStatus.loading || child.hasColors) return;

        Color[] clrs = colors;

        if (clrs == null) return;
        if (child._colors == null) child._colors = new Color[OnlineMapsUtils.sqrTileSize];

        const int s = OnlineMapsUtils.tileSize;
        const int hs = s / 2;
        int sx = (child.x % 2) * hs;
        int sy = hs - (child.y % 2) * hs;

        int childLength = child._colors.Length;
        int colorLength = clrs.Length;

        for (int py = 0; py < s; py++)
        {
            int spy = py * s;
            int hpy = (py / 2 + sy) * s;
            for (int px = 0; px < s; px++)
            {
                int i1 = spy + px;
                int i2 = hpy + px / 2 + sx;
                if (childLength <= i1 || colorLength <= i2) continue;

                child._colors[i1] = clrs[i2];
            }
        }

        child.hasColors = true;
        if (child.OnSetColor != null) child.OnSetColor(child);
    }

    public void SetParent(OnlineMapsTile tile)
    {
        parent = tile;
        parent.SetChild(this);
    }

    public override string ToString()
    {
        return string.Format("{0}x{1}.jpg", x, y);
    }
}