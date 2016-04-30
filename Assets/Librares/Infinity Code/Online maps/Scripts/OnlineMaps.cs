/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if !UNITY_WEBGL
using System.Threading;
#endif

/// <summary>
/// The main class. With it you can control the map.
/// </summary>
[AddComponentMenu("Infinity Code/Online Maps/Online Maps")]
[Serializable]
public class OnlineMaps : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The current version of Online Maps
    /// </summary>
    public const string version = "2.3.0.18";

    /// <summary>
    /// The maximum number simultaneously downloading tiles.
    /// </summary>
    public static int maxTileDownloads = 5;

    /// <summary>
    /// Allows you to customize the appearance of the tooltip.
    /// </summary>
    /// <param name="style">The reference to the style.</param>
    public delegate void OnPrepareTooltipStyleHandler(ref GUIStyle style);

    /// <summary>
    /// Event caused when the user change map position.
    /// </summary>
    public Action OnChangePosition;

    /// <summary>
    /// Event caused when the user change map zoom.
    /// </summary>
    public Action OnChangeZoom;

    /// <summary>
    /// Event caused after received and processed a request to search for a location.
    /// </summary>
    public Action OnFindLocationAfter;

    /// <summary>
    /// Event which is called after the redrawing of the map.
    /// </summary>
    public Action OnMapUpdated;

    /// <summary>
    /// Event caused when preparing tooltip style.
    /// </summary>
    public OnPrepareTooltipStyleHandler OnPrepareTooltipStyle;

    /// <summary>
    /// An event that occurs when loading the tile. Allows you to intercept of loading tile, and load it yourself.
    /// </summary>
    public Action<OnlineMapsTile> OnStartDownloadTile;

    /// <summary>
    /// Event is called before Update.
    /// </summary>
    public Action OnUpdateBefore;

    /// <summary>
    /// Event is called after Update.
    /// </summary>
    public Action OnUpdateLate;

    /// <summary>
    /// Specifies whether the user interacts with the map.
    /// </summary>
    public static bool isUserControl = false;

    private static OnlineMaps _instance;

    /// <summary>
    /// Allows drawing of map.\n
    /// <strong>
    /// Important: The interaction with the map, add or remove markers and drawing elements, automatically allowed to redraw the map.\n
    /// Use lockRedraw, to prohibit the redrawing of the map.
    /// </strong>
    /// </summary>
    public bool allowRedraw;

    /// <summary>
    /// Display control script.
    /// </summary>
    public OnlineMapsControlBase control;

    /// <summary>
    /// URL of custom provider.\n
    /// Support tokens:\n
    /// {x} - tile x\n
    /// {y} - tile y\n
    /// {zoom} - zoom level\n
    /// {quad} - uniquely identifies a single tile at a particular level of detail.
    /// </summary>
    public string customProviderURL = "http://localhost/{zoom}/{y}/{x}";

    /// <summary>
    /// Alignment marker default.
    /// </summary>
    public OnlineMapsAlign defaultMarkerAlign = OnlineMapsAlign.Bottom;

    /// <summary>
    /// Texture used by default for the marker.
    /// </summary>
    public Texture2D defaultMarkerTexture;

    /// <summary>
    /// Texture displayed until the tile is not loaded.
    /// </summary>
    public Texture2D defaultTileTexture;

    /// <summary>
    /// Specifies whether to dispatch the event.
    /// </summary>
    public bool dispatchEvents = true;

    /// <summary>
    /// The drawing elements.
    /// </summary>
    public List<OnlineMapsDrawingElement> drawingElements;

    /// <summary>
    /// Color, which is used until the tile is not loaded, unless specified field defaultTileTexture.
    /// </summary>
    public Color emptyColor = Color.gray;

    /// <summary>
    /// Map height in pixels.
    /// </summary>
    public int height;

    /// <summary>
    /// Specifies whether to display the labels on the map.
    /// </summary>
    public bool labels = true;

    /// <summary>
    /// Language of the labels on the map.
    /// </summary>
    public string language = "en";

    /// <summary>
    /// Prohibits drawing of maps.\n
    /// <strong>
    /// Important: Do not forget to disable this restriction. \n
    /// Otherwise, the map will never be redrawn.
    /// </strong>
    /// </summary>
    public bool lockRedraw = false;

    /// <summary>
    /// List of all 2D markers. <br/>
    /// Use AddMarker, RemoveMarker and RemoveAllMarkers.
    /// </summary>
    public OnlineMapsMarker[] markers;

    /// <summary>
    /// Specifies that need to collect the garbage.
    /// </summary>
    public bool needGC;

    /// <summary>
    /// A flag that indicates that need to redraw the map.
    /// </summary>
    public bool needRedraw;

    /// <summary>
    /// Not interact under the GUI.
    /// </summary>
    public bool notInteractUnderGUI = true;

    /// <summary>
    /// Limits the range of map coordinates.
    /// </summary>
    public OnlineMapsPositionRange positionRange;

    /// <summary>
    /// Map provider.
    /// </summary>
    public OnlineMapsProviderEnum provider = OnlineMapsProviderEnum.nokia;

    /// <summary>
    /// A flag that indicates whether to redraw the map at startup.
    /// </summary>
    public bool redrawOnPlay;

    /// <summary>
    /// Render map in a separate thread. Recommended.
    /// </summary>
    public bool renderInThread = true;

    /// <summary>
    /// Template path in Resources, from where the tiles will be loaded.\n
    /// This field supports tokens.
    /// </summary>
    public string resourcesPath = "OnlineMapsTiles/{zoom}/{x}/{y}";

    /// <summary>
    /// Indicates when the marker will show tips.
    /// </summary>
    public OnlineMapsShowMarkerTooltip showMarkerTooltip = OnlineMapsShowMarkerTooltip.onHover;

    /// <summary>
    /// Skin for the tooltip.
    /// </summary>
    public GUISkin skin;

    /// <summary>
    /// Reduced texture that is displayed when the user move map.
    /// </summary>
    public Texture2D smartTexture;

    /// <summary>
    /// Specifies from where the tiles should be loaded (Online, Resources, Online and Resources).
    /// </summary>
    public OnlineMapsSource source = OnlineMapsSource.Online;

    /// <summary>
    /// Specifies where the map should be drawn (Texture or Tileset).
    /// </summary>
    public OnlineMapsTarget target = OnlineMapsTarget.texture;

    /// <summary>
    /// Texture, which is used to draw the map. <br/>
    /// <strong>To change this value, use OnlineMaps.SetTexture.</strong>
    /// </summary>
    public Texture2D texture;

    /// <summary>
    /// Width of tileset in pixels.
    /// </summary>
    public int tilesetWidth = 1024;

    /// <summary>
    /// Height of tileset in pixels.
    /// </summary>
    public int tilesetHeight = 1024;

    /// <summary>
    /// Tileset size in scene;
    /// </summary>
    public Vector2 tilesetSize = new Vector2(1024, 1024);

    /// <summary>
    /// Tooltip, which will be shown.
    /// </summary>
    public string tooltip = string.Empty;

    /// <summary>
    /// Drawing element for which displays tooltip.
    /// </summary>
    public OnlineMapsDrawingElement tooltipDrawingElement;

    /// <summary>
    /// Marker for which displays tooltip.
    /// </summary>
    public OnlineMapsMarkerBase tooltipMarker;

    /// <summary>
    /// Specifies whether to draw traffic.
    /// </summary>
    public bool traffic = false;

    /// <summary>
    /// Map type.
    /// </summary>
    public int type;

    /// <summary>
    /// Use only the current zoom level of the tiles.
    /// </summary>
    public bool useCurrentZoomTiles = false;

    /// <summary>
    /// Specifies is necessary to use software JPEG decoder.
    /// Use only if you have problems with hardware decoding of JPEG.
    /// </summary>
    public bool useSoftwareJPEGDecoder = false;

    /// <summary>
    /// Specifies whether when you move the map showing the reduction texture.
    /// </summary>
    public bool useSmartTexture = true;

    /// <summary>
    /// URL of the proxy server used for Webplayer platform.
    /// </summary>
    public string webplayerProxyURL = "http://service.infinity-code.com/redirect.php?";

    /// <summary>
    /// Map width in pixels.
    /// </summary>
    public int width;

    /// <summary>
    /// Specifies the valid range of map zoom.
    /// </summary>
    public OnlineMapsRange zoomRange;

    [SerializeField]
    private double latitude;

    [SerializeField]
    private double longitude;

    public Vector2 _position;

    public int _zoom;

    private OnlineMapsBuffer _buffer;
    private List<OnlineMapsGoogleAPIQuery> _googleQueries;
    private bool _labels;
    private string _language;
    private OnlineMapsProviderEnum _provider;
    private bool _traffic;
    private int _type;
    
    private Texture2D activeTexture;
    private Action<bool> checkConnectioCallback;
    private WWW checkConnectionWWW;
    private Color[] defaultColors;
    private OnlineMapsTile downloads;
    private long lastGC;
    private OnlineMapsRedrawType redrawType = OnlineMapsRedrawType.none;

#if !UNITY_WEBGL
    private Thread renderThread;
#endif

    private OnlineMapsMarker rolledMarker;

    [NonSerialized]
    private double bottomRightLatitude;
    
    [NonSerialized]
    private double bottomRightLongitude;

    [NonSerialized]
    private double topLeftLatitude;

    [NonSerialized]
    private double topLeftLongitude;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton instance of map.
    /// </summary>
    public static OnlineMaps instance
    {
        get { return _instance; }
    }

    /// <summary>
    /// Gets whether the selected provider labels.
    /// </summary>
    /// <value>
    /// true if available labels, false if not.
    /// </value>
    public bool availableLabels
    {
        get
        {
            if (provider == OnlineMapsProviderEnum.google && type == 0) return true;
            if (provider == OnlineMapsProviderEnum.nokia && type == 0) return true;
            if (provider == OnlineMapsProviderEnum.virtualEarth && type == 0) return true;
            return false;
        }
    }

    /// <summary>
    /// Gets a list of available map types on selected provider.
    /// </summary>
    /// <value>
    /// A list of available map types.
    /// </value>
    public string[] availableTypes
    {
        get
        {
            string[] types = {"Satellite", "Relief", "Terrain", "Map"};
            if (provider == OnlineMapsProviderEnum.aMap) return new[] { types[0], types[2] };
            if (provider == OnlineMapsProviderEnum.arcGis) return new[] {types[0], types[2]};
            if (provider == OnlineMapsProviderEnum.custom) return null;
            if (provider == OnlineMapsProviderEnum.google) return new[] {types[0], types[1], types[2]};
            if (provider == OnlineMapsProviderEnum.mapQuest) return new[] {types[0], types[2]};
            if (provider == OnlineMapsProviderEnum.nokia) return new[] {types[0], types[2], types[3]};
            if (provider == OnlineMapsProviderEnum.openStreetMap) return null;
            if (provider == OnlineMapsProviderEnum.sputnik) return null;
            if (provider == OnlineMapsProviderEnum.virtualEarth) return new[] {types[0], types[2]};
            return types;
        }
    }

    /// <summary>
    /// Gets whether the selected provider label language.
    /// </summary>
    /// <value>
    /// true if language available, false if not.
    /// </value>
    public bool availableLanguage
    {
        get
        {
            if (provider == OnlineMapsProviderEnum.aMap) return false;
            if (provider == OnlineMapsProviderEnum.arcGis) return false;
            if (provider == OnlineMapsProviderEnum.mapQuest) return false;
            if (provider == OnlineMapsProviderEnum.openStreetMap) return false;
            if (provider == OnlineMapsProviderEnum.sputnik) return false;
            return true;
        }
    }

    /// <summary>
    /// Gets the bottom right position.
    /// </summary>
    /// <value>
    /// The bottom right position.
    /// </value>
    public Vector2 bottomRightPosition
    {
        get
        {
            if (bottomRightLatitude == 0 && bottomRightLongitude == 0) UpdateBottonRightPosition();
            return new Vector2((float)bottomRightLongitude, (float)bottomRightLatitude);
        }
    }

    /// <summary>
    /// Reference to the current draw buffer.
    /// </summary>
    public OnlineMapsBuffer buffer
    {
        get
        {
            if (_buffer == null) _buffer = new OnlineMapsBuffer(this);
            return _buffer;
        }
    }

    /// <summary>
    /// The current state of the drawing buffer.
    /// </summary>
    public OnlineMapsBufferStatus bufferStatus
    {
        get { return buffer.status; }
    }

    /// <summary>
    /// Checks whether the current settings display label.
    /// </summary>
    public bool enabledLabels
    {
        get
        {
            if (provider == OnlineMapsProviderEnum.arcGis && type == 1) return true;
            if (provider == OnlineMapsProviderEnum.google) return true;
            if (provider == OnlineMapsProviderEnum.mapQuest && type == 1) return true;
            if (provider == OnlineMapsProviderEnum.nokia) return true;
            if (provider == OnlineMapsProviderEnum.virtualEarth) return true;
            if (provider == OnlineMapsProviderEnum.sputnik) return true;
            if (provider == OnlineMapsProviderEnum.aMap && type == 1) return true;
            return false;
        }
    }

    private List<OnlineMapsGoogleAPIQuery> googleQueries
    {
        get { return _googleQueries ?? (_googleQueries = new List<OnlineMapsGoogleAPIQuery>()); }
    }

    /// <summary>
    /// Coordinates of the center point of the map.
    /// </summary>
    public Vector2 position
    {
        get { return new Vector2((float)longitude, (float)latitude); }
        set
        {
            SetPosition(value.x, value.y);
        }
    }

    /// <summary>
    /// Gets the top left position.
    /// </summary>
    /// <value>
    /// The top left position.
    /// </value>
    public Vector2 topLeftPosition
    {
        get
        {
            if (topLeftLatitude == 0 && topLeftLongitude == 0) UpdateTopLeftPosition();

            return new Vector2((float)topLeftLongitude, (float)topLeftLatitude);
        }
    }

    /// <summary>
    /// Current zoom.
    /// </summary>
    public int zoom
    {
        get { return _zoom; }
        set
        {
            int z = Mathf.Clamp(value, 3, 20);
            if (zoomRange != null) z = zoomRange.CheckAndFix(z);
            z = CheckMapSize(z);
            if (_zoom == z) return;

            _zoom = z;
            UpdateBottonRightPosition();
            UpdateTopLeftPosition();
            allowRedraw = true;
            needRedraw = true;
            redrawType = OnlineMapsRedrawType.full;
            DispatchEvent(OnlineMapsEvents.changedZoom);
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds a drawing element.
    /// </summary>
    /// <param name="element">
    /// The element.
    /// </param>
    public void AddDrawingElement(OnlineMapsDrawingElement element)
    {
        drawingElements.Add(element);
        needRedraw = true;
    }

    /// <summary>
    /// Adds a new request to the Google API in the processing queue.
    /// </summary>
    /// <param name="query">Queue</param>
    public void AddGoogleAPIQuery(OnlineMapsGoogleAPIQuery query)
    {
        googleQueries.Add(query);
    }

    /// <summary>
    /// Adds a 2D marker on the map.
    /// </summary>
    /// <param name="marker">
    /// The marker you want to add.
    /// </param>
    /// <returns>
    /// Marker instance.
    /// </returns>
    public OnlineMapsMarker AddMarker(OnlineMapsMarker marker)
    {
        List<OnlineMapsMarker> ms = markers.ToList();
        marker.Init();
        ms.Add(marker);
        markers = ms.ToArray();
        needRedraw = allowRedraw = true;
        return marker;
    }

    /// <summary>
    /// Adds a new 2D marker on the map.
    /// </summary>
    /// <param name="markerPosition">X - Longituge. Y - Latitude.</param>
    /// <param name="label">The text that will be displayed when you hover a marker.</param>
    /// <returns>Marker instance.</returns>
    public OnlineMapsMarker AddMarker(Vector2 markerPosition, string label)
    {
        return AddMarker(markerPosition, null, label);
    }

    /// <summary>
    /// Adds a new 2D marker on the map.
    /// </summary>
    /// <param name="markerPosition">X - Longituge. Y - Latitude.</param>
    /// <param name="markerTexture">
    /// <strong>Optional</strong><br/>
    /// Marker texture. <br/>
    /// In import settings must be enabled "Read / Write enabled". <br/>
    /// Texture format: ARGB32. <br/>
    /// If not specified, the will be used default marker texture.</param>
    /// <param name="label">
    /// <strong>Optional</strong><br/>
    /// The text that will be displayed when you hover a marker.</param>
    /// <returns>Marker instance.</returns>
    public OnlineMapsMarker AddMarker(Vector2 markerPosition, Texture2D markerTexture = null, string label = "")
    {
        if (markerTexture == null) markerTexture = defaultMarkerTexture;

        List<OnlineMapsMarker> ms = markers.ToList();
        OnlineMapsMarker marker = new OnlineMapsMarker
        {
            position = markerPosition,
            texture = markerTexture,
            label = label,
            align = defaultMarkerAlign
        };
        marker.Init();
        ms.Add(marker);
        markers = ms.ToArray();
        needRedraw = allowRedraw = true;
        return marker;
    }

    /// <summary>
    /// Adds a 2D markers on the map.
    /// </summary>
    /// <param name="newMarkers">
    /// The markers.
    /// </param>
    public void AddMarkers(OnlineMapsMarker[] newMarkers)
    {
        List<OnlineMapsMarker> ms = markers.ToList();
        foreach (OnlineMapsMarker marker in newMarkers)
        {
            marker.Init();
            ms.Add(marker);
        }
        markers = ms.ToArray();
        needRedraw = allowRedraw = true;
    }

// ReSharper disable once UnusedMember.Global
    public void Awake()
    {
        _instance = this;

        if (target == OnlineMapsTarget.texture)
        {
            width = texture.width;
            height = texture.height;
        }
        else
        {
            width = tilesetWidth;
            height = tilesetHeight;
            texture = null;
        }

        control = GetComponent<OnlineMapsControlBase>();
        if (control == null) Debug.LogError("Can not find a Control.");
        else control.OnAwakeBefore();

        if (target == OnlineMapsTarget.texture)
        {
            if (texture != null) defaultColors = texture.GetPixels();

            if (defaultTileTexture == null)
            {
                OnlineMapsTile.defaultColors = new Color[OnlineMapsUtils.sqrTileSize];
                for (int i = 0; i < OnlineMapsUtils.sqrTileSize; i++) OnlineMapsTile.defaultColors[i] = emptyColor;
            }
            else OnlineMapsTile.defaultColors = defaultTileTexture.GetPixels();
        }

        foreach (OnlineMapsMarker marker in markers) marker.Init();

        if (target == OnlineMapsTarget.texture && useSmartTexture && smartTexture == null)
        {
            smartTexture = new Texture2D(texture.width / 2, texture.height / 2, TextureFormat.RGB24, false);
        }
    }

    private void CheckBaseProps()
    {
        if (provider != _provider || type != _type || _language != language || _labels != labels)
        {
            _labels = labels;
            _language = language;
            _provider = provider;
            _type = type;

#if !UNITY_WEBGL
            int maxCount = 50;

            while (buffer.status == OnlineMapsBufferStatus.working && maxCount > 0)
            {
                Thread.Sleep(1);
                maxCount--;
            }

#if !UNITY_WEBPLAYER
            if (renderThread != null)
            {
#if UNITY_IOS
                renderThread.Interrupt();
#else
                renderThread.Abort();
#endif
            }
#endif
            renderThread = null;
#endif

            buffer.Dispose();
            buffer.status = OnlineMapsBufferStatus.wait;

            GCCollect();
            
            Redraw();
        }
        if (traffic != _traffic)
        {
            _traffic = traffic;
            OnlineMapsTile[] tiles;
            lock (OnlineMapsTile.tiles)
            {
                tiles = OnlineMapsTile.tiles.ToArray();
            }
            if (traffic)
            {
                foreach (OnlineMapsTile tile in tiles)
                {
                    if (!string.IsNullOrEmpty(tile.trafficURL))
                    {
                        tile.trafficWWW = OnlineMapsUtils.GetWWW(tile.trafficURL);
                    }
                }
            }
            else
            {
                foreach (OnlineMapsTile tile in tiles)
                {
                    tile.trafficTexture = null;
                    tile.trafficWWW = null;
                }
            }
        }
    }

    private void CheckBufferComplete()
    {
        if (buffer.status != OnlineMapsBufferStatus.complete) return;
        if (allowRedraw)
        {
            if (target == OnlineMapsTarget.texture)
            {
                if (!useSmartTexture || !buffer.generateSmartBuffer)
                {
                    texture.SetPixels(buffer.frontBuffer);
                    texture.Apply(false);
                    if (control.activeTexture != texture) control.SetTexture(texture);
                }
                else
                {
                    smartTexture.SetPixels(buffer.smartBuffer);
                    smartTexture.Apply(false);
                    if (control.activeTexture != smartTexture) control.SetTexture(smartTexture);

                    if (!isUserControl) needRedraw = true;
                }
            }

            if (control is OnlineMapsControlBase3D) ((OnlineMapsControlBase3D) control).UpdateControl();

            if (OnMapUpdated != null) OnMapUpdated();
        }
        buffer.status = OnlineMapsBufferStatus.wait;
    }

    private void CheckDownloadComplete()
    {
        if (checkConnectionWWW != null)
        {
            if (checkConnectionWWW.isDone)
            {
                checkConnectioCallback(string.IsNullOrEmpty(checkConnectionWWW.error));
                checkConnectionWWW = null;
                checkConnectioCallback = null;
            }
        }

        if (OnlineMapsTile.tiles.Count == 0) return;

        long startTicks = DateTime.Now.Ticks;

        OnlineMapsTile[] tiles;

        lock (OnlineMapsTile.tiles)
        {
            tiles = OnlineMapsTile.tiles.ToArray();
        }
        foreach (OnlineMapsTile tile in tiles)
        {
            if (DateTime.Now.Ticks - startTicks > 20000) break;

            if (tile.status == OnlineMapsTileStatus.loading && tile.www == null) tile.status = OnlineMapsTileStatus.none;

            if (tile.status == OnlineMapsTileStatus.loading && tile.www != null && tile.www.isDone)
            {
                if (string.IsNullOrEmpty(tile.www.error))
                {
                    if (target == OnlineMapsTarget.texture)
                    {
                        tile.OnDownloadComplete();
                        if (tile.status != OnlineMapsTileStatus.error) buffer.ApplyTile(tile);
                    }
                    else
                    {
                        Texture2D tileTexture = new Texture2D(256, 256, TextureFormat.RGB24, false)
                        {
                            wrapMode = TextureWrapMode.Clamp
                        };

                        if (useSoftwareJPEGDecoder) OnlineMapsTile.LoadTexture(tileTexture, tile.www.bytes);
                        else tile.www.LoadImageIntoTexture(tileTexture);

                        tile.CheckTextureSize(tileTexture);

                        if (tile.status != OnlineMapsTileStatus.error)
                        {
                            tile.texture = tileTexture;
                            tile.status = OnlineMapsTileStatus.loaded;
                        }
                    }

                    if (tile.status != OnlineMapsTileStatus.error)
                    {
                        if (OnlineMapsTile.OnTileDownloaded != null) OnlineMapsTile.OnTileDownloaded(tile);
                    }

                    CheckRedrawType();
                }
                else tile.OnDownloadError();

                if (tile.www != null)
                {
                    tile.www.Dispose();
                    tile.www = null;
                }
            }

            if (tile.status == OnlineMapsTileStatus.loaded && tile.trafficWWW != null && tile.trafficWWW.isDone)
            {
                if (string.IsNullOrEmpty(tile.trafficWWW.error))
                {
                    if (target == OnlineMapsTarget.texture)
                    {
                        if (tile.OnLabelDownloadComplete()) buffer.ApplyTile(tile);
                    }
                    else
                    {
                        Texture2D trafficTexture = new Texture2D(256, 256, TextureFormat.RGB24, false)
                        {
                            wrapMode = TextureWrapMode.Clamp
                        };
                        if (useSoftwareJPEGDecoder) OnlineMapsTile.LoadTexture(trafficTexture, tile.trafficWWW.bytes);
                        else tile.trafficWWW.LoadImageIntoTexture(trafficTexture);
                        tile.trafficTexture = trafficTexture;
                    }

                    if (OnlineMapsTile.OnTrafficDownloaded != null) OnlineMapsTile.OnTrafficDownloaded(tile);

                    CheckRedrawType();
                }

                if (tile.trafficWWW != null)
                {
                    tile.trafficWWW.Dispose();
                    tile.trafficWWW = null;
                }
            }
        }

        StartDownloading();
    }

    private void CheckGoogleAPIQuery()
    {
        if (googleQueries != null)
        {
            bool reqDelete = false;
            List<OnlineMapsGoogleAPIQuery> queries = googleQueries.Select(q => q).ToList();
            foreach (OnlineMapsGoogleAPIQuery item in queries)
            {
                item.CheckComplete();
                if (item.status != OnlineMapsQueryStatus.downloading)
                {
                    if (item.type == OnlineMapsQueryType.location && OnFindLocationAfter != null) OnFindLocationAfter();
                    item.Destroy();
                    reqDelete = true;
                }
            }
            if (reqDelete)
            {
                googleQueries.RemoveAll(f => f.status == OnlineMapsQueryStatus.disposed);
            }
        }
    }

    private int CheckMapSize(int z)
    {
        try
        {
            int maxX = (2 << z) / 2 * OnlineMapsUtils.tileSize;
            int maxY = (2 << z) / 2 * OnlineMapsUtils.tileSize;
            int w = (target == OnlineMapsTarget.texture) ? texture.width : tilesetWidth;
            int h = (target == OnlineMapsTarget.texture) ? texture.height : tilesetHeight;
            if (maxX <= w || maxY <= h) return CheckMapSize(z + 1);
        }
        catch{}
        
        return z;
    }

    /// <summary>
    /// Sets the desired type of redrawing the map.
    /// </summary>
    public void CheckRedrawType()
    {
        if (allowRedraw)
        {
            redrawType = OnlineMapsRedrawType.full;
            needRedraw = true;
        }
    }

    /// <summary>
    /// Allows you to test the connection to the Internet.
    /// </summary>
    /// <param name="callback">Function, which will return the availability of the Internet.</param>
    public void CheckServerConnection(Action<bool> callback)
    {
        OnlineMapsTile tempTile = new OnlineMapsTile(350, 819, 11, this, false);
        string url = tempTile.url;
        tempTile.Dispose();

        checkConnectioCallback = callback;
        checkConnectionWWW = OnlineMapsUtils.GetWWW(url);
    }

    /// <summary>
    /// Dispatch map events.
    /// </summary>
    /// <param name="evs">Events you want to dispatch.</param>
    public void DispatchEvent(params OnlineMapsEvents[] evs)
    {
        if (!dispatchEvents) return;

        foreach (OnlineMapsEvents ev in evs)
        {
            if (ev == OnlineMapsEvents.changedPosition && OnChangePosition != null) OnChangePosition();
            else if (ev == OnlineMapsEvents.changedZoom && OnChangeZoom != null) OnChangeZoom();
        }
    }

    /// <summary>Find route by coordinates or title.</summary>
    /// <param name="origin">Coordinates or the name of the route begins.</param>
    /// <param name="destination">Coordinates or the name of the route ends.</param>
    /// <param name="alternatives">Search for alternative routes.</param>
    /// <returns>Query instance to the Google API.</returns>
    [Obsolete("OnlineMaps.FindDirection is obsolete and will be removed. Use OnlineMapsFindDirection.Find.")]
    public OnlineMapsGoogleAPIQuery FindDirection(string origin, string destination, bool alternatives = false)
    {
        OnlineMapsGoogleAPIQuery fl = OnlineMapsFindDirection.Find(origin, destination, alternatives);
        googleQueries.Add(fl);
        return fl;
    }

    /// <summary>
    /// Search for location coordinates by the specified string and change the current position in the first search results.
    /// </summary>
    /// <param name="search">Address you want to find.</param>
    /// <returns>Instance of the search query.</returns>
    [Obsolete("OnlineMaps.FindLocation is obsolete and will be removed. Use OnlineMapsFindLocation.Find.")]
    public OnlineMapsGoogleAPIQuery FindLocation(string search)
    {
        OnlineMapsFindLocation fl = new OnlineMapsFindLocation(search);
        fl.OnComplete += OnlineMapsFindLocation.MovePositionToResult;
        googleQueries.Add(fl);
        return fl;
    }

    /// <summary>
    /// Search for location coordinates by the specified string and return search result to callback function.
    /// </summary>
    /// <param name="search">Address you want to find.</param>
    /// <param name="callback">Function, which will be given search results as XML string.</param>
    /// <returns>Instance of the search query.</returns>
    [Obsolete("OnlineMaps.FindLocation is obsolete and will be removed. Use OnlineMapsFindLocation.Find.")]
    public OnlineMapsFindLocation FindLocation(string search, Action<string> callback)
    {
        OnlineMapsFindLocation fl = new OnlineMapsFindLocation(search);
        fl.OnComplete += callback;
        googleQueries.Add(fl);
        return fl;
    }

    /// <summary>
    /// Unloads unused assets and initializes the garbage collection.
    /// </summary>
    public void GCCollect()
    {
        try
        {
            lastGC = DateTime.Now.Ticks;
            needGC = false;
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }
        catch
        {
        }
        
    }

    /// <summary>
    /// Get the bottom-right corner of the map.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    public void GetBottomRightPosition(out double lng, out double lat)
    {
        if (bottomRightLatitude == 0 && bottomRightLongitude == 0) UpdateBottonRightPosition();
        lng = bottomRightLongitude;
        lat = bottomRightLatitude;
    }

    /// <summary>
    /// Gets drawing element from screen.
    /// </summary>
    /// <param name="screenPosition">Screen position.</param>
    /// <returns>Drawing element</returns>
    public OnlineMapsDrawingElement GetDrawingElement(Vector2 screenPosition)
    {
        return drawingElements.FirstOrDefault(el => el.HitTest(OnlineMapsControlBase.instance.GetCoords(screenPosition), zoom));
    }

    /// <summary>
    /// Gets 2D marker from screen.
    /// </summary>
    /// <param name="screenPosition">
    /// Screen position.
    /// </param>
    /// <returns>
    /// The 2D marker.
    /// </returns>
    public OnlineMapsMarker GetMarkerFromScreen(Vector2 screenPosition)
    {
        if (control is OnlineMapsTileSetControl)
        {
            return OnlineMapsTileSetControl.instance.GetMarkerFromScreen(screenPosition);
        }
        return markers.FirstOrDefault(marker => marker.HitTest(OnlineMapsControlBase.instance.GetCoords(screenPosition), zoom));
    }

    /// <summary>
    /// Get the map coordinate.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    public void GetPosition(out double lng, out double lat)
    {
        lat = latitude;
        lng = longitude;
    }

    /// <summary>
    /// Get the top-left corner of the map.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    public void GetTopLeftPosition(out double lng, out double lat)
    {
        if (topLeftLatitude == 0 && topLeftLongitude == 0) UpdateTopLeftPosition();
        lng = topLeftLongitude;
        lat = topLeftLatitude;
    }

    private void LateUpdate()
    {
        if (control == null || lockRedraw) return;
        CheckBufferComplete();
        StartBuffer();

        if (needGC || DateTime.Now.Ticks - lastGC > OnlineMapsUtils.second * 5) GCCollect();
    }

// ReSharper disable once UnusedMember.Local
    private void OnDestroy()
    {
        OnlineMapsThreadManager.Dispose();

        if (_buffer != null)
        {
            _buffer.Dispose();
            _buffer = null;
        }
        
        if (defaultColors != null && texture != null)
        {
            texture.SetPixels(defaultColors);
            texture.Apply();
        }

#if !UNITY_WEBGL
        if (renderThread != null)
        {
#if UNITY_IOS
            renderThread.Interrupt();
#else
            renderThread.Abort();
#endif
        }
        renderThread = null;
#endif

        drawingElements = null;
        markers = null;

        GCCollect();
    }

// ReSharper disable once UnusedMember.Local
    private void OnDisable ()
    {
        if (_instance == this) _instance = null;

#if !UNITY_WEBGL
        if (renderThread != null)
        {
#if UNITY_IOS
            renderThread.Interrupt();
#else
            renderThread.Abort();
#endif
        }
        renderThread = null;
#endif
    }

    private void OnEnable()
    {
        _instance = this;

        if (language == "") language = provider != OnlineMapsProviderEnum.nokia ? "en" : "eng";
        if (drawingElements == null) drawingElements = new List<OnlineMapsDrawingElement>();
        _provider = provider;
        _type = type;
        _language = language;

        UpdateTopLeftPosition();
        UpdateBottonRightPosition();
    }

// ReSharper disable once UnusedMember.Local
    private void OnGUI()
    {
        if (string.IsNullOrEmpty(tooltip) && showMarkerTooltip != OnlineMapsShowMarkerTooltip.always) return;

        if (skin != null) GUI.skin = skin;
        GUIStyle style = new GUIStyle(GUI.skin.label);
			
        if (OnPrepareTooltipStyle != null) OnPrepareTooltipStyle(ref style);

        if (!string.IsNullOrEmpty(tooltip))
        {
            if (tooltipMarker != null)
            {
                if (tooltipMarker.OnDrawTooltip != null) tooltipMarker.OnDrawTooltip(tooltipMarker);
                else if (OnlineMapsMarkerBase.OnMarkerDrawTooltip != null) OnlineMapsMarkerBase.OnMarkerDrawTooltip(tooltipMarker);
                else OnGUITooltip(style, tooltip, Input.mousePosition);
            }
            else if (tooltipDrawingElement != null)
            {
                if (tooltipDrawingElement.OnDrawTooltip != null) tooltipDrawingElement.OnDrawTooltip(tooltipDrawingElement);
                else if (OnlineMapsDrawingElement.OnElementDrawTooltip != null) OnlineMapsDrawingElement.OnElementDrawTooltip(tooltipDrawingElement);
                else OnGUITooltip(style, tooltip, Input.mousePosition);
            }
        }

        if (showMarkerTooltip == OnlineMapsShowMarkerTooltip.always)
        {
            if (OnlineMapsControlBase.instance is OnlineMapsTileSetControl)
            {
                double tlx = topLeftLongitude;
                double tly = topLeftLatitude;
                double brx = bottomRightLongitude;
                double bry = bottomRightLatitude;
                if (brx < tlx) brx += 360;

                foreach (OnlineMapsMarker marker in markers)
                {
                    if (string.IsNullOrEmpty(marker.label)) continue;

                    float mx = marker.position.x;

                    if (!(((mx > tlx && mx < brx) || (mx + 360 > tlx && mx + 360 < brx) ||
                       (mx - 360 > tlx && mx - 360 < brx)) &&
                      marker.position.y < tly && marker.position.y > bry)) continue;

                    if (marker.OnDrawTooltip != null) marker.OnDrawTooltip(marker);
                    else if (OnlineMapsMarkerBase.OnMarkerDrawTooltip != null) OnlineMapsMarkerBase.OnMarkerDrawTooltip(marker);
                    else
                    {
                        Vector3 p1 = OnlineMapsTileSetControl.instance.GetWorldPositionWithElevation(marker.position, tlx, tly, brx, bry);
                        Vector3 p2 = p1 + new Vector3(0, 0, tilesetSize.y / tilesetHeight * marker.height * marker.scale);

                        Vector2 screenPoint1 = OnlineMapsTileSetControl.instance.activeCamera.WorldToScreenPoint(p1);
                        Vector2 screenPoint2 = OnlineMapsTileSetControl.instance.activeCamera.WorldToScreenPoint(p2);

                        float yOffset = (screenPoint1.y - screenPoint2.y) * transform.localScale.x - 10;

                        OnGUITooltip(style, marker.label, screenPoint1 + new Vector2(0, yOffset));
                    }
                }

                foreach (OnlineMapsMarker3D marker in OnlineMapsTileSetControl.instance.markers3D)
                {
                    if (string.IsNullOrEmpty(marker.label)) continue;

                    float mx = marker.position.x;

                    if (!(((mx > tlx && mx < brx) || (mx + 360 > tlx && mx + 360 < brx) ||
                       (mx - 360 > tlx && mx - 360 < brx)) &&
                      marker.position.y < tly && marker.position.y > bry)) continue;

                    if (marker.OnDrawTooltip != null) marker.OnDrawTooltip(marker);
                    else if (OnlineMapsMarkerBase.OnMarkerDrawTooltip != null) OnlineMapsMarkerBase.OnMarkerDrawTooltip(marker);
                    else
                    {
                        Vector3 p1 = OnlineMapsTileSetControl.instance.GetWorldPositionWithElevation(marker.position, tlx, tly, brx, bry);
                        Vector3 p2 = p1 + new Vector3(0, 0, tilesetSize.y / tilesetHeight * marker.scale);

                        Vector2 screenPoint1 = OnlineMapsTileSetControl.instance.activeCamera.WorldToScreenPoint(p1);
                        Vector2 screenPoint2 = OnlineMapsTileSetControl.instance.activeCamera.WorldToScreenPoint(p2);

                        float yOffset = (screenPoint1.y - screenPoint2.y) * transform.localScale.x - 10;

                        OnGUITooltip(style, marker.label, screenPoint1 + new Vector2(0, yOffset));
                    }
                }
            }
            else
            {
                foreach (OnlineMapsMarker marker in markers)
                {
                    if (string.IsNullOrEmpty(marker.label)) continue;

                    Rect rect = marker.screenRect;

                    if (rect.xMax > 0 && rect.xMin < Screen.width && rect.yMax > 0 && rect.yMin < Screen.height)
                    {
                        if (marker.OnDrawTooltip != null) marker.OnDrawTooltip(marker);
                        else if (OnlineMapsMarkerBase.OnMarkerDrawTooltip != null) OnlineMapsMarkerBase.OnMarkerDrawTooltip(marker);
                        else OnGUITooltip(style, marker.label, new Vector2(rect.x + rect.width / 2, rect.y + rect.height));
                    }
                }
            }
        }
    }

    private void OnGUITooltip(GUIStyle style, string text, Vector2 position)
    {
        GUIContent tip = new GUIContent(text);
        Vector2 size = style.CalcSize(tip);
        GUI.Label(new Rect(position.x - size.x / 2, Screen.height - position.y - size.y - 20, size.x + 10, size.y + 5), text, style);
    }

    /// <summary>
    /// Full redraw map.
    /// </summary>
    public void Redraw()
    {
        needRedraw = true;
        allowRedraw = true;
        redrawType = OnlineMapsRedrawType.full;
        buffer.updateBackBuffer = true;
    }

    /// <summary>
    /// Stops the current process map generation, clears all buffers and completely redraws the map.
    /// </summary>
    public void RedrawImmediately()
    {
#if !UNITY_WEBGL
        if (renderThread != null)
        {
#if UNITY_IOS
            renderThread.Interrupt();
#else
            renderThread.Abort();
#endif
        }
        renderThread = null;
#endif

        OnlineMapsThreadManager.Dispose();

        if (_buffer != null)
        {
            _buffer.Dispose();
            _buffer = null;
        }

        GCCollect();

        Redraw();
    }

    /// <summary>
    /// Remove the all drawing elements from the map.
    /// </summary>
    public void RemoveAllDrawingElements()
    {
        foreach (OnlineMapsDrawingElement element in drawingElements) element.OnRemoveFromMap();
        drawingElements.Clear();
        needRedraw = true;
    }

    /// <summary>
    /// Remove all 2D markers from map.
    /// </summary>
    public void RemoveAllMarkers()
    {
        markers = new OnlineMapsMarker[0];
        Redraw();
    }

    /// <summary>
    /// Remove the specified drawing element from the map.
    /// </summary>
    /// <param name="element">Drawing element you want to remove.</param>
    public void RemoveDrawingElement(OnlineMapsDrawingElement element)
    {
        element.OnRemoveFromMap();
        drawingElements.Remove(element);
        needRedraw = true;
    }

    /// <summary>
    /// Remove drawing element from the map by index.
    /// </summary>
    /// <param name="elementIndex">Drawing element index.</param>
    public void RemoveDrawingElementAt(int elementIndex)
    {
        if (elementIndex < 0 || elementIndex >= markers.Length) return;

        OnlineMapsDrawingElement element = drawingElements[elementIndex];

        element.OnRemoveFromMap();
        drawingElements.Remove(element);
        needRedraw = true;
    }

    /// <summary>
    /// Remove the specified 2D marker from the map.
    /// </summary>
    /// <param name="marker">2D marker you want to remove.</param>
    public void RemoveMarker(OnlineMapsMarker marker)
    {
        List<OnlineMapsMarker> ms = markers.ToList();
        ms.Remove(marker);
        markers = ms.ToArray();
        Redraw();
    }

    /// <summary>
    /// Remove 2D marker from the map by marker index.
    /// </summary>
    /// <param name="markerIndex">Marker index.</param>
    public void RemoveMarkerAt(int markerIndex)
    {
        if (markerIndex < 0 || markerIndex >= markers.Length) return;

        OnlineMapsMarker marker = markers[markerIndex];

        List<OnlineMapsMarker> ms = markers.ToList();
        ms.Remove(marker);
        markers = ms.ToArray();
        Redraw();
    }

    /// <summary>
    /// This method is for the editor. \n
    /// Please do not use it.
    /// </summary>
    public void Save()
    {
        if (target == OnlineMapsTarget.texture) defaultColors = texture.GetPixels();
        else Debug.LogWarning("OnlineMaps.Save() only works with texture maps.  Current map is: " + target);
    }

    /// <summary>
    /// This method is for the editor. \n
    /// Please do not use it.
    /// </summary>
    /// <param name="parent">Parent XML Element</param>
    public void SaveMarkers(OnlineMapsXML parent)
    {
        if (markers == null || markers.Length == 0) return;

        OnlineMapsXML element = parent.Create("Markers");
        foreach (OnlineMapsMarker marker in markers) marker.Save(element);
    }

    /// <summary>
    /// This method is for the editor. \n
    /// Please do not use it.
    /// </summary>
    /// <param name="parent">Parent XML Element</param>
    /// <returns></returns>
    public OnlineMapsXML SaveSettings(OnlineMapsXML parent)
    {
        OnlineMapsXML element = parent.Create("Settings");

        element.Create("Position", position);
        element.Create("Zoom", zoom);

        if (target == OnlineMapsTarget.texture) element.Create("Texture", texture);
        else
        {
            element.Create("TilesetWidth", tilesetWidth);
            element.Create("TilesetHeight", tilesetHeight);
            element.Create("TilesetSize", tilesetSize);
        }

        element.Create("Source", (int)source);
        element.Create("Provider", (int)provider);
        if (provider == OnlineMapsProviderEnum.custom) element.Create("CustomProviderURL", customProviderURL);
        element.Create("Type", type);
        element.Create("Labels", labels);
        element.Create("Traffic", traffic);
        element.Create("RedrawOnPlay", redrawOnPlay);
        element.Create("UseSmartTexture", useSmartTexture);
        element.Create("EmptyColor", emptyColor);
        element.Create("DefaultTileTexture", defaultTileTexture);
        element.Create("Skin", skin);
        element.Create("DefaultMarkerTexture", defaultMarkerTexture);
        element.Create("DefaultMarkerAlign", (int)defaultMarkerAlign);
        element.Create("ShowMarkerTooltip", (int)showMarkerTooltip);
        element.Create("UseSoftwareJPEGDecoder", useSoftwareJPEGDecoder);

        return element;
    }

    /// <summary>
    /// Set the the map coordinate.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    public void SetPosition(double lng, double lat)
    {
        int countX = width / OnlineMapsUtils.tileSize;
        int countY = height / OnlineMapsUtils.tileSize;

        if (positionRange != null)
        {
            if (positionRange.type == OnlineMapsPositionRangeType.center)
            {
                positionRange.CheckAndFix(ref lng, ref lat);
            }
            else if (positionRange.type == OnlineMapsPositionRangeType.border)
            {
                double px, py;
                OnlineMapsUtils.LatLongToTiled(lng, lat, _zoom, out px, out py);
                Vector2 offset = new Vector2(countX / 2f, countY / 2f);

                double tlx, tly, brx, bry;

                OnlineMapsUtils.TileToLatLong(px - offset.x, py - offset.y, _zoom, out tlx, out tly);
                OnlineMapsUtils.TileToLatLong(px + offset.x, py + offset.y, _zoom, out brx, out bry);

                double ltlx = tlx;
                double lbrx = brx;

                bool tlc = positionRange.CheckAndFix(ref tly, ref tlx);
                bool brc = positionRange.CheckAndFix(ref bry, ref brx);

                if (tlc && brc)
                {
                    if (ltlx == tlx || lbrx == brx)
                    {
                        double tx, ty;
                        OnlineMapsUtils.LatLongToTiled(tlx, tly, _zoom, out tx, out ty);
                        OnlineMapsUtils.TileToLatLong(tx + offset.x, ty + offset.y, _zoom, out lng, out lat);
                    }
                    else
                    {
                        lng = positionRange.center.x;
                        lat = positionRange.center.y;
                    }
                }
                else if (tlc)
                {
                    double tx, ty;
                    OnlineMapsUtils.LatLongToTiled(tlx, tly, _zoom, out tx, out ty);
                    OnlineMapsUtils.TileToLatLong(tx + offset.x, ty + offset.y, _zoom, out lng, out lat);
                }
                else if (brc)
                {
                    double tx, ty;
                    OnlineMapsUtils.LatLongToTiled(brx, bry, _zoom, out tx, out ty);
                    OnlineMapsUtils.TileToLatLong(tx - offset.x, ty - offset.y, _zoom, out lng, out lat);
                }
            }
        }

        double tpx, tpy;
        OnlineMapsUtils.LatLongToTiled(lng, lat, _zoom, out tpx, out tpy);

        float haftCountY = countY / 2f;
        int maxY = (2 << zoom) / 2;
        bool modified = false;
        if (tpy - haftCountY < 0)
        {
            tpy = haftCountY;
            modified = true;
        }
        else if (tpy + haftCountY >= maxY - 1)
        {
            tpy = maxY - haftCountY - 1;
            modified = true;
        }

        if (modified) OnlineMapsUtils.TileToLatLong(tpx, tpy, _zoom, out lng, out lat);

        if (latitude == lat && longitude == lng) return;

        allowRedraw = true;
        needRedraw = true;
        if (redrawType == OnlineMapsRedrawType.none || redrawType == OnlineMapsRedrawType.move)
            redrawType = OnlineMapsRedrawType.move;
        else redrawType = OnlineMapsRedrawType.full;

        latitude = lat;
        longitude = lng;
        UpdateTopLeftPosition();
        UpdateBottonRightPosition();

        DispatchEvent(OnlineMapsEvents.changedPosition);
    }

    /// <summary>
    /// Sets the position and zoom.
    /// </summary>
    /// <param name="lng">Longitude</param>
    /// <param name="lat">Latitude</param>
    /// <param name="ZOOM">Zoom</param>
    public void SetPositionAndZoom(float lng, float lat, int ZOOM = 0)
    {
        SetPosition(lng, lat);
        if (ZOOM != 0) zoom = ZOOM;
    }

    /// <summary>
    /// Sets the texture, which will draw the map.
    /// Texture displaying on the source you need to change yourself.
    /// </summary>
    /// <param name="newTexture">Texture, where you want to draw the map.</param>
    public void SetTexture(Texture2D newTexture)
    {
        texture = newTexture;
        width = newTexture.width;
        height = newTexture.height;
        allowRedraw = true;
        needRedraw = true;
        redrawType = OnlineMapsRedrawType.full;
    }

    /// <summary>
    /// Checks if the marker in the specified screen coordinates, and shows him a tooltip.
    /// </summary>
    /// <param name="screenPosition">Screen coordinates</param>
    public void ShowMarkersTooltip(Vector2 screenPosition)
    {
        if (showMarkerTooltip != OnlineMapsShowMarkerTooltip.onPress)
        {
            tooltip = string.Empty;
            tooltipDrawingElement = null;
            tooltipMarker = null;
        }

        if (control is OnlineMapsControlBase3D && OnlineMapsControlBase3D.instance.marker2DMode == OnlineMapsMarker2DMode.billboard)
        {
            return;
        }

        OnlineMapsMarker marker = GetMarkerFromScreen(screenPosition);

        if (showMarkerTooltip == OnlineMapsShowMarkerTooltip.onHover)
        {
            if (marker != null)
            {
                tooltip = marker.label;
                tooltipMarker = marker;
            }
            else
            {
                OnlineMapsDrawingElement drawingElement = GetDrawingElement(screenPosition);
                if (drawingElement != null)
                {
                    tooltip = drawingElement.tooltip;
                    tooltipDrawingElement = drawingElement;
                }
            }
        }

        if (rolledMarker != marker)
        {
            if (rolledMarker != null && rolledMarker.OnRollOut != null) rolledMarker.OnRollOut(rolledMarker);
            rolledMarker = marker;
            if (rolledMarker != null && rolledMarker.OnRollOver != null) rolledMarker.OnRollOver(rolledMarker);
        }
    }

// ReSharper disable once UnusedMember.Local
    private void Start()
    {
        if (redrawOnPlay)
        {
            allowRedraw = true;
            redrawType = OnlineMapsRedrawType.full;
        }
        needRedraw = true;
        _zoom = CheckMapSize(_zoom);
    }

    private void StartDownloading()
    {
        long startTick = DateTime.Now.Ticks;

        int countDownload = 0;

        IEnumerable tiles;

        lock (OnlineMapsTile.tiles)
        {
            countDownload = OnlineMapsTile.tiles.Count(t => t.status == OnlineMapsTileStatus.loading);
            if (countDownload >= maxTileDownloads) return;

            tiles = OnlineMapsTile.tiles.Where(t => t.status == OnlineMapsTileStatus.none).OrderBy(t => t.zoom).ToArray();
        }
        foreach (OnlineMapsTile tile in tiles)
        {
            if (DateTime.Now.Ticks - startTick > 20000) break;

            countDownload++;
            if (countDownload > maxTileDownloads) break;

            if (OnStartDownloadTile != null) OnStartDownloadTile(tile);
            else StartDownloadTile(tile);
        }
    }

    /// <summary>
    /// Starts dowloading of specified tile.
    /// </summary>
    /// <param name="tile">Tile to be downloaded.</param>
    public void StartDownloadTile(OnlineMapsTile tile)
    {
        bool loadOnline = true;

        if (source != OnlineMapsSource.Online)
        {
            UnityEngine.Object tileTexture = Resources.Load(tile.resourcesPath);
            if (tileTexture != null)
            {
                if (target == OnlineMapsTarget.texture)
                {
                    tile.ApplyTexture(tileTexture as Texture2D);
                    buffer.ApplyTile(tile);
                }
                else
                {
                    tile.texture = tileTexture as Texture2D;
                    tile.status = OnlineMapsTileStatus.loaded;
                }
                CheckRedrawType();
                loadOnline = false;
            }
            else if (source == OnlineMapsSource.Resources)
            {
                tile.status = OnlineMapsTileStatus.error;
                return;
            }
        }

        if (loadOnline)
        {
            tile.www = OnlineMapsUtils.GetWWW(tile.url);
            tile.status = OnlineMapsTileStatus.loading;
        }

        if (traffic && !string.IsNullOrEmpty(tile.trafficURL))
        {
            tile.trafficWWW = OnlineMapsUtils.GetWWW(tile.trafficURL);
        }
    }

    private void StartBuffer()
    {
        if (!allowRedraw || !needRedraw) return;
        if (buffer.status != OnlineMapsBufferStatus.wait) return;

        if (latitude < -90) latitude = -90;
        else if (latitude > 90) latitude = 90;
        while (longitude < -180 || longitude > 180)
        {
            if (longitude < -180) longitude += 360;
            else if (longitude > 180) longitude -= 360;
        }
        
        buffer.redrawType = redrawType;
        buffer.generateSmartBuffer = isUserControl;
        buffer.status = OnlineMapsBufferStatus.start;
        
#if !UNITY_WEBGL
        if (renderInThread)
        {
            if (renderThread == null)
            {
                renderThread = new Thread(buffer.GenerateFrontBuffer);
                renderThread.Start();
            }
        }
        else buffer.GenerateFrontBuffer();
#else
        buffer.GenerateFrontBuffer();
#endif

        redrawType = OnlineMapsRedrawType.none;
        needRedraw = false;
    }

// ReSharper disable once UnusedMember.Local
    private void Update()
    {
        if (OnUpdateBefore != null) OnUpdateBefore();
        
        CheckBaseProps();
        CheckGoogleAPIQuery();
        CheckDownloadComplete();

        if (OnUpdateLate != null) OnUpdateLate();
    }

    private void UpdateBottonRightPosition()
    {
        int countX = width / OnlineMapsUtils.tileSize;
        int countY = height / OnlineMapsUtils.tileSize;

        double px, py;
        OnlineMapsUtils.LatLongToTiled(longitude, latitude, _zoom, out px, out py);

        px += countX / 2.0;
        py += countY / 2.0;

        OnlineMapsUtils.TileToLatLong(px, py, _zoom, out bottomRightLongitude, out bottomRightLatitude);
    }

    private void UpdateTopLeftPosition()
    {
        int countX = width / OnlineMapsUtils.tileSize;
        int countY = height / OnlineMapsUtils.tileSize;

        double px, py;

        OnlineMapsUtils.LatLongToTiled(longitude, latitude, _zoom, out px, out py);

        px -= countX / 2.0;
        py -= countY / 2.0;

        OnlineMapsUtils.TileToLatLong(px, py, _zoom, out topLeftLongitude, out topLeftLatitude);
    }

#endregion
}