/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class OnlineMapsPrefs
{
    private const string prefsKey = "OM_Settings";

    static OnlineMapsPrefs()
    {
        EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
    }

    private static bool Exists()
    {
        return EditorPrefs.HasKey(prefsKey);
    }

    private static Object GetObject(int tid)
    {
        if (tid == 0) return null;
        return EditorUtility.InstanceIDToObject(tid);
    }

    private static void Load(OnlineMaps api)
    {
        if (!Exists()) return;

        string s = EditorPrefs.GetString(prefsKey);
        OnlineMapsXML node = OnlineMapsXML.Load(s);

        foreach (OnlineMapsXML el in node)
        {
            string name = el.name;
            if (name == "Settings") LoadSettings(el, api);
            else if (name == "Control") LoadControl(el, api);
            else if(name == "Markers") LoadMarkers(el, api);
            else if (name == "Markers3D") LoadMarkers3D(el, api);
            else if (name == "LocationService") LoadLocationService(el, api);
        }

        EditorPrefs.DeleteKey(prefsKey);
    }

    private static void LoadControl(OnlineMapsXML el, OnlineMaps api)
    {
        OnlineMapsControlBase control = api.GetComponent<OnlineMapsControlBase>();
        if (control == null) return;

        control.allowAddMarkerByM = el.Get<bool>("AllowAddMarkerByM");
        control.allowZoom = el.Get<bool>("AllowZoom");
        control.allowUserControl = el.Get<bool>("AllowUserControl");
        control.zoomInOnDoubleClick = el.Get<bool>("ZoomInOnDoubleClick");

        if (control is OnlineMapsControlBase3D)
        {
            OnlineMapsControlBase3D control3D = control as OnlineMapsControlBase3D;

            control3D.allowAddMarker3DByN = el.Get<bool>("AllowAddMarker3DByN");
            control3D.allowCameraControl = el.Get<bool>("AllowCameraControl");
            control3D.marker2DMode = (OnlineMapsMarker2DMode)el.Get<int>("Marker2DMode");
            control3D.marker2DSize = el.Get<float>("Marker2DSize");
            control3D.marker3DScale = el.Get<float>("Marker3DScale");
            control3D.activeCamera = GetObject(el.Get<int>("Camera")) as Camera;

            if (control3D.allowCameraControl)
            {
                control3D.cameraDistance = el.Get<float>("CameraDistance");
                control3D.cameraRotation = el.Get<Vector2>("CameraRotation");
                control3D.cameraSpeed = el.Get<Vector2>("CameraSpeed");
            }
        }

        if (control is OnlineMapsTileSetControl)
        {
            OnlineMapsTileSetControl ts = control as OnlineMapsTileSetControl;

            ts.checkMarker2DVisibility = (OnlineMapsTilesetCheckMarker2DVisibility)el.Get<int>("CheckMarker2DVisibility");
            ts.smoothZoom = el.Get<bool>("SmoothZoom");
            ts.useElevation = el.Get<bool>("UseElevation");
            ts.tileMaterial = GetObject(el.Get<int>("TileMaterial")) as Material;
            ts.tilesetShader = GetObject(el.Get<int>("TileShader")) as Shader;
            ts.drawingShader = GetObject(el.Get<int>("DrawingShader")) as Shader;
            ts.markerMaterial = GetObject(el.Get<int>("MarkerMaterial")) as Material;
            ts.markerShader = GetObject(el.Get<int>("MarkerShader")) as Shader;
        }
    }

    private static void LoadLocationService(OnlineMapsXML el, OnlineMaps api)
    {
        OnlineMapsLocationService ls = api.GetComponent<OnlineMapsLocationService>();
        ls.desiredAccuracy = el.Get<float>("DesiredAccuracy");
        ls.updatePosition = el.Get<bool>("UpdatePosition");
        ls.createMarkerInUserPosition = el.Get<bool>("CreateMarkerInUserPosition");
        ls.restoreAfter = el.Get<int>("RestoreAfter");

        if (ls.createMarkerInUserPosition)
        {
            ls.markerType = (OnlineMapsLocationServiceMarkerType) el.Get<int>("MarkerType");

            if (ls.markerType == OnlineMapsLocationServiceMarkerType.twoD)
            {
                ls.marker2DTexture = GetObject(el.Get<int>("Marker2DTexture")) as Texture2D;
                ls.marker2DAlign = (OnlineMapsAlign) el.Get<int>("Marker2DAlign");
            }
            else ls.marker3DPrefab = GetObject(el.Get<int>("Marker3DPrefab")) as GameObject;

            ls.markerTooltip = el.Get<string>("MarkerTooltip");
            ls.useCompassForMarker = el.Get<bool>("UseCompassForMarker");
        }

        ls.useGPSEmulator = el.Get<bool>("UseGPSEmulator");
        if (ls.useGPSEmulator)
        {
            ls.emulatorPosition = el.Get<Vector2>("EmulatorPosition");
            ls.emulatorCompass = el.Get<float>("EmulatorCompass");
        }
    }

    private static void LoadMarkers(OnlineMapsXML el, OnlineMaps api)
    {
        List<OnlineMapsMarker> markers = new List<OnlineMapsMarker>();

        foreach (OnlineMapsXML m in el)
        {
            OnlineMapsMarker marker = new OnlineMapsMarker();
            marker.position = m.Get<Vector2>("Position");
            marker.range = m.Get<OnlineMapsRange>("Range");
            marker.label = m.Get<string>("Label");
            marker.texture = GetObject(m.Get<int>("Texture")) as Texture2D;
            marker.align = (OnlineMapsAlign)m.Get<int>("Align");
            marker.rotation = m.Get<float>("Rotation");
            markers.Add(marker);
        }

        api.markers = markers.ToArray();
    }

    private static void LoadMarkers3D(OnlineMapsXML el, OnlineMaps api)
    {
        OnlineMapsControlBase3D control = api.GetComponent<OnlineMapsControlBase3D>();
        List<OnlineMapsMarker3D> markers = new List<OnlineMapsMarker3D>();

        foreach (OnlineMapsXML m in el)
        {
            OnlineMapsMarker3D marker = new OnlineMapsMarker3D();
            marker.position = m.Get<Vector2>("Position");
            marker.range = m.Get<OnlineMapsRange>("Range");
            marker.label = m.Get<string>("Label");
            marker.prefab = GetObject(m.Get<int>("Prefab")) as GameObject;
            marker.rotation = Quaternion.Euler(m.Get<Vector3>("Rotation"));
            markers.Add(marker);
        }
        control.markers3D = markers.ToArray();
    }

    private static void LoadSettings(OnlineMapsXML el, OnlineMaps api)
    {
        api.position = el.Get<Vector2>("Position");
        api.zoom = el.Get<int>("Zoom");

        if (api.target == OnlineMapsTarget.texture)
        {
            api.texture = GetObject(el.Get<int>("Texture")) as Texture2D;
        }
        else
        {
            api.tilesetWidth = el.Get<int>("TilesetWidth");
            api.tilesetHeight = el.Get<int>("TilesetHeight");
            api.tilesetSize = el.Get<Vector2>("TilesetSize");
        }

        api.source = (OnlineMapsSource) el.Get<int>("Source");
        api.provider = (OnlineMapsProviderEnum) el.Get<int>("Provider");
        if (api.provider == OnlineMapsProviderEnum.custom) api.customProviderURL = el.Get<string>("CustomProviderURL");
        api.type = el.Get<int>("Prefs");
        api.labels = el.Get<bool>("Labels");
        api.traffic = el.Get<bool>("Traffic");
        api.redrawOnPlay = el.Get<bool>("RedrawOnPlay");
        api.useSmartTexture = el.Get<bool>("UseSmartTexture");
        api.emptyColor = el.Get<Color>("EmptyColor");
        api.defaultTileTexture = GetObject(el.Get<int>("DefaultTileTexture")) as Texture2D;
        api.skin = GetObject(el.Get<int>("Skin")) as GUISkin;
        api.defaultMarkerTexture = GetObject(el.Get<int>("DefaultMarkerTexture")) as Texture2D;
        api.defaultMarkerAlign = (OnlineMapsAlign) el.Get<int>("DefaultMarkerAlign");
        api.showMarkerTooltip = (OnlineMapsShowMarkerTooltip) el.Get<int>("ShowMarkerTooltip");
        api.useSoftwareJPEGDecoder = el.Get<bool>("UseSoftwareJPEGDecoder");
    }

    private static void PlaymodeStateChanged()
    {
        if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if (Exists())
            {
#pragma warning disable 618
                OnlineMaps api = ((OnlineMaps[]) Object.FindSceneObjectsOfType(typeof (OnlineMaps))).FirstOrDefault();
#pragma warning restore 618
                if (api != null)
                {
                    Load(api);
                    EditorUtility.SetDirty(api);
                }
            }
        }
    }

    public static void Save(string data)
    {
        EditorPrefs.SetString(prefsKey, data);
    }
}