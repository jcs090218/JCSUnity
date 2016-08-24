/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

[CustomEditor(typeof (OnlineMapsTileSetControl))]
public class OnlineMapsTilesetControlEditor : OnlineMapsTextureControlEditor
{
    private bool showShaders;
    private OnlineMapsTileSetControl tilesetControl;
    private Shader defaultTilesetShader;

    private void CheckCameraDistance(OnlineMaps api)
    {
        if (api == null) return;

        Camera tsCamera = (tilesetControl.activeCamera != null) ? tilesetControl.activeCamera : Camera.main;

        if (tsCamera == null) return;

        Vector3 mapCenter = api.transform.position + new Vector3(api.tilesetSize.x / -2, 0, api.tilesetSize.y / 2);
        float distance = (tsCamera.transform.position - mapCenter).magnitude * 1.5f;
        if (distance <= tsCamera.farClipPlane) return;

        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.HelpBox(
            "Potential problem detected:\n\"Camera - Clipping Planes - Far\" is too small.",
            MessageType.Warning);

        if (GUILayout.Button("Fix Clipping Planes - Far"))
        {
            tsCamera.farClipPlane = distance;
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawElevationGUI(OnlineMapsTileSetControl tilesetControl, ref bool dirty)
    {
        bool useElevation = tilesetControl.useElevation;

        if (useElevation) EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUI.BeginChangeCheck();
        tilesetControl.useElevation = EditorGUILayout.Toggle("Use elevation", tilesetControl.useElevation);
        if (EditorGUI.EndChangeCheck())
        {
            dirty = true;
            if (EditorApplication.isPlaying) control.UpdateControl();
        }

        if (tilesetControl.useElevation)
        {
            EditorGUI.BeginChangeCheck();
            tilesetControl.elevationScale = EditorGUILayout.FloatField("Elevation Scale:", tilesetControl.elevationScale);
            if (EditorGUI.EndChangeCheck())
            {
                dirty = true;
                if (EditorApplication.isPlaying) OnlineMaps.instance.Redraw();
            }

            if (tilesetControl.elevationZoomRange == null) tilesetControl.elevationZoomRange = new OnlineMapsRange(11, 20);

            float min = tilesetControl.elevationZoomRange.min;
            float max = tilesetControl.elevationZoomRange.max;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.MinMaxSlider(new GUIContent(string.Format("Zoom ({0}-{1}): ", min, max)), ref min, ref max, 3, 20);
            tilesetControl.elevationZoomRange.min = Mathf.Clamp(Mathf.RoundToInt(min), 3, 20);
            tilesetControl.elevationZoomRange.max = Mathf.Clamp(Mathf.RoundToInt(max), 3, 20);

            tilesetControl.bingAPI = EditorGUILayout.TextField("Bing Maps API key:", tilesetControl.bingAPI);

            if (EditorGUI.EndChangeCheck()) dirty = true;
            if (GUILayout.Button("Create Bing Maps API Key")) Process.Start("https://msdn.microsoft.com/en-us/library/ff428642.aspx");
        }

        if (useElevation) EditorGUILayout.EndVertical();
    }

    public override void DrawMarker2DPropsGUI()
    {
        base.DrawMarker2DPropsGUI();

        if (tilesetControl.marker2DMode == OnlineMapsMarker2DMode.flat)
        {
            tilesetControl.checkMarker2DVisibility = (OnlineMapsTilesetCheckMarker2DVisibility)EditorGUILayout.EnumPopup("Check marker 2D visibility",
                tilesetControl.checkMarker2DVisibility);
        }
    }

    private void DrawMaterialsAndShaders(ref bool dirty)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        showShaders = GUILayout.Toggle(showShaders, "Materials & Shaders", EditorStyles.foldout);

        if (showShaders)
        {
            EditorGUI.BeginChangeCheck();
            tilesetControl.tileMaterial = (Material)EditorGUILayout.ObjectField("Tile material: ", tilesetControl.tileMaterial, typeof (Material), false);
            tilesetControl.markerMaterial = EditorGUILayout.ObjectField("Marker Material:", tilesetControl.markerMaterial, typeof(Material), false) as Material;
            tilesetControl.tilesetShader = EditorGUILayout.ObjectField("Tileset Shader:", tilesetControl.tilesetShader, typeof (Shader), true) as Shader;
            tilesetControl.markerShader = EditorGUILayout.ObjectField("Marker Shader:", tilesetControl.markerShader, typeof (Shader), false) as Shader;
            tilesetControl.drawingShader = EditorGUILayout.ObjectField("Drawing Shader:", tilesetControl.drawingShader, typeof (Shader), false) as Shader;
            if (EditorGUI.EndChangeCheck()) dirty = true;
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawMoveCameraGUI(OnlineMaps api)
    {
        if (!GUILayout.Button("Move camera to center of Tileset")) return;
        if (api == null) return;

        Camera tsCamera = (tilesetControl.activeCamera != null) ? tilesetControl.activeCamera : Camera.main;

        if (tsCamera == null)
        {
            Debug.Log("Camera is null");
            return;
        }

        GameObject go = tsCamera.gameObject;
        float minSide = Mathf.Min(api.tilesetSize.x, api.tilesetSize.y);
        Vector3 position = api.transform.position + api.transform.rotation * new Vector3(api.tilesetSize.x / -2, minSide, api.tilesetSize.y / 2);
        go.transform.position = position;
        go.transform.rotation = api.transform.rotation * Quaternion.Euler(90, 180, 0);
    }

    protected override void DrawZoomLate()
    {
        base.DrawZoomLate();

        tilesetControl.smoothZoom = EditorGUILayout.Toggle("Smooth Zoom", tilesetControl.smoothZoom);
        if (tilesetControl.smoothZoom)
        {
            tilesetControl.smoothZoomMode = (OnlineMapsTileSetControl.OnlineMapsSmoothZoomMode)EditorGUILayout.EnumPopup("Smooth Zoom Mode:", tilesetControl.smoothZoomMode);
        }
    }

    private void OnEnable()
    {
        control = (OnlineMapsControlBase3D)target;
        tilesetControl = (OnlineMapsTileSetControl)control;
        defaultTilesetShader = Shader.Find("Infinity Code/Online Maps/Tileset");
    }

    public override void OnInspectorGUI()
    {
        bool dirty = false;

        OnlineMapsControlBaseEditor.CheckMultipleInstances(control, ref dirty);

        OnlineMaps api = OnlineMapsControlBaseEditor.GetOnlineMaps(control);
        OnlineMapsControlBaseEditor.CheckTarget(api, OnlineMapsTarget.tileset, ref dirty);

        if (tilesetControl.tilesetShader == null) tilesetControl.tilesetShader = defaultTilesetShader;
        if (tilesetControl.markerShader == null) tilesetControl.markerShader = Shader.Find("Transparent/Diffuse");
        if (tilesetControl.drawingShader == null) tilesetControl.drawingShader = Shader.Find("Infinity Code/Online Maps/Tileset DrawingElement");

        CheckCameraDistance(api);

        DrawPropsGUI(ref dirty);

        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
        EditorGUI.BeginChangeCheck();
        tilesetControl.colliderType = (OnlineMapsTileSetControl.OnlineMapsColliderType)EditorGUILayout.EnumPopup("Collider Type", tilesetControl.colliderType);
        if (EditorGUI.EndChangeCheck()) dirty = true;

        if (tilesetControl.colliderType == OnlineMapsTileSetControl.OnlineMapsColliderType.box &&
            tilesetControl.useElevation)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox("Potential problem detected:\nWhen using BoxCollider, can be a problem in interaction with a map with elevation.", MessageType.Warning);

            if (GUILayout.Button("Set Collider Type - Mesh"))
            {
                tilesetControl.colliderType = OnlineMapsTileSetControl.OnlineMapsColliderType.mesh;
                dirty = true;
            }

            EditorGUILayout.EndVertical();
        }

        
        EditorGUI.EndDisabledGroup();

        DrawMarkersGUI(ref dirty);
        DrawMaterialsAndShaders(ref dirty);
        DrawElevationGUI(tilesetControl, ref dirty);
        DrawMoveCameraGUI(api);

        if (dirty)
        {
            EditorUtility.SetDirty(api);
            Repaint();
        }
    }

    private void OnSceneGUI()
    {
        OnlineMaps api = control.GetComponent<OnlineMaps>();
        if (api == null) return;
        Quaternion rotation = api.transform.rotation;
        Vector3[] points = new Vector3[5];
        points[0] = points[4] = api.transform.position;
        points[1] = points[0] + rotation * new Vector3(-api.tilesetSize.x, 0, 0);
        points[2] = points[1] + rotation * new Vector3(0, 0, api.tilesetSize.y);
        points[3] = points[0] + rotation * new Vector3(0, 0, api.tilesetSize.y);
        Handles.DrawSolidRectangleWithOutline(points, new Color(1, 1, 1, 0.3f), Color.black);

        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.black;

        Handles.Label(points[0] + rotation * new Vector3(api.tilesetSize.x / -2, 0, api.tilesetSize.y / 2), "Tileset Map", style);
    }
}