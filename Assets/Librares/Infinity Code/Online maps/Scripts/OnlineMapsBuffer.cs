/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if !UNITY_WEBGL
using System.Threading;
#endif

/// <summary>
/// This class is responsible for drawing the map.\n
/// <strong>Please do not use it if you do not know what you're doing.</strong>\n
/// Perform all operations with the map through other classes.
/// </summary>
[Serializable]
public class OnlineMapsBuffer
{
    public delegate IEnumerable<OnlineMapsMarker> SortMarkersDelegate(IEnumerable<OnlineMapsMarker> markers);

    /// <summary>
    /// Allows you to manually control the sorting marker in a mode Drawing to Texture.
    /// </summary>
    public static SortMarkersDelegate OnSortMarker;

    /// <summary>
    /// Reference to OnlineMaps.
    /// </summary>
    public OnlineMaps api;

    /// <summary>
    /// Zoom for which the map displayed.
    /// </summary>
    public int apiZoom;

    /// <summary>
    /// Position the tile, which begins buffer.
    /// </summary>
    public OnlineMapsVector2i bufferPosition;

    public Color[] frontBuffer;

    public bool generateSmartBuffer = false;

    /// <summary>
    /// Height of the map.
    /// </summary>
    public int height;

    /// <summary>
    /// List of tiles that are already loaded, but not yet applied to the buffer.
    /// </summary>
    public List<OnlineMapsTile> newTiles;

    /// <summary>
    /// Type redraw the map.
    /// </summary>
    public OnlineMapsRedrawType redrawType;

    /// <summary>
    /// The current status of the buffer.
    /// </summary>
    public OnlineMapsBufferStatus status = OnlineMapsBufferStatus.wait;
    public Color[] smartBuffer;
    public bool updateBackBuffer;

    /// <summary>
    /// Width of the map.
    /// </summary>
    public int width;

    private Color[] backBuffer;
    private int bufferZoom;
    private bool disposed;
    private OnlineMapsVector2i frontBufferPosition;
    private Dictionary<int, OnlineMapsBufferZoom> zooms;
    private bool needUnloadTiles;
    private double apiLongitude;
    private double apiLatitude;

    /// <summary>
    /// Position for which the map displayed.
    /// </summary>
    public Vector2 apiPosition
    {
        get
        {
            return new Vector2((float)apiLongitude, (float)apiLatitude);
        }
    }

    /// <summary>
    /// The coordinates of the top-left the point of map that displays.
    /// </summary>
    public Vector2 topLeftPosition
    {
        get
        {
            int countX = api.width / OnlineMapsUtils.tileSize;
            int countY = api.height / OnlineMapsUtils.tileSize;

            double px, py;
            OnlineMapsUtils.LatLongToTiled(apiLongitude, apiLatitude, apiZoom, out px, out py);

            px -= countX / 2f;
            py -= countY / 2f;

            OnlineMapsUtils.TileToLatLong(px, py, apiZoom, out px, out py);
            return new Vector2((float)px, (float)py);
        }
    }

    public OnlineMapsBuffer(OnlineMaps api)
    {
        this.api = api;
        newTiles = new List<OnlineMapsTile>();
    }

    private void ApplyNewTiles()
    {
        if (newTiles == null || newTiles.Count == 0) return;

        lock (newTiles)
        {
            foreach (OnlineMapsTile tile in newTiles)
            {
                if (tile.status == OnlineMapsTileStatus.disposed) continue;

#if !UNITY_WEBGL
                int counter = 20;
                while (tile.colors.Length < OnlineMapsUtils.sqrTileSize && counter > 0)
                {
                    Thread.Sleep(1);
                    counter--;
                }
#endif
                tile.ApplyColorsToChilds();
            }
            if (newTiles.Count > 0) newTiles.Clear();
        }
    }

    /// <summary>
    /// Adds a tile into the buffer.
    /// </summary>
    /// <param name="tile">Tile</param>
    public void ApplyTile(OnlineMapsTile tile)
    {
        if (newTiles == null) newTiles = new List<OnlineMapsTile>();
        lock (newTiles)
        {
            newTiles.Add(tile);
        }
    }

    private List<OnlineMapsTile> CreateParents(List<OnlineMapsTile> tiles, int zoom)
    {
        List<OnlineMapsTile> newParentTiles = new List<OnlineMapsTile>();

        OnlineMapsBufferZoom parentZoom;
        if (!zooms.ContainsKey(zoom))
        {
            parentZoom = new OnlineMapsBufferZoom(zoom);
            zooms[zoom] = parentZoom;
        }
        else
        {
            parentZoom = zooms[zoom];
        }

        foreach (OnlineMapsTile tile in tiles)
        {
            if (tile.parent == null) CreateTileParent(zoom, tile, parentZoom, newParentTiles);
            else newParentTiles.Add(tile.parent);
            tile.used = true;
            tile.parent.used = true;
        }

        if (api.target == OnlineMapsTarget.texture)
        {
            foreach (OnlineMapsTile tile in newParentTiles)
            {
                if (tile.status == OnlineMapsTileStatus.loaded) continue;
                tile.GetColorsFromChilds();
            }
        }

        return newParentTiles;
    }

    private void CreateTileParent(int zoom, OnlineMapsTile tile, OnlineMapsBufferZoom parentZoom,
        List<OnlineMapsTile> newParentTiles)
    {
        int px = tile.x / 2;
        int py = tile.y / 2;

        OnlineMapsTile parent =
            parentZoom.tiles.FirstOrDefault(t => t.x == px && t.y == py);
        if (parent == null)
        {
            parent = new OnlineMapsTile(px, py, zoom, api) {OnSetColor = OnTileSetColor};
            parentZoom.tiles.Add(parent);
        }

        newParentTiles.Add(parent);
        parent.used = true;
        tile.SetParent(parent);
        if (parent.hasColors && tile.status != OnlineMapsTileStatus.loaded) parent.SetChildColor(tile);
    }

    /// <summary>
    /// Dispose of buffer.
    /// </summary>
    public void Dispose()
    {
        try
        {
            lock (OnlineMapsTile.tiles)
            {
                foreach (OnlineMapsTile tile in OnlineMapsTile.tiles) tile.Dispose();
                OnlineMapsTile.tiles = null;
            }

            frontBuffer = null;
            backBuffer = null;
            smartBuffer = null;

            status = OnlineMapsBufferStatus.disposed;
            newTiles = null;
            zooms = null;
            disposed = true;
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
        
    }

    public void GenerateFrontBuffer()
    {
        try
        {
            api.GetPosition(out apiLongitude, out apiLatitude);
            apiZoom = api.zoom;
            while (true)
            {
#if !UNITY_WEBGL
                while (status != OnlineMapsBufferStatus.start && api.renderInThread)
                {
                    if (disposed) return;
                    Thread.Sleep(1);
                }
#endif
                status = OnlineMapsBufferStatus.working;
                double px = 0, py = 0;

                try
                {
                    api.GetPosition(out px, out py);
                    int zoom = api.zoom;

                    bool fullRedraw = redrawType == OnlineMapsRedrawType.full;

                    if (disposed)
                    {
                        fullRedraw = true;
                        disposed = false;
                    }
                    else if (newTiles != null && api.target == OnlineMapsTarget.texture) ApplyNewTiles();

                    bool backBufferUpdated = UpdateBackBuffer(px, py, zoom, fullRedraw);

                    if (api.target == OnlineMapsTarget.texture)
                    {
                        GetFrontBufferPosition(px, py, bufferPosition, zoom, api.width, api.height);

                        if (backBufferUpdated)
                        {
                            foreach (OnlineMapsDrawingElement element in api.drawingElements)
                                element.Draw(backBuffer, bufferPosition, width, height, zoom);
                            SetMarkersToBuffer(api.markers);
                        }

                        if (!api.useSmartTexture || !generateSmartBuffer) UpdateFrontBuffer(api.width, api.height);
                        else UpdateSmartBuffer(api.width, api.height);
                    }
                }
                catch (Exception exception)
                {
                    Debug.Log(exception.Message + "\n" + exception.StackTrace);
                }

                status = OnlineMapsBufferStatus.complete;
                apiLongitude = px;
                apiLatitude = py;
                apiZoom = api.zoom;

                if (needUnloadTiles) UnloadOldTiles();

#if !UNITY_WEBGL
                if (api.renderInThread && !api.needGC) GC.Collect();
                if (!api.renderInThread) break;
#else
                break;
#endif
            }
        }
        catch
        {
        }

    }

    private OnlineMapsBufferZoom GetActiveZoom(int zoom)
    {
        OnlineMapsBufferZoom activeZoom;

        if (zooms == null) zooms = new Dictionary<int, OnlineMapsBufferZoom>();

        if (!zooms.ContainsKey(zoom))
        {
            activeZoom = new OnlineMapsBufferZoom(zoom);
            zooms[zoom] = activeZoom;
            return activeZoom;
        }
        return zooms[zoom];
    }

    private OnlineMapsVector2i GetBackBufferPosition(double px, double py, OnlineMapsVector2i _bufferPosition, int zoom, int apiWidth, int apiHeight)
    {
        OnlineMapsUtils.LatLongToTiled(px, py, zoom, out px, out py);

        int countX = apiWidth / OnlineMapsUtils.tileSize + 2;
        int countY = apiHeight / OnlineMapsUtils.tileSize + 2;

        px -= countX / 2f + _bufferPosition.x - 1;
        py -= countY / 2f + _bufferPosition.y - 1;

        int ix = (int) ((px / countX) * width);
        int iy = (int) ((py / countY) * height);

        return new OnlineMapsVector2i(ix, iy);
    }

    private void GetFrontBufferPosition(double px, double py, OnlineMapsVector2i _bufferPosition, int zoom, int apiWidth,
        int apiHeight)
    {
        OnlineMapsVector2i pos = GetBackBufferPosition(px, py, _bufferPosition, zoom, apiWidth, apiHeight);
        int ix = pos.x;
        int iy = pos.y;

        if (iy < 0) iy = 0;
        else if (iy >= height - apiHeight) iy = height - apiHeight;

        frontBufferPosition = new OnlineMapsVector2i(ix, iy);
    }

    private Rect GetMarkerRect(OnlineMapsMarker marker)
    {
        const int s = OnlineMapsUtils.tileSize;
        Vector2 p = OnlineMapsUtils.LatLongToTilef(marker.position, bufferZoom);
        p.x -= bufferPosition.x;
        p.y -= bufferPosition.y;
        OnlineMapsVector2i ip = marker.GetAlignedPosition(new OnlineMapsVector2i((int)(p.x * s), (int)(p.y * s)));
        return new Rect(ip.x, ip.y, marker.width, marker.height);
    }

    private void InitTile(int zoom, OnlineMapsBufferZoom activeZoom, OnlineMapsVector2i pos, int maxY,
        List<OnlineMapsTile> newBaseTiles,
        int y, IEnumerable<OnlineMapsTile> ts, int px)
    {
        int py = y + pos.y;
        if (py < 0 || py >= maxY) return;

        OnlineMapsTile tile = null;

        foreach (OnlineMapsTile t in ts)
        {
            if (t.x == px && t.y == py)
            {
                tile = t;
                break;
            }
        }

        if (tile == null)
        {
            OnlineMapsTile parent = null;

            if (!api.useCurrentZoomTiles && zooms.ContainsKey(zoom - 1))
            {
                int ptx = px / 2;
                int pty = py / 2;
                foreach (OnlineMapsTile t in zooms[zoom - 1].tiles)
                {
                    if (t.x == ptx && t.y == pty)
                    {
                        parent = t;
                        parent.used = true;
                        break;
                    }
                }
            }

            tile = new OnlineMapsTile(px, py, zoom, api, parent) { OnSetColor = OnTileSetColor };
            activeZoom.tiles.Add(tile);
        }
        newBaseTiles.Add(tile);
        tile.used = true;
        if (api.target == OnlineMapsTarget.texture) SetBufferTile(tile);
    }

    private void InitTiles(int zoom, OnlineMapsBufferZoom activeZoom, int countX, OnlineMapsVector2i pos, int countY,
        int maxY, List<OnlineMapsTile> newBaseTiles)
    {
        //IEnumerable<OnlineMapsTile> tiles = OnlineMapsTile.tiles.Where(t => t.provider == api.provider && t.zoom == activeZoom.id && t.type == api.type);
        IEnumerable<OnlineMapsTile> tiles = activeZoom.tiles;

        int maxX = 1 << bufferZoom;
        for (int x = 0; x < countX; x++)
        {
            int px = x + pos.x;
            if (px < 0) px += maxX;
            else if (px >= maxX) px -= maxX;

            for (int y = 0; y < countY; y++) InitTile(zoom, activeZoom, pos, maxY, newBaseTiles, y, tiles, px);
        }
    }

    private void OnTileSetColor(OnlineMapsTile tile)
    {
        if (tile.zoom == bufferZoom) SetBufferTile(tile);
    }

    private Rect SetBufferTile(OnlineMapsTile tile)
    {
        if (api.target == OnlineMapsTarget.tileset) return default(Rect);

        const int s = OnlineMapsUtils.tileSize;
        int i = 0;
        int px = tile.x - bufferPosition.x;
        int py = tile.y - bufferPosition.y;

        int maxX = 2 << (tile.zoom - 1);

        if (px < 0) px += maxX;
        else if (px >= maxX) px -= maxX;

        px *= s;
        py *= s;

        if (px + s < 0 || py + s < 0 || px > width || py > height) return new Rect(0, 0, 0, 0);

        Color[] colors = tile.colors;

        lock (colors)
        {
            int maxSize = width * height;

            for (int y = py + s - 1; y >= py; y--)
            {
                int bp = y * width + px;
                if (bp + s < 0 || bp >= maxSize) continue;
                int l = s;
                if (bp < 0)
                {
                    l -= bp;
                    bp = 0;
                }
                else if (bp + s > maxSize)
                {
                    l -= maxSize - (bp + s);
                    bp = maxSize - s - 1;
                }

                try
                {
                    Array.Copy(colors, i, backBuffer, bp, l);
                }
                catch
                {
                }

                i += s;
            }

            return new Rect(px, py, OnlineMapsUtils.tileSize, OnlineMapsUtils.tileSize);
        }
    }

    private void SetColorToBuffer(Color clr, OnlineMapsVector2i ip, int y, int x)
    {
        if (clr.a == 0) return;
        int bufferIndex = (ip.y + y) * width + ip.x + x;
        if (clr.a < 1)
        {
            float alpha = clr.a;
            Color bufferColor = backBuffer[bufferIndex];
            clr.a = 1;
            clr.r = Mathf.Lerp(bufferColor.r, clr.r, alpha);
            clr.g = Mathf.Lerp(bufferColor.g, clr.g, alpha);
            clr.b = Mathf.Lerp(bufferColor.b, clr.b, alpha);
        }
        backBuffer[bufferIndex] = clr;
    }

    private void SetMarkerToBuffer(OnlineMapsMarker marker, Vector2 startPos, Vector2 endPos)
    {
        const int s = OnlineMapsUtils.tileSize;
        float mx = marker.position.x;
        if (!(((mx > startPos.x && mx < endPos.x) || (mx + 360 > startPos.x && mx + 360 < endPos.x) ||
             (mx - 360 > startPos.x && mx - 360 < endPos.x)) &&
            marker.position.y < startPos.y && marker.position.y > endPos.y)) return;

#if !UNITY_WEBGL
        int maxCount = 20;
        while (marker.locked && maxCount > 0)
        {
            Thread.Sleep(1);
            maxCount--;
        }
#endif

        marker.locked = true;
        Vector2 p = OnlineMapsUtils.LatLongToTilef(marker.position, bufferZoom);
        p -= bufferPosition;

        int maxX = 1 << bufferZoom;

        if (p.x < 0) p.x += maxX;
        else if (p.x > maxX) p.x -= maxX;

        OnlineMapsVector2i ip = marker.GetAlignedPosition(new OnlineMapsVector2i((int) (p.x * s), (int) (p.y * s)));

        Color[] markerColors = marker.colors;
        if (markerColors == null || markerColors.Length == 0) return;

        int markerWidth = marker.width;
        int markerHeight = marker.height;

        for (int y = 0; y < marker.height; y++)
        {
            if (ip.y + y < 0 || ip.y + y >= height) continue;

            int my = (markerHeight - y - 1) * markerWidth;
            
            for (int x = 0; x < marker.width; x++)
            {
                if (ip.x + x < 0 || ip.x + x >= width) continue;
            
                try
                {
                    SetColorToBuffer(markerColors[my + x], ip, y, x);
                }
                catch
                {
                }
            }
        }

        marker.locked = false;
    }

    public void SetMarkersToBuffer(IEnumerable<OnlineMapsMarker> markers)
    {
        if (OnlineMapsControlBase.instance is OnlineMapsControlBase3D)
        {
            if (((OnlineMapsControlBase3D) OnlineMapsControlBase.instance).marker2DMode ==
                OnlineMapsMarker2DMode.billboard)
            {
                return;
            }
        }

        const int s = OnlineMapsUtils.tileSize;
        int countX = api.width / s + 2;
        int countY = api.height / s + 2;
        Vector2 startPos = OnlineMapsUtils.TileToLatLong(bufferPosition.x, bufferPosition.y, bufferZoom);
        Vector2 endPos = OnlineMapsUtils.TileToLatLong(bufferPosition.x + countX, bufferPosition.y + countY + 1,
            bufferZoom);

        if (endPos.x < startPos.x) endPos.x += 360;

        IEnumerable<OnlineMapsMarker> usedMarkers = markers.Where(m => m.enabled && m.range.InRange(bufferZoom));
        if (OnSortMarker != null) usedMarkers = OnSortMarker(usedMarkers);
        else usedMarkers = usedMarkers.OrderByDescending(m => m.position.y);

        foreach (OnlineMapsMarker marker in usedMarkers) SetMarkerToBuffer(marker, startPos, endPos);
    }

    private void UnloadOldTiles()
    {
        needUnloadTiles = false;
        bool hasOld = false;

        foreach (KeyValuePair<int, OnlineMapsBufferZoom> zoom in zooms)
        {
            List<OnlineMapsTile> tiles = zoom.Value.tiles;
            foreach (OnlineMapsTile tile in tiles)
            {
                if (tile.used) continue;

                tile.Dispose();
                hasOld = true;
            }
            tiles.RemoveAll(t => t.status == OnlineMapsTileStatus.disposed);
        }

        lock (OnlineMapsTile.tiles)
        {
            if (hasOld)
            {
                OnlineMapsTile.tiles.RemoveAll(t => t.status == OnlineMapsTileStatus.disposed);
                api.needGC = true;
            }
        }
    }

    private bool UpdateBackBuffer(double px, double py, int zoom, bool fullRedraw)
    {
        const int s = OnlineMapsUtils.tileSize;
        int countX = api.width / s + 2;
        int countY = api.height / s + 2;

        OnlineMapsVector2i pos = OnlineMapsUtils.LatLongToTile(px, py, zoom);
        pos.x -= countX / 2;
        pos.y -= countY / 2;

        int maxY = (2 << zoom) / 2;

        if (pos.y < 0) pos.y = 0;
        if (pos.y >= maxY - countY - 1) pos.y = maxY - countY - 1;

        if (api.target == OnlineMapsTarget.texture)
        {
            if (frontBuffer == null || frontBuffer.Length != api.width * api.height)
            {
                frontBuffer = new Color[api.width * api.height];
                fullRedraw = true;
            }

            if (backBuffer == null || width != countX * s || height != countY * s)
            {
                width = countX * s;
                height = countY * s;
                backBuffer = new Color[height * width];

                fullRedraw = true;
            }
        }

        if (!updateBackBuffer && !fullRedraw && bufferZoom == zoom && bufferPosition != null &&
            bufferPosition == pos) return false;

        updateBackBuffer = false;

        bufferPosition = pos;
        bufferZoom = zoom;

        OnlineMapsBufferZoom activeZoom = GetActiveZoom(zoom);

        List<OnlineMapsTile> newBaseTiles = new List<OnlineMapsTile>();

        lock (OnlineMapsTile.tiles)
        {
            foreach (OnlineMapsTile tile in OnlineMapsTile.tiles) tile.used = false;

            InitTiles(zoom, activeZoom, countX, pos, countY, maxY, newBaseTiles);

            if (!api.useCurrentZoomTiles)
            {
                List<OnlineMapsTile> newParentTiles = CreateParents(newBaseTiles, zoom - 1);
                if (zoom - 2 > 2)
                {
                    newParentTiles = CreateParents(newParentTiles, zoom - 2);
                    if (zoom - 3 > 2) CreateParents(newParentTiles, zoom - 3);
                }
            }

            if (api.target == OnlineMapsTarget.texture) SetMarkersToBuffer(api.markers);
        }

        needUnloadTiles = true;

        return true;
    }

    private void UpdateFrontBuffer(int apiWidth, int apiHeight)
    {
        int i = 0;

        for (int y = frontBufferPosition.y + apiHeight - 1; y >= frontBufferPosition.y; y--)
        {
            Array.Copy(backBuffer, frontBufferPosition.x + y * width, frontBuffer, i, apiWidth);
            i += apiWidth;
        }
    }

    private void UpdatePriorities(List<OnlineMapsTile> tiles)
    {
        foreach (OnlineMapsTile tile in tiles)
        {
            int zOff = bufferZoom - tile.zoom;
            if (zOff == 3) tile.priority = 0;
            else if (zOff == 2) tile.priority = 1;
            else if (zOff == 1) tile.priority = 2;
            else if (zOff == 0) tile.priority = 3;
            else tile.priority = 4;
        }
    }

    private void UpdateSmartBuffer(int apiWidth, int apiHeight)
    {
        int w = apiWidth;
        int hw = w / 2;
        int hh = apiHeight / 2;

        if (smartBuffer == null || smartBuffer.Length != hw * hh) smartBuffer = new Color[hw * hh];

        for (int y = 0; y < hh; y++)
        {
            int sy = (hh - y - 1) * hw;
            int fy = (y * 2 + frontBufferPosition.y) * width + frontBufferPosition.x;
            int fny = (y * 2 + frontBufferPosition.y + 1) * width + frontBufferPosition.x + 1;
            for (int x = 0, x2 = 0; x < hw; x++, x2 += 2)
            {
                Color clr1 = backBuffer[fy + x2];
                Color clr2 = backBuffer[fny + x2];

                clr1.r = (clr1.r + clr2.r) / 2;
                clr1.g = (clr1.g + clr2.g) / 2;
                clr1.b = (clr1.b + clr2.b) / 2;

                smartBuffer[sy + x] = clr1;
            }
        }
    }
}

[Serializable]
internal class OnlineMapsBufferZoom
{
    public readonly int id;
    public readonly List<OnlineMapsTile> tiles;

    public OnlineMapsBufferZoom(int zoom)
    {
        id = zoom;
        tiles = new List<OnlineMapsTile>();
    }
}