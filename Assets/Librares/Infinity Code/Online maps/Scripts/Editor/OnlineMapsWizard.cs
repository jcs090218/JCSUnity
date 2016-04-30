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

#if !UNITY_4_3 && !UNITY_4_5
using UnityEngine.UI;
#endif

#if IGUI
using iGUI;
#endif

[Serializable]
public class OnlineMapsWizard : EditorWindow
{
    private GameObject NGUIParent;
    private Camera activeCamera;
    private bool allowCameraControl;
    private string[] availableSizesStr;
    private string bingAPI;
    private string customProviderURL = "http://localhost/{zoom}/{y}/{x}";
    private Shader defaultTilesetShader;
    private Shader drawingShader;
    private bool labels;
    private string language = "en";
    private int mapControl2D = 0;
    private int mapControl3D = 0;
    private int mapProviderType = 0;
    private int mapType = 1;
    private Material markerMaterial;
    private Shader markerShader;
    private bool moveCameraToTileset = true;
    private OnlineMapsProviderEnum provider = OnlineMapsProviderEnum.arcGis;
    private Vector2 scrollPosition;
    private bool showCustomProviderTokens;
    private OnlineMapsSource source = OnlineMapsSource.Online;
    private List<OnlineMapsWizardDelegate> steps;
    private string textureFilename = "OnlineMaps";
    private int textureHeight = 512;
    private int textureWidth = 512;
    private Material tileMaterial;
    private int tilesetHeight = 1024;
    private Shader tilesetShader;
    private Vector2 tilesetSize = new Vector2(1024, 1024);
    private int tilesetWidth = 1024;
    private bool traffic;
    private GameObject uGUIParent;
    private bool useBuildings;
    private bool useElevation;
    private bool useLocationService;
    private bool useRWT;
    private string webplayerProxyURL = "http://service.infinity-code.com/redirect.php?";
    private bool smoothZoom = true;
    private bool smartTexture = true;
    private GameObject DFGUIParent;
    private GameObject IGUIParent;

    public bool availableLabels
    {
        get
        {
            if (provider == OnlineMapsProviderEnum.google && mapProviderType == 0) return true;
            if (provider == OnlineMapsProviderEnum.nokia && mapProviderType == 0) return true;
            if (provider == OnlineMapsProviderEnum.virtualEarth && mapProviderType == 0) return true;
            return false;
        }
    }

    public bool availableLanguage
    {
        get
        {
            if (provider == OnlineMapsProviderEnum.arcGis) return false;
            if (provider == OnlineMapsProviderEnum.mapQuest) return false;
            if (provider == OnlineMapsProviderEnum.openStreetMap) return false;
            if (provider == OnlineMapsProviderEnum.sputnik) return false;
            return true;
        }
    }

    public string[] availableTypes
    {
        get
        {
            string[] types = {"Satellite", "Relief", "Terrain", "Map"};
            if (provider == OnlineMapsProviderEnum.arcGis) return new[] {types[0], types[2]};
            if (provider == OnlineMapsProviderEnum.google) return new[] {types[0], types[1], types[2]};
            if (provider == OnlineMapsProviderEnum.mapQuest) return new[] {types[0], types[2]};
            if (provider == OnlineMapsProviderEnum.nokia) return new[] {types[0], types[2], types[3]};
            if (provider == OnlineMapsProviderEnum.openStreetMap) return null;
            if (provider == OnlineMapsProviderEnum.virtualEarth) return new[] {types[0], types[2]};
            if (provider == OnlineMapsProviderEnum.sputnik) return null;
            if (provider == OnlineMapsProviderEnum.custom) return null;
            return types;
        }
    }

    public bool enabledLabels
    {
        get
        {
            if (provider == OnlineMapsProviderEnum.arcGis && mapType == 1) return true;
            if (provider == OnlineMapsProviderEnum.google) return true;
            if (provider == OnlineMapsProviderEnum.mapQuest && mapType == 1) return true;
            if (provider == OnlineMapsProviderEnum.nokia) return true;
            if (provider == OnlineMapsProviderEnum.virtualEarth) return true;
            if (provider == OnlineMapsProviderEnum.sputnik) return true;
            return false;
        }
    }

    private float CheckCameraDistance(Camera tsCamera)
    {
        if (tsCamera == null) return -1;

        Vector3 mapCenter = new Vector3(tilesetSize.x / -2, 0, tilesetSize.y / 2);
        float distance = (tsCamera.transform.position - mapCenter).magnitude * 1.5f;
        if (distance <= tsCamera.farClipPlane) return -1;

        return distance;
    }

    private void CheckThirdPartyDirectives(ref bool allowCreate)
    {
        if (mapControl2D == 4)
        {
#if !NGUI
            EditorGUILayout.HelpBox("Make sure that you have NGUI in your project and click «Enable NGUI».", MessageType.Warning);
            allowCreate = false;
            if (GUILayout.Button("Enable NGUI", GUILayout.ExpandWidth(false)))
            {
                string currentDefinitions = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                if (currentDefinitions != "") currentDefinitions += ";";
                currentDefinitions += "NGUI";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currentDefinitions);
            }
#endif
        }
        else if (mapControl2D == 5)
        {
#if !DFGUI
            EditorGUILayout.HelpBox("Make sure that you have DF-GUI in your project and click «Enable DF-GUI».", MessageType.Warning);
            allowCreate = false;
            if (GUILayout.Button("Enable DF-GUI", GUILayout.ExpandWidth(false)))
            {
                string currentDefinitions = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                if (currentDefinitions != "") currentDefinitions += ";";
                currentDefinitions += "DFGUI";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currentDefinitions);
            }
#endif
        }
        else if (mapControl2D == 6)
        {
#if !IGUI
            EditorGUILayout.HelpBox("Make sure that you have iGUI in your project and click «Enable iGUI».", MessageType.Warning);
            allowCreate = false;
            if (GUILayout.Button("Enable iGUI", GUILayout.ExpandWidth(false)))
            {
                string currentDefinitions = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                if (currentDefinitions != "") currentDefinitions += ";";
                currentDefinitions += "IGUI";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currentDefinitions);
            }
#endif
        }
    }

    private void DrawControls(ref bool allowCreate)
    {
        if (mapType == 0)
        {
            EditorGUILayout.HelpBox(
                "All 2D controls have the same features.\nSelect a control, depending on the place where you want to show the map.",
                MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox(
                "Tileset - a dynamic mesh. Faster, uses less memory and has many additional features. It is recommended for most applications.\nTexture (Plane) - used to display maps on the plane.",
                MessageType.Info);
        }

        EditorGUILayout.LabelField("Select control");

        if (mapType == 0)
        {
            EditorGUI.BeginChangeCheck();
            mapControl2D = GUILayout.SelectionGrid(mapControl2D,
                new[] {"GUITexture", "SpriteRenderer", "UIImage", "UIRawImage", "NGUI", "DF-GUI", "iGUI"}, 1, "toggle");
            if (EditorGUI.EndChangeCheck()) InitSteps();
            CheckThirdPartyDirectives(ref allowCreate);

#if UNITY_4_3 || UNITY_4_5
            if (mapControl2D == 2 || mapControl2D == 3)
            {
                allowCreate = false;
                EditorGUILayout.HelpBox("To use UIImage and UIRawImage needs Unity v4.6 or higher.", MessageType.Error);
            }
#endif
        }
        else
        {
            EditorGUI.BeginChangeCheck();
            mapControl3D = GUILayout.SelectionGrid(mapControl3D, new[] {"Tileset", "Texture (Plane)"}, 1, "toggle");
            if (EditorGUI.EndChangeCheck()) InitSteps();
        }
    }

    private void DrawElevation(ref bool allowcreate)
    {
        EditorGUILayout.HelpBox("To use the Elevation requires the Bing Maps API Key.", MessageType.Warning);
        bingAPI = EditorGUILayout.TextField("Bing Maps API key:", bingAPI);
        if (GUILayout.Button("Create Bing Maps API Key"))
            Process.Start("https://msdn.microsoft.com/en-us/library/ff428642.aspx");
    }

    private void DrawLabels(ref bool allowCreate)
    {
        bool showLanguage;
        if (availableLabels)
        {
            labels = EditorGUILayout.Toggle("Labels: ", labels);
            showLanguage = labels;
        }
        else
        {
            showLanguage = enabledLabels;
            GUILayout.Label("Labels " + (showLanguage ? "enabled" : "disabled"));
        }
        if (showLanguage && availableLanguage)
        {
            language = EditorGUILayout.TextField("Language: ", language);
            GUILayout.Label(provider == OnlineMapsProviderEnum.nokia
                ? "Use three-letter code such as: eng"
                : "Use two-letter code such as: en");
        }
    }

    private void DrawMapType(ref bool allowCreate)
    {
        EditorGUILayout.LabelField("Select the type of map");
        EditorGUI.BeginChangeCheck();
        mapType = GUILayout.SelectionGrid(mapType, new[] {"2D", "3D"}, 1, "toggle");
        if (EditorGUI.EndChangeCheck()) InitSteps();
    }

    private void DrawMore(ref bool allowcreate)
    {
        EditorGUILayout.LabelField("More Features");
        useLocationService = EditorGUILayout.Toggle("Location Service (GPS): ", useLocationService);
        traffic = EditorGUILayout.Toggle("Traffic: ", traffic);
        useRWT = EditorGUILayout.Toggle("Real World Terrain", useRWT);

        if (mapType == 0 || (mapType == 1 && mapControl3D != 0))
        {
            smartTexture = EditorGUILayout.Toggle("Smart Texture", smartTexture);
        }

        if (mapType == 1)
            allowCameraControl = EditorGUILayout.Toggle("Camera Control", allowCameraControl);
    }

    private void DrawNGUIParent(ref bool allowCreate)
    {
        EditorGUILayout.HelpBox("Select the parent GameObject in the scene.", MessageType.Warning);
        NGUIParent = EditorGUILayout.ObjectField("Parent: ", NGUIParent, typeof (GameObject), true) as GameObject;
        if (NGUIParent == null) allowCreate = false;
#if NGUI
        else if (NGUIParent.GetComponent<UIRoot>() == null && NGUIParent.GetComponent<UIWidget>() == null)
        {
            EditorGUILayout.HelpBox("Selected the wrong parent. Parent must contain the UIRoot or UIWidget.", MessageType.Error);
            allowCreate = false;
        }
#endif

    }

    private void DrawDFGUIParent(ref bool allowCreate)
    {
        EditorGUILayout.HelpBox("Select the parent GameObject in the scene.", MessageType.Warning);
        DFGUIParent = EditorGUILayout.ObjectField("Parent: ", DFGUIParent, typeof(GameObject), true) as GameObject;
        if (DFGUIParent == null) allowCreate = false;
#if DFGUI
        else if (DFGUIParent.GetComponent<dfGUIManager>() == null && DFGUIParent.GetComponent<dfControl>() == null)
        {
            EditorGUILayout.HelpBox("Selected the wrong parent. Parent must contain the dfGUIManager or dfControl.", MessageType.Error);
            allowCreate = false;
        }
#endif

    }

    private void DrawIGUIParent(ref bool allowCreate)
    {
        EditorGUILayout.HelpBox("Select the parent GameObject in the scene.", MessageType.Warning);
        IGUIParent = EditorGUILayout.ObjectField("Parent: ", IGUIParent, typeof(GameObject), true) as GameObject;
        if (IGUIParent == null) allowCreate = false;
#if IGUI
        else if (IGUIParent.GetComponent<iGUIRoot>() == null && IGUIParent.GetComponent<iGUIElement>() == null)
        {
            EditorGUILayout.HelpBox("Selected the wrong parent. Parent must contain the iGUIRoot or iGUIElement.", MessageType.Error);
            allowCreate = false;
        }
#endif

    }

    private void DrawProvider(ref bool allowCreate)
    {
        EditorGUI.BeginChangeCheck();
        provider =
            (OnlineMapsProviderEnum)
                EditorGUILayout.EnumPopup(new GUIContent("Provider: ", "Provider of tiles"), provider);
        if (EditorGUI.EndChangeCheck()) mapProviderType = 0;

        if (provider == OnlineMapsProviderEnum.custom)
        {
            customProviderURL = EditorGUILayout.TextField("URL: ", customProviderURL);

            EditorGUILayout.BeginVertical(GUI.skin.box);
            showCustomProviderTokens = OnlineMapsEditor.Foldout(showCustomProviderTokens, "Available tokens");
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
    }

    private void DrawSource(ref bool allowCreate)
    {
        source = (OnlineMapsSource) EditorGUILayout.EnumPopup("Source: ", source);

        if (source != OnlineMapsSource.Resources)
        {
            webplayerProxyURL = EditorGUILayout.TextField("Proxy (for Webplayer): ", webplayerProxyURL);

            DrawProvider(ref allowCreate);

            if (availableTypes != null)
            {
                GUIContent[] aviableTypes = availableTypes.Select(t => new GUIContent(t)).ToArray();
                if (aviableTypes != null)
                {
                    mapProviderType = EditorGUILayout.Popup(new GUIContent("Type: ", "Type of map texture"),
                        mapProviderType, aviableTypes);
                }
            }

            DrawLabels(ref allowCreate);
        }
    }

    private void DrawTextureSize(ref bool allowCreate)
    {
        EditorGUILayout.LabelField("Texture Size");
        if (availableSizesStr == null || availableSizesStr.Length == 0)
            availableSizesStr = OnlineMapsEditor.availableSizes.Select(s => s.ToString()).ToArray();

        if (!OnlineMapsEditor.availableSizes.Contains(textureWidth)) textureWidth = 512;
        if (!OnlineMapsEditor.availableSizes.Contains(textureHeight)) textureHeight = 512;

        textureWidth = EditorGUILayout.IntPopup("Width: ", textureWidth,
            availableSizesStr, OnlineMapsEditor.availableSizes);
        textureHeight = EditorGUILayout.IntPopup("Height: ", textureHeight,
            availableSizesStr, OnlineMapsEditor.availableSizes);

        textureFilename = EditorGUILayout.TextField("Filename: ", textureFilename);
    }

    private void DrawTilesetCamera(ref bool allowCreate)
    {
        EditorGUILayout.LabelField("Camera Settings");

        Camera tsCamera = (activeCamera != null) ? activeCamera : Camera.main;
        activeCamera = EditorGUILayout.ObjectField("Camera: ", tsCamera, typeof (Camera), true) as Camera;
        moveCameraToTileset = EditorGUILayout.Toggle("Move camera to Tileset", moveCameraToTileset);

        float needFixCameraDistance = CheckCameraDistance(activeCamera);

        if (needFixCameraDistance != -1)
        {
            EditorGUILayout.HelpBox(
                "Potential problem detected:\n\"Camera - Clipping Planes - Far\" is too small.",
                MessageType.Warning);

            if (GUILayout.Button("Fix Clipping Planes - Far"))
                tsCamera.farClipPlane = needFixCameraDistance;
        }
    }

    private void DrawTilesetMaterialsAndShaders(ref bool allowcreate)
    {
        EditorGUILayout.LabelField("Materials and Shaders (optional)");

        tileMaterial =
            EditorGUILayout.ObjectField("Tile material: ", tileMaterial, typeof (Material), false) as Material;
        markerMaterial =
            EditorGUILayout.ObjectField("Marker Material:", markerMaterial, typeof (Material), false) as Material;
        tilesetShader = EditorGUILayout.ObjectField("Tileset Shader:", tilesetShader, typeof (Shader), true) as Shader;
        markerShader = EditorGUILayout.ObjectField("Marker Shader:", markerShader, typeof (Shader), false) as Shader;
        drawingShader = EditorGUILayout.ObjectField("Drawing Shader:", drawingShader, typeof (Shader), false) as Shader;
    }

    private void DrawTilesetMore(ref bool allowCreate)
    {
        DrawMore(ref allowCreate);

        smoothZoom = EditorGUILayout.Toggle("Smooth zoom", smoothZoom);

        EditorGUI.BeginChangeCheck();
        useElevation = EditorGUILayout.Toggle("Elevation", useElevation);
        if (EditorGUI.EndChangeCheck()) InitSteps();

        useBuildings = EditorGUILayout.Toggle("Buildings", useBuildings);
    }

    private void DrawTilesetSize(ref bool allowCreate)
    {
        EditorGUILayout.LabelField("Size");
        tilesetWidth = EditorGUILayout.IntField("Width (pixels): ", tilesetWidth);
        tilesetHeight = EditorGUILayout.IntField("Height (pixels): ", tilesetHeight);
        tilesetSize = EditorGUILayout.Vector2Field("Size (in scene): ", tilesetSize);

        textureWidth = Mathf.ClosestPowerOfTwo(textureWidth);
        if (textureWidth < 512) textureWidth = 512;

        tilesetHeight = Mathf.ClosestPowerOfTwo(tilesetHeight);
        if (tilesetHeight < 512) tilesetHeight = 512;
    }

    private void DrawUGUIParent(ref bool allowCreate)
    {
        EditorGUILayout.HelpBox("Select the parent GameObject in the scene.", MessageType.Warning);
        uGUIParent = EditorGUILayout.ObjectField("Parent: ", uGUIParent, typeof (GameObject), true) as GameObject;
        if (uGUIParent == null) allowCreate = false;
#if !UNITY_4_3 && !UNITY_4_5
        else if (uGUIParent.GetComponent<CanvasRenderer>() == null && uGUIParent.GetComponent<Canvas>() == null)
        {
            EditorGUILayout.HelpBox("Selected the wrong parent. Parent must contain the Canvas or Canvas Renderer.", MessageType.Error);
            allowCreate = false;
        }
#endif
    }

    private void InitSteps()
    {
        steps = new List<OnlineMapsWizardDelegate>();

        steps.Add(DrawMapType);
        steps.Add(DrawControls);
        steps.Add(DrawSource);

        if (mapType == 0 || (mapType == 1 && mapControl3D == 1))
        {
            if (mapType == 0)
            {
                if (mapControl2D == 2 || mapControl2D == 3) steps.Add(DrawUGUIParent);
                else if (mapControl2D == 4) steps.Add(DrawNGUIParent);
                else if (mapControl2D == 5) steps.Add(DrawDFGUIParent);
                else if (mapControl2D == 6) steps.Add(DrawIGUIParent);
            }

            steps.Add(DrawTextureSize);
            steps.Add(DrawMore);
        }
        else if (mapControl3D == 0)
        {
            steps.Add(DrawTilesetSize);
            steps.Add(DrawTilesetCamera);
            steps.Add(DrawTilesetMaterialsAndShaders);
            steps.Add(DrawTilesetMore);

            if (useElevation)
                steps.Add(DrawElevation);
        }
    }

    private void OnEnable()
    {
        InitSteps();
        defaultTilesetShader = Shader.Find("Infinity Code/Online Maps/Tileset");
        tilesetShader = defaultTilesetShader;
        markerShader = Shader.Find("Transparent/Diffuse");
        drawingShader = Shader.Find("Infinity Code/Online Maps/Tileset DrawingElement");
        activeCamera = Camera.main;
    }

    private void OnGUI()
    {
        if (CheckMapInScene()) return;

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        bool allowCreate = true;

        if (steps == null || steps.Count == 0) InitSteps();

        foreach (OnlineMapsWizardDelegate s in steps)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            s(ref allowCreate);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUIStyle.none);

        EditorGUI.BeginDisabledGroup(!allowCreate);
        if (GUILayout.Button("Create", GUILayout.ExpandWidth(false)))
        {
            CreateMap();
            Close();
        }

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();
    }

    private bool CheckMapInScene()
    {
        OnlineMaps map = FindObjectOfType<OnlineMaps>();
        if (map != null)
        {
            EditorGUILayout.HelpBox("In the scene already have a map. You can only use one instance of map in the scene.", MessageType.Error);
            if (GUILayout.Button("Delete map")) DestroyImmediate(map.gameObject);
            if (GUILayout.Button("Close Wizard")) Close();
            return true;
        }
        return false;
    }

    private void CreateMap()
    {
        OnlineMaps map = CreateMapGameObject();
        GameObject go = map.gameObject;
        if (mapType == 0)
        {
            Texture2D texture = CreateTexture(map);

            if (mapControl2D == 0)
            {
                go.AddComponent<OnlineMapsGUITextureControl>();
                GUITexture guiTexture = go.GetComponent<GUITexture>();
                Debug.Log(guiTexture + "   " + texture);
                guiTexture.texture = texture;
                go.transform.localPosition = new Vector3(0.5f, 0.5f);
                go.transform.localScale = Vector3.zero;
                guiTexture.pixelInset = new Rect(textureWidth / -2, textureHeight / -2, textureWidth, textureHeight);
            }
            else if (mapControl2D == 1)
            {
                go.AddComponent<OnlineMapsSpriteRendererControl>();
                SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, textureWidth, textureHeight), Vector2.zero);
                go.AddComponent<BoxCollider>();
            }
#if !UNITY_4_3 && !UNITY_4_5
            else if (mapControl2D == 2 || mapControl2D == 3)
            {
                RectTransform rectTransform = go.AddComponent<RectTransform>();
                rectTransform.SetParent(uGUIParent.transform as RectTransform);
                go.AddComponent<CanvasRenderer>();
                rectTransform.localPosition = Vector3.zero;
                rectTransform.anchorMax = rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.sizeDelta = new Vector2(textureWidth, textureHeight);

                if (mapControl2D == 2)
                {
                    go.AddComponent<OnlineMapsUIImageControl>();
                    Image image = go.AddComponent<Image>();
                    image.sprite = Sprite.Create(texture, new Rect(0, 0, textureWidth, textureHeight), Vector2.zero);
                }
                else
                {
                    go.AddComponent<OnlineMapsUIRawImageControl>();
                    RawImage image = go.AddComponent<RawImage>();
                    image.texture = texture;
                }
            }
#endif
#if NGUI
            else if (mapControl2D == 4)
            {
                go.layer = NGUIParent.layer;
                UITexture uiTexture = go.AddComponent<UITexture>();
                uiTexture.mainTexture = texture;
                uiTexture.width = textureWidth;
                uiTexture.height = textureHeight;
                go.transform.parent = NGUIParent.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.Euler(Vector3.zero);
                BoxCollider boxCollider = go.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(textureWidth, textureHeight, 0);
                go.AddComponent<OnlineMapsNGUITextureControl>();
            }
#endif
#if DFGUI
            else if (mapControl2D == 5)
            {
                go.transform.parent = DFGUIParent.transform;

                dfTextureSprite textureSprite = go.AddComponent<dfTextureSprite>();
                textureSprite.Texture = texture;
                textureSprite.Width = textureWidth;
                textureSprite.Height = textureHeight;
                textureSprite.Pivot = dfPivotPoint.MiddleCenter;
                textureSprite.transform.localPosition = Vector3.zero;

                go.AddComponent<OnlineMapsDFGUITextureControl>();
            }
#endif
#if IGUI
            else if (mapControl2D == 6)
            {
                go.transform.parent = IGUIParent.transform;

                iGUIImage image = go.AddComponent<iGUIImage>();
                image.image = texture;
                image.positionAndSize = new Rect(0, 0, 1, 1);

                go.AddComponent<OnlineMapsIGUITextureControl>();
            }
#endif

            map.useSmartTexture = smartTexture;
            map.redrawOnPlay = true;
        }
        else
        {
            OnlineMapsControlBase3D control3D = null;

            if (mapControl3D == 0)
            {
                map.target = OnlineMapsTarget.tileset;
                map.tilesetWidth = tilesetWidth;
                map.tilesetHeight = tilesetHeight;
                map.tilesetSize = tilesetSize;

                OnlineMapsTileSetControl ts = go.AddComponent<OnlineMapsTileSetControl>();
                control3D = ts;
                ts.useElevation = useElevation;
                ts.bingAPI = bingAPI;
                ts.smoothZoom = smoothZoom;
                ts.tileMaterial = tileMaterial;
                ts.markerMaterial = markerMaterial;
                ts.tilesetShader = tilesetShader;
                ts.drawingShader = drawingShader;
                ts.markerShader = markerShader;

                if (moveCameraToTileset)
                {
                    GameObject cameraGO = activeCamera.gameObject;
                    float minSide = Mathf.Min(tilesetSize.x, tilesetSize.y);
                    Vector3 position = new Vector3(tilesetSize.x / -2, minSide, tilesetSize.y / 2);
                    cameraGO.transform.position = position;
                    cameraGO.transform.rotation = Quaternion.Euler(90, 180, 0);
                }

                if (useBuildings)
                {
                    go.AddComponent<OnlineMapsBuildings>();
                }
            }
            else if (mapControl3D == 1)
            {
                control3D = go.AddComponent<OnlineMapsTextureControl>();
                map.useSmartTexture = smartTexture;
                map.redrawOnPlay = true;
            }

            if (control3D != null)
            {
                control3D.activeCamera = activeCamera;
                control3D.allowCameraControl = allowCameraControl;
            }
        }

        if (useLocationService) go.AddComponent<OnlineMapsLocationService>();
        if (useRWT) go.AddComponent<OnlineMapsRWTConnector>();

        EditorGUIUtility.PingObject(go);
        Selection.activeGameObject = go;
    }

    private Texture2D CreateTexture(OnlineMaps map)
    {
        string texturePath = string.Format("Assets/{0}.png", textureFilename);
        map.texture = new Texture2D(textureWidth, textureHeight);
        File.WriteAllBytes(texturePath, map.texture.EncodeToPNG());
        AssetDatabase.Refresh();
        TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        if (textureImporter != null)
        {
            textureImporter.mipmapEnabled = false;
            textureImporter.isReadable = true;
            textureImporter.textureFormat = TextureImporterFormat.RGB24;
            textureImporter.maxTextureSize = Mathf.Max(textureWidth, textureHeight);

            if (mapType == 0 && (mapControl2D == 1 || mapControl2D == 2))
            {
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                textureImporter.npotScale = TextureImporterNPOTScale.None;
            }

            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
            map.texture = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
        }
        return map.texture;
    }

    private OnlineMaps CreateMapGameObject()
    {
        GameObject go = new GameObject("Map");
        OnlineMaps map = go.AddComponent<OnlineMaps>();

        map.source = source;
        map.provider = provider;
        map.type = mapProviderType;
        map.webplayerProxyURL = webplayerProxyURL;
        map.labels = labels;
        map.customProviderURL = customProviderURL;
        map.language = language;
        map.traffic = traffic;

        return map;
    }

    [MenuItem("GameObject/Create Other/Map")]
    public static void OpenWindow()
    {
        GetWindow<OnlineMapsWizard>(true, "Create Map", true);
    }

    private delegate void OnlineMapsWizardDelegate(ref bool allowCreate);
}