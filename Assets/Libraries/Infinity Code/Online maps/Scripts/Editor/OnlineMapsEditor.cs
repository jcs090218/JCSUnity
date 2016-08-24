/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CustomEditor(typeof (OnlineMaps))]
// ReSharper disable once UnusedMember.Global
public class OnlineMapsEditor : Editor
{
    internal enum TextureType
    {
        Texture,
        Sprite
    }

    private static GUIStyle _warningStyle;

    private OnlineMaps api;
    public static readonly int[] availableSizes = {256, 512, 1024, 2048, 4096};
    private string[] availableSizesStr;
    private bool showAdvanced;
    private bool showCacheInfo;
    private bool showCreateTexture;
    private bool showCustomProviderTokens;
    private bool showMarkers;
    private bool showSave;
    private bool showResourcesTokens;
    private bool showTroubleshooting;
    private int textureHeight = 512;
    private int textureWidth = 512;
    private TextureType textureType;
    private GUIContent updateAvailableContent;
    private string textureFilename = "OnlineMaps";

    private bool saveSettings = true;
    private bool saveTexture = true;
    private bool saveControl = true;
    private bool saveMarkers = true;
    private bool saveMarkers3D = true;
    private bool saveLocationService = true;
    private bool allowSaveMarkers3D;
    private bool allowSaveLocationService;
    private bool allowSaveTexture;

    public static GUIStyle warningStyle
    {
        get
        {
            if (_warningStyle == null)
            {
                _warningStyle = new GUIStyle(GUI.skin.label);
                _warningStyle.normal.textColor = Color.red;
                _warningStyle.fontStyle = FontStyle.Bold;
            }
            
            return _warningStyle;
        }
    }

    private static bool isPlay
    {
        get { return Application.isPlaying; }
    }

    public static void AddCompilerDirective(string directive)
    {
        string currentDefinitions = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> directives = currentDefinitions.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();
        if (!directives.Contains(directive)) directives.Add(directive);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, String.Join(";", directives.ToArray()));
    }

    private void CheckAPITextureImporter(Texture2D texture)
    {
        if (texture == null) return;

        string textureFilename = AssetDatabase.GetAssetPath(texture.GetInstanceID());
        TextureImporter textureImporter = AssetImporter.GetAtPath(textureFilename) as TextureImporter;
        if (textureImporter == null) return;

        bool needReimport = false;
        if (textureImporter.mipmapEnabled)
        {
            textureImporter.mipmapEnabled = false;
            needReimport = true;
        }
        if (!textureImporter.isReadable)
        {
            textureImporter.isReadable = true;
            needReimport = true;
        }
        if (textureImporter.textureFormat != TextureImporterFormat.RGB24)
        {
            textureImporter.textureFormat = TextureImporterFormat.RGB24;
            needReimport = true;
        }
        if (textureImporter.maxTextureSize < 256)
        {
            textureImporter.maxTextureSize = 256;
            needReimport = true;
        }

        if (needReimport) AssetDatabase.ImportAsset(textureFilename, ImportAssetOptions.ForceUpdate);
    }

    public static void CheckMarkerTextureImporter(Texture2D texture)
    {
        if (texture == null) return;

        string textureFilename = AssetDatabase.GetAssetPath(texture.GetInstanceID());
        TextureImporter textureImporter = AssetImporter.GetAtPath(textureFilename) as TextureImporter;
        if (textureImporter == null) return;

        bool needReimport = false;
        if (textureImporter.mipmapEnabled)
        {
            textureImporter.mipmapEnabled = false;
            needReimport = true;
        }
        if (!textureImporter.isReadable)
        {
            textureImporter.isReadable = true;
            needReimport = true;
        }
        if (textureImporter.textureFormat != TextureImporterFormat.ARGB32)
        {
            textureImporter.textureFormat = TextureImporterFormat.ARGB32;
            needReimport = true;
        }

        if (needReimport) AssetDatabase.ImportAsset(textureFilename, ImportAssetOptions.ForceUpdate);
    }

    private void CreateTexture()
    {
        string texturePath = string.Format("Assets/{0}.png", textureFilename);
        api.texture = new Texture2D(textureWidth, textureHeight);
        File.WriteAllBytes(texturePath, api.texture.EncodeToPNG());
        AssetDatabase.Refresh();
        TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        if (textureImporter != null)
        {
            textureImporter.mipmapEnabled = false;
            textureImporter.isReadable = true;
            textureImporter.textureFormat = TextureImporterFormat.RGB24;
            textureImporter.maxTextureSize = Mathf.Max(textureWidth, textureHeight);

            if (textureType == TextureType.Sprite)
            {
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                textureImporter.npotScale = TextureImporterNPOTScale.None;
            }

            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
            api.texture = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
        }

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
        EditorUtility.UnloadUnusedAssets();
#else
        EditorUtility.UnloadUnusedAssetsImmediate();
#endif
    }

    private void DrawAdvancedGUI(ref bool dirty)
    {
        EditorGUI.BeginChangeCheck();

        if (api.target == OnlineMapsTarget.texture)
        {
            api.redrawOnPlay = EditorGUILayout.Toggle("Redraw on Play", api.redrawOnPlay);
            api.useSmartTexture = EditorGUILayout.Toggle("Smart Texture", api.useSmartTexture);
        }

        api.useCurrentZoomTiles = EditorGUILayout.Toggle("Use Current Zoom Tiles", api.useCurrentZoomTiles);

        DrawTrafficGUI(ref dirty);

        api.emptyColor = EditorGUILayout.ColorField("Empty Color: ", api.emptyColor);

        api.defaultTileTexture = (Texture2D)EditorGUILayout.ObjectField("Default Tile: ", api.defaultTileTexture, typeof(Texture2D), false);
        CheckAPITextureImporter(api.defaultTileTexture);

        api.skin = (GUISkin) EditorGUILayout.ObjectField("Skin: ", api.skin, typeof (GUISkin), false);

        api.defaultMarkerTexture = (Texture2D) EditorGUILayout.ObjectField("Default Marker: ", api.defaultMarkerTexture, typeof (Texture2D), false);
        CheckMarkerTextureImporter(api.defaultMarkerTexture);

        api.defaultMarkerAlign = (OnlineMapsAlign) EditorGUILayout.EnumPopup("Markers Align: ", api.defaultMarkerAlign);
        api.showMarkerTooltip = (OnlineMapsShowMarkerTooltip)EditorGUILayout.EnumPopup("Show Marker Tooltip: ", api.showMarkerTooltip);

        if (EditorGUI.EndChangeCheck()) dirty = true;
    }

    private void DrawCacheGUI(ref bool dirty)
    {
        if (api.source == OnlineMapsSource.Resources || !GUILayout.Button("Cache tiles to Resources")) return;

        List<string> files = new List<string>();

        lock (OnlineMapsTile.tiles)
        {
            const string resPath = "Assets/Resources";

            foreach (OnlineMapsTile tile in OnlineMapsTile.tiles)
            {
                if (tile.status != OnlineMapsTileStatus.loaded || tile.texture == null) continue;

                string tilePath = Path.Combine(resPath, tile.resourcesPath + ".png");
                FileInfo info = new FileInfo(tilePath);
                if (!info.Directory.Exists) info.Directory.Create();

                if (api.target == OnlineMapsTarget.tileset) File.WriteAllBytes(tilePath, tile.texture.EncodeToPNG());
                else
                {
                    Texture2D texture = new Texture2D(OnlineMapsUtils.tileSize, OnlineMapsUtils.tileSize,
                        TextureFormat.ARGB32, false);
                    texture.SetPixels(tile.colors);
                    texture.Apply();
                    File.WriteAllBytes(tilePath, texture.EncodeToPNG());
                }

                files.Add(tilePath);
            }
        }

        AssetDatabase.Refresh();

        for (int i = 0; i < files.Count; i++)
        {
            string f = files[i];
            FixTileImporter(f, i / (float) files.Count);
        }

        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("Cache complete", "Stop playback and select 'Source - Resources And Online'.", "OK");

        dirty = true;
    }

    private void DrawCreateTextureGUI(ref bool dirty)
    {
        if (availableSizesStr == null) availableSizesStr = availableSizes.Select(s => s.ToString()).ToArray();

        textureFilename = EditorGUILayout.TextField("Filename: ", textureFilename);
        textureType = (TextureType)EditorGUILayout.EnumPopup("Type: ", textureType);

        textureWidth = EditorGUILayout.IntPopup("Width: ", textureWidth,
            availableSizesStr, availableSizes);
        textureHeight = EditorGUILayout.IntPopup("Height: ", textureHeight,
            availableSizesStr, availableSizes);

        if (GUILayout.Button("Create"))
        {
            CreateTexture();
            dirty = true;
        }

        EditorGUILayout.Space();
    }

    private bool DrawGeneralGUI()
    {
        bool dirty = false;

        DrawSourceGUI(ref dirty);
        DrawLocationGUI(ref dirty);
        DrawTargetGUI(ref dirty);

        if (isPlay)
        {
            DrawCacheGUI(ref dirty);

            if (!showSave) 
            {
                if (GUILayout.Button("Save state"))
                {
                    allowSaveMarkers3D = api.GetComponent<OnlineMapsControlBase3D>() != null;
                    allowSaveLocationService = api.GetComponent<OnlineMapsLocationService>() != null;
                    allowSaveTexture = api.target == OnlineMapsTarget.texture;

                    showSave = true;
                    dirty = true;
                }
            }
            else
            {
                DrawSaveGUI(ref dirty);
            }
        }

        return dirty;
    }

    private void DrawLabelsGUI(ref bool dirty)
    {
        bool showLanguage;
        if (api.availableLabels)
        {
            bool labels = api.labels;
            api.labels = EditorGUILayout.Toggle("Labels: ", api.labels);
            if (labels != api.labels) dirty = true;
            showLanguage = api.labels;
        }
        else
        {
            showLanguage = api.enabledLabels;
            GUILayout.Label("Labels " + (showLanguage ? "enabled" : "disabled"));
        }
        if (showLanguage && api.availableLanguage)
        {
            api.language = EditorGUILayout.TextField("Language: ", api.language);
            GUILayout.Label(api.provider == OnlineMapsProviderEnum.nokia
                ? "Use three-letter code such as: eng"
                : "Use two-letter code such as: en");
        }
    }

    private void DrawLocationGUI(ref bool dirty)
    {
        Vector2 p = api.position;
        EditorGUI.BeginChangeCheck();
        p.y = EditorGUILayout.FloatField("Latitude: ", p.y);
        p.x = EditorGUILayout.FloatField("Longitude: ", p.x);
        if (EditorGUI.EndChangeCheck())
        {
            dirty = true;
            api.SetPosition(p.x, p.y);
        }
        EditorGUI.BeginChangeCheck();
        api.zoom = EditorGUILayout.IntSlider("Zoom: ", api.zoom, 3, 20);
        if (EditorGUI.EndChangeCheck()) dirty = true;
    }

    private void DrawMarkerGUI(int i, ref int index, ref bool hasDeleted)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        OnlineMapsMarker marker = api.markers[i];
        GUILayout.Label("Marker " + index);

        EditorGUI.BeginChangeCheck();
        marker.position.y = EditorGUILayout.FloatField("Latitude: ", marker.position.y);
        marker.position.x = EditorGUILayout.FloatField("Longitude: ", marker.position.x);

        float min = marker.range.min;
        float max = marker.range.max;
        EditorGUILayout.MinMaxSlider(new GUIContent(string.Format("Zooms ({0}-{1}): ", marker.range.min, marker.range.max) ), ref min, ref max, 3, 20);
        marker.range.min = Mathf.RoundToInt(min);
        marker.range.max = Mathf.RoundToInt(max);

        marker.range.min = Mathf.Clamp(marker.range.min, 3, 20);
        marker.range.max = Mathf.Clamp(marker.range.max, 3, 20);

        marker.rotation = Mathf.Repeat(EditorGUILayout.FloatField("Rotation (0-1): ", marker.rotation), 1);
        marker.label = EditorGUILayout.TextField("Label: ", marker.label);
        marker.align = (OnlineMapsAlign) EditorGUILayout.EnumPopup("Align: ", marker.align);
        marker.texture =
            (Texture2D) EditorGUILayout.ObjectField("Texture: ", marker.texture, typeof (Texture2D), true);

        if (EditorGUI.EndChangeCheck() && Application.isPlaying) api.Redraw();

        CheckMarkerTextureImporter(marker.texture);

        if (GUILayout.Button("Remove"))
        {
            api.markers[i] = null;
            hasDeleted = true;
        }
        index++;

        EditorGUILayout.EndVertical();
    }

    private void DrawMarkersGUI()
    {
        if (api.markers == null) api.markers = new OnlineMapsMarker[0];

        int index = 1;
        bool hasDeleted = false;

        for (int i = 0; i < api.markers.Length; i++) DrawMarkerGUI(i, ref index, ref hasDeleted);

        if (hasDeleted)
        {
            List<OnlineMapsMarker> markers = api.markers.ToList();
            markers.RemoveAll(m => m == null);
            api.markers = markers.ToArray();
            if (Application.isPlaying) api.Redraw();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Add marker"))
        {
            if (!Application.isPlaying)
            {
                OnlineMapsMarker marker = new OnlineMapsMarker { position = api.position, align = api.defaultMarkerAlign };
                List<OnlineMapsMarker> markers = new List<OnlineMapsMarker>(api.markers) { marker };
                api.markers = markers.ToArray();
            }
            else
            {
                OnlineMapsMarker marker = api.AddMarker(api.position);
                marker.align = api.defaultMarkerAlign;
            }
        }
    }

    private void DrawProviderGUI(ref bool dirty)
    {
        EditorGUI.BeginChangeCheck();
        api.provider = (OnlineMapsProviderEnum)EditorGUILayout.EnumPopup(new GUIContent("Provider: ", "Provider of tiles"), api.provider);
        if (EditorGUI.EndChangeCheck())
        {
            api.type = 0;
            dirty = true;
        }

        if (api.provider == OnlineMapsProviderEnum.sputnik)
        {
            EditorGUILayout.HelpBox("Sputnik uses http, which can cause problems in iOS9+.", MessageType.Warning);
        }
        else if (api.provider == OnlineMapsProviderEnum.custom)
        {
            string customProviderURL = api.customProviderURL;
            api.customProviderURL = EditorGUILayout.TextField("URL: ", api.customProviderURL);
            if (customProviderURL != api.customProviderURL) dirty = true;

            EditorGUILayout.BeginVertical(GUI.skin.box);
            showCustomProviderTokens = Foldout(showCustomProviderTokens, "Available tokens");
            if (showCustomProviderTokens)
            {
                GUILayout.Label("{zoom}");
                GUILayout.Label("{x}");
                GUILayout.Label("{y}");
                GUILayout.Label("{quad}");
                GUILayout.Space(10);
            }
            EditorGUILayout.EndVertical();
        }

#if UNITY_WEBPLAYER
        //if (provider != OnlineMapsProviderEnum.custom) GUILayout.Label("This provider can be slow on Webplayer");
#endif
    }

    private void DrawSaveGUI(ref bool dirty)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.LabelField("Save state:");

        saveSettings = EditorGUILayout.Toggle("Settings", saveSettings);
        
        if (allowSaveTexture) saveTexture = EditorGUILayout.Toggle("Texture", saveTexture);

        saveControl = EditorGUILayout.Toggle("Control", saveControl);
        saveMarkers = EditorGUILayout.Toggle("Markers", saveMarkers);

        if (allowSaveMarkers3D) saveMarkers3D = EditorGUILayout.Toggle("Markers 3D", saveMarkers3D);
        if (allowSaveLocationService) saveLocationService = EditorGUILayout.Toggle("Location Service", saveLocationService);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save state"))
        {
            if (allowSaveTexture && saveTexture)
            {
                api.Save();

                string path = AssetDatabase.GetAssetPath(api.texture);
                File.WriteAllBytes(path, api.texture.EncodeToPNG());
                AssetDatabase.Refresh();
            }

            OnlineMapsXML prefs = new OnlineMapsXML("OnlineMaps");

            if (saveSettings) api.SaveSettings(prefs);
            if (saveControl) api.GetComponent<OnlineMapsControlBase>().SaveSettings(prefs);
            if (saveMarkers) api.SaveMarkers(prefs);
            if (allowSaveMarkers3D && saveMarkers3D) api.GetComponent<OnlineMapsControlBase3D>().SaveMarkers3D(prefs);
            if (allowSaveLocationService && saveLocationService) api.GetComponent<OnlineMapsLocationService>().Save(prefs);

            OnlineMapsPrefs.Save(prefs.outerXml);

            ResetSaveSettings();
            dirty = true;
        }

        if (GUILayout.Button("Cancel", GUILayout.ExpandWidth(false)))
        {
            ResetSaveSettings();
            dirty = true;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
    }

    private void DrawSourceGUI(ref bool dirty)
    {
        EditorGUI.BeginDisabledGroup(isPlay);

        OnlineMapsSource source = (OnlineMapsSource) EditorGUILayout.EnumPopup("Source: ", api.source);
        if (source != api.source)
        {
            api.source = source;
            dirty = true;
        }

#if UNITY_WEBPLAYER
        if (source != OnlineMapsSource.Resources)
        {
            api.webplayerProxyURL = EditorGUILayout.TextField("Proxy: ", api.webplayerProxyURL);
        }
#endif

        if (source != OnlineMapsSource.Online)
        {
            if (GUILayout.Button("Fix import settings for tiles")) FixImportSettings();
            if (GUILayout.Button("Import from GMapCatcher")) ImportFromGMapCatcher();
            api.resourcesPath = EditorGUILayout.TextField("Resources Path:", api.resourcesPath);
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            showResourcesTokens = Foldout(showResourcesTokens, "Available tokens");
            if (showResourcesTokens)
            {
                GUILayout.Label("{zoom}");
                GUILayout.Label("{x}");
                GUILayout.Label("{y}");
                GUILayout.Label("{quad}");
                GUILayout.Space(10);
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUI.EndDisabledGroup();

        if (source != OnlineMapsSource.Resources)
        {
            DrawProviderGUI(ref dirty);

            string[] availableTypes = api.availableTypes;
            if (availableTypes != null)
            {
                GUIContent[] aviableTypes = availableTypes.Select(t => new GUIContent(t)).ToArray();
                if (aviableTypes != null)
                {
                    int type = api.type;
                    api.type = EditorGUILayout.Popup(new GUIContent("Type: ", "Type of map texture"), api.type, aviableTypes);
                    if (type != api.type) dirty = true;
                }
            }

            DrawLabelsGUI(ref dirty);
        }
    }

    private void DrawTargetGUI(ref bool dirty)
    {
        EditorGUI.BeginDisabledGroup(isPlay);

        OnlineMapsTarget mapTarget = api.target;
        api.target =
            (OnlineMapsTarget) EditorGUILayout.EnumPopup(new GUIContent("Target:", "Where will be drawn map"), api.target);
        if (mapTarget != api.target) dirty = true;

        if (api.target == OnlineMapsTarget.texture)
        {
            Texture2D texture = api.texture;
            api.texture = (Texture2D) EditorGUILayout.ObjectField("Texture: ", api.texture, typeof (Texture2D), true);
            if (texture != api.texture)
            {
                if (api.texture != null && (!Mathf.IsPowerOfTwo(api.texture.width) || !Mathf.IsPowerOfTwo(api.texture.height)))
                {
                    EditorUtility.DisplayDialog("Error", "Texture width and height must be power of two!!!", "OK");
                    api.texture = texture;
                }
                else
                {
                    CheckAPITextureImporter(api.texture);
                }
                dirty = true;
            }
        }
        else if (api.target == OnlineMapsTarget.tileset) DrawTilesetPropsGUI(ref dirty);

        EditorGUI.EndDisabledGroup();
    }

    private void DrawTilesetPropsGUI(ref bool dirty)
    {
        EditorGUI.BeginChangeCheck();
        api.tilesetWidth = EditorGUILayout.IntField("Width (pixels): ", api.tilesetWidth);
        api.tilesetHeight = EditorGUILayout.IntField("Height (pixels): ", api.tilesetHeight);
        api.tilesetSize = EditorGUILayout.Vector2Field("Size (in scene): ", api.tilesetSize);
        if (EditorGUI.EndChangeCheck()) dirty = true;

        int dts = OnlineMapsUtils.tileSize * 2;
        if (api.tilesetWidth % dts != 0) api.tilesetWidth = Mathf.FloorToInt(api.tilesetWidth / (float) dts + 0.5f) * dts;
        if (api.tilesetHeight % dts != 0)
            api.tilesetHeight = Mathf.FloorToInt(api.tilesetHeight / (float) dts + 0.5f) * dts;

        if (api.tilesetWidth <= 0) api.tilesetWidth = dts;
        if (api.tilesetHeight <= 0) api.tilesetHeight = dts;
    }

    private void DrawToolbarGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (OnlineMapsUpdater.hasNewVersion && updateAvailableContent != null)
        {
            Color defBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(1, 0.5f, 0.5f);
            if (GUILayout.Button(updateAvailableContent, EditorStyles.toolbarButton))
            {
                OnlineMapsUpdater.OpenWindow();
            }
            GUI.backgroundColor = defBackgroundColor;
        }
        else GUILayout.Label("");

        if (GUILayout.Button("Help", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Documentation"), false, OnViewDocs);
            menu.AddItem(new GUIContent("API Reference"), false, OnViewAPI);
            menu.AddItem(new GUIContent("Atlas of Examples"), false, OnViewAtlas);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Product Page"), false, OnProductPage);
            menu.AddItem(new GUIContent("Forum"), false, OnViewForum);
            menu.AddItem(new GUIContent("Check Updates"), false, OnCheckUpdates);
            menu.AddItem(new GUIContent("Support"), false, OnSendMail);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("About"), false, OnAbout);
            menu.ShowAsContext();
        }

        GUILayout.EndHorizontal();
    }

    private void DrawTrafficGUI(ref bool dirty)
    {
        bool traffic = api.traffic;
        api.traffic = EditorGUILayout.Toggle("Traffic: ", api.traffic);
        if (traffic != api.traffic) dirty = true;
    }

    private void DrawTroubleshootingGUI(ref bool dirty)
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.Label("Use this props only if you have a problem!!!", warningStyle);
        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();
        api.useSoftwareJPEGDecoder = EditorGUILayout.Toggle("Software JPEG Decoder", api.useSoftwareJPEGDecoder);

#if !UNITY_WEBGL
        api.renderInThread = EditorGUILayout.Toggle("Render in Thread", api.renderInThread);
#endif

        api.notInteractUnderGUI = EditorGUILayout.Toggle("Not interact under GUI", api.notInteractUnderGUI);

        if (EditorGUI.EndChangeCheck()) dirty = true;
    }

    private static void FixImportSettings()
    {
        string resourcesFolder = Path.Combine(Application.dataPath, "Resources/OnlineMapsTiles");
        string[] tiles = Directory.GetFiles(resourcesFolder, "*.png", SearchOption.AllDirectories);
        float count = tiles.Length;
        int index = 0;
        foreach (string tile in tiles)
        {
            string shortPath = "Assets/" + tile.Substring(Application.dataPath.Length + 1);
            FixTileImporter(shortPath, index / count);
            index++;
        }

        EditorUtility.ClearProgressBar();
    }

    private static void FixTileImporter(string shortPath, float progress)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(shortPath) as TextureImporter;
        EditorUtility.DisplayProgressBar("Update import settings for tiles",
            "Please wait, this may take several minutes.", progress);
        if (textureImporter != null)
        {
            textureImporter.mipmapEnabled = false;
            textureImporter.isReadable = true;
            textureImporter.textureFormat = TextureImporterFormat.RGB24;
            textureImporter.wrapMode = TextureWrapMode.Clamp;
            textureImporter.maxTextureSize = 256;
            AssetDatabase.ImportAsset(shortPath, ImportAssetOptions.ForceSynchronousImport);
        }
    }

    public static bool Foldout(bool value, string text)
    {
        return GUILayout.Toggle(value, text, EditorStyles.foldout);
    }

    public static Texture2D GetIcon(string iconName)
    {
        string[] path = Directory.GetFiles(Application.dataPath, iconName, SearchOption.AllDirectories);
        if (path.Length == 0) return null;
        string iconFile = "Assets" + path[0].Substring(Application.dataPath.Length).Replace('\\', '/');
        return AssetDatabase.LoadAssetAtPath(iconFile, typeof (Texture2D)) as Texture2D;
    }

    private void ImportFromGMapCatcher()
    {
        string folder = EditorUtility.OpenFolderPanel("Select GMapCatcher tiles folder", string.Empty, "");
        if (string.IsNullOrEmpty(folder)) return;

        string[] files = Directory.GetFiles(folder, "*.png", SearchOption.AllDirectories);
        if (files.Length == 0) return;

        const string resPath = "Assets/Resources/OnlineMapsTiles";

        bool needAsk = true;
        bool overwrite = false;
        foreach (string file in files)
        {
            if (!ImportTileFromGMapCatcher(file, folder, resPath, ref overwrite, ref needAsk)) break;
        }

        AssetDatabase.Refresh();
    }

    private static bool ImportTileFromGMapCatcher(string file, string folder, string resPath, ref bool overwrite, ref bool needAsk)
    {
        string shortPath = file.Substring(folder.Length + 1);
        shortPath = shortPath.Replace('\\', '/');
        string[] shortArr = shortPath.Split(new[] {'/'});
        int zoom = 17 - int.Parse(shortArr[0]);
        int x = int.Parse(shortArr[1]) * 1024 + int.Parse(shortArr[2]);
        int y = int.Parse(shortArr[3]) * 1024 + int.Parse(shortArr[4].Substring(0, shortArr[4].Length - 4));
        string dir = Path.Combine(resPath, string.Format("{0}/{1}", zoom, x));
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        string destFileName = Path.Combine(dir, y + ".png");
        if (File.Exists(destFileName))
        {
            if (needAsk)
            {
                needAsk = false;
                int result = EditorUtility.DisplayDialogComplex("File already exists", "File already exists. Overwrite?", "Overwrite", "Skip", "Cancel");
                if (result == 0) overwrite = true;
                else if (result == 1)
                {
                    overwrite = false;
                    return true;
                }
                else return false;
            }

            if (!overwrite) return true;
        }
        File.Copy(file, destFileName, true);
        return true;
    }

    private void OnAbout()
    {
        OnlineMapsAboutWindow.OpenWindow();
    }

    private void OnCheckUpdates()
    {
        OnlineMapsUpdater.OpenWindow();
    }

    private void OnEnable()
    {
        api = (OnlineMaps) target;

        double lng, lat;
        api.GetPosition(out lng, out lat);

        if (api._position != Vector2.zero && lng == 0 && lat == 0)
        {
            api.position = api._position;
            api._position = Vector2.zero;
        }

        if (api.defaultMarkerTexture == null) api.defaultMarkerTexture = GetIcon("DefaultMarker.png");
        if (api.skin == null)
        {
            api.skin =
                (GUISkin)
                    AssetDatabase.LoadAssetAtPath("Assets/Infinity Code/Online maps/Skin/DefaultSkin.guiskin",
                        typeof (GUISkin));
        }

        string[] files = Directory.GetFiles("Assets", "update_available.png", SearchOption.AllDirectories);
        if (files.Length > 0)
        {
            Texture updateAvailableIcon = AssetDatabase.LoadAssetAtPath(files[0], typeof (Texture)) as Texture;
            updateAvailableContent = new GUIContent("Update Available", updateAvailableIcon, "Update available");
        }

        OnlineMapsUpdater.CheckNewVersionAvailable();
    }

    public override void OnInspectorGUI()
    {
        DrawToolbarGUI();

        bool dirty = DrawGeneralGUI();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        showMarkers = Foldout(showMarkers, "2D Markers");
        if (showMarkers) DrawMarkersGUI();
        EditorGUILayout.EndVertical();

        if (api.target == OnlineMapsTarget.texture)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            showCreateTexture = Foldout(showCreateTexture, "Create texture");
            if (showCreateTexture) DrawCreateTextureGUI(ref dirty);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginVertical(GUI.skin.box);
        showAdvanced = Foldout(showAdvanced, "Advanced");
        if (showAdvanced) DrawAdvancedGUI(ref dirty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        showTroubleshooting = Foldout(showTroubleshooting, "Troubleshooting");
        if (showTroubleshooting) DrawTroubleshootingGUI(ref dirty);
        EditorGUILayout.EndVertical();

        OnlineMapsControlBase[] controls = api.GetComponents<OnlineMapsControlBase>();
        if (controls == null || controls.Length == 0)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox("Problem detected:\nCan not find OnlineMaps Control component.", MessageType.Error);
            if (GUILayout.Button("Add Control"))
            {
                GenericMenu menu = new GenericMenu();

                Type[] types = api.GetType().Assembly.GetTypes();
                foreach (Type t in types)
                {
                    if (t.IsSubclassOf(typeof(OnlineMapsControlBase))) 
                    {
                        if (t == typeof(OnlineMapsControlBase2D) || t == typeof(OnlineMapsControlBase3D)) continue;

                        string fullName = t.FullName.Substring(10);

                        int controlIndex = fullName.IndexOf("Control");
                        fullName = fullName.Insert(controlIndex, " ");

                        int textureIndex = fullName.IndexOf("Texture");
                        if (textureIndex > 0) fullName = fullName.Insert(textureIndex, " ");

                        menu.AddItem(new GUIContent(fullName), false, data =>
                        {
                            Type ct = data as Type;
                            api.gameObject.AddComponent(ct);
                            api.target = (ct == typeof (OnlineMapsTileSetControl))
                                ? OnlineMapsTarget.tileset
                                : OnlineMapsTarget.texture;
                            Repaint();
                        }, t);
                    }
                }

                menu.ShowAsContext();
            }

            EditorGUILayout.EndVertical();
        }

        if (dirty)
        {
            EditorUtility.SetDirty(api);
            Repaint();
        }
    }

    private void OnProductPage()
    {
        Process.Start("http://infinity-code.com/en/products/online-maps");
    }

    private void OnSendMail()
    {
        Process.Start("mailto:support@infinity-code.com?subject=Online maps");
    }

    private void OnViewAPI()
    {
        Process.Start("http://infinity-code.com/en/docs/api/online-maps");
    }

    private void OnViewAtlas()
    {
        Process.Start("http://infinity-code.com/atlas/online-maps");
    }

    private void OnViewDocs()
    {
        Process.Start("http://infinity-code.com/en/docs/online-maps");
    }

    private void OnViewForum()
    {
        Process.Start("http://forum.infinity-code.com");
    }

    private void ResetSaveSettings()
    {
        showSave = false;

        saveControl = true;
        saveLocationService = true;
        saveMarkers = true;
        saveMarkers3D = true;
        saveSettings = true;
        saveTexture = true;
    }
}