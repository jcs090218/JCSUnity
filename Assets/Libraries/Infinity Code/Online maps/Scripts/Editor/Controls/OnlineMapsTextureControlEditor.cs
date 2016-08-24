/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnlineMapsTextureControl))]
public class OnlineMapsTextureControlEditor : Editor
{
    protected OnlineMapsControlBase3D control;
    private bool showMarkers;

    private void DrawAllowsGUI()
    {
        control.allowUserControl = EditorGUILayout.Toggle("Allow User Control", control.allowUserControl);
        control.allowAddMarkerByM = EditorGUILayout.Toggle("Allow Add 2D marker by M", control.allowAddMarkerByM);
        control.allowAddMarker3DByN = EditorGUILayout.Toggle("Allow Add 3D marker by N", control.allowAddMarker3DByN);
    }

    private void DrawCameraControlGUI()
    {
        bool allowCameraControl = control.allowCameraControl;
        if (allowCameraControl) EditorGUILayout.BeginVertical(GUI.skin.box);

        control.allowCameraControl = EditorGUILayout.Toggle("Allow Camera Control", control.allowCameraControl);

        if (control.allowCameraControl)
        {
            control.cameraDistance = EditorGUILayout.FloatField("Camera distance", control.cameraDistance);
            control.cameraRotation = EditorGUILayout.Vector2Field("Camera rotation", control.cameraRotation);
            control.cameraSpeed = EditorGUILayout.Vector2Field("Camera rotation speed", control.cameraSpeed);
        }

        if (allowCameraControl) EditorGUILayout.EndVertical();
    }

    public virtual void DrawMarker2DPropsGUI()
    {
        EditorGUI.BeginChangeCheck();
        control.marker2DMode = (OnlineMapsMarker2DMode) EditorGUILayout.EnumPopup("Marker 2D Mode: ", control.marker2DMode);
        if (control.marker2DMode == OnlineMapsMarker2DMode.billboard)
        {
            control.marker2DSize = EditorGUILayout.FloatField("Marker 2D size: ", control.marker2DSize);
            if (control.marker2DSize < 1) control.marker2DSize = 1;
        }
        if (EditorGUI.EndChangeCheck() && EditorApplication.isPlaying)
        {
            control.Clear2DMarkerInstances();
            OnlineMaps.instance.Redraw();
        }
    }

    private void DrawMarkerGUI(int i, ref int index, ref bool hasDeleted, OnlineMaps map, ref bool dirty)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        OnlineMapsMarker3D marker = control.markers3D[i];
        GUILayout.Label("Marker " + index);
        Vector2 oldPosition = marker.position;

        EditorGUI.BeginChangeCheck();

        marker.allowDefaultMarkerEvents = control.allowDefaultMarkerEvents;

        marker.position.y = EditorGUILayout.FloatField("Latitude: ", marker.position.y);
        marker.position.x = EditorGUILayout.FloatField("Longitude: ", marker.position.x);

        float min = marker.range.min;
        float max = marker.range.max;
        EditorGUILayout.MinMaxSlider(new GUIContent(string.Format("Zooms ({0}-{1}): ", marker.range.min, marker.range.max)), ref min, ref max, 3, 20);
        if (marker.range.Update(Mathf.RoundToInt(min), Mathf.RoundToInt(max)) && EditorApplication.isPlaying)
            marker.Update(map.topLeftPosition, map.bottomRightPosition, map.zoom);

        if (Application.isPlaying && marker.position != oldPosition) marker.Update(map.topLeftPosition, map.bottomRightPosition, map.zoom);

        marker.label = EditorGUILayout.TextField("Label: ", marker.label);
        marker.scale = EditorGUILayout.FloatField("Scale: ", marker.scale);
        GameObject prefab = marker.prefab;
        marker.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab: ", marker.prefab, typeof(GameObject), false);

        if (Application.isPlaying && marker.prefab != prefab) marker.Reinit(map.topLeftPosition, map.bottomRightPosition, map.zoom);

        if (EditorGUI.EndChangeCheck()) dirty = true;

        if (GUILayout.Button("Remove"))
        {
            control.markers3D[i] = null;
            hasDeleted = true;
            if (Application.isPlaying) Destroy(marker.instance);
        }
        index++;
        EditorGUILayout.EndVertical();
    }

    protected void DrawMarkersGUI(ref bool dirty)
    {
        if (control.markers3D == null)
        {
            control.markers3D = new OnlineMapsMarker3D[0];
            dirty = true;
        }

        EditorGUILayout.BeginVertical(GUI.skin.box);
        showMarkers = OnlineMapsEditor.Foldout(showMarkers, "3D markers");

        if (showMarkers)
        {
            EditorGUI.BeginChangeCheck();
            control.marker3DScale = EditorGUILayout.FloatField("Marker3D Scale: ", control.marker3DScale);
            control.allowDefaultMarkerEvents = EditorGUILayout.Toggle("Allow Default Marker Events: ", control.allowDefaultMarkerEvents);
            if (EditorGUI.EndChangeCheck()) dirty = true;

            int index = 1;
            bool hasDeleted = false;

            OnlineMaps map = control.GetComponent<OnlineMaps>();

            for (int i = 0; i < control.markers3D.Length; i++) DrawMarkerGUI(i, ref index, ref hasDeleted, map, ref dirty);

            if (hasDeleted)
            {
                List<OnlineMapsMarker3D> markers = control.markers3D.ToList();
                markers.RemoveAll(m => m == null);
                control.markers3D = markers.ToArray();
                if (Application.isPlaying) OnlineMaps.instance.Redraw();
                dirty = true;
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Add marker"))
            {
                if (!Application.isPlaying)
                {
                    OnlineMapsMarker3D marker = new OnlineMapsMarker3D
                    {
                        position = control.GetComponent<OnlineMaps>().position,
                        scale = control.marker3DScale
                    };
                    List<OnlineMapsMarker3D> markers = new List<OnlineMapsMarker3D>(control.markers3D) { marker };
                    control.markers3D = markers.ToArray();
                }
                else
                {
                    GameObject prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    control.AddMarker3D(OnlineMaps.instance.position, prefab);
                    DestroyImmediate(prefab);
                }
                EditorUtility.SetDirty(control);
            }
        }

        EditorGUILayout.EndVertical();
    }

    protected void DrawPropsGUI(ref bool dirty)
    {
        EditorGUI.BeginChangeCheck();

        DrawAllowsGUI();
        DrawZoomGUI();
        DrawCameraControlGUI();

        control.activeCamera = (Camera) EditorGUILayout.ObjectField("Camera:", control.activeCamera, typeof (Camera), true);
        DrawMarker2DPropsGUI();

        if (EditorGUI.EndChangeCheck()) dirty = true;
    }

    private void DrawZoomGUI()
    {
        bool allowZoom = control.allowZoom;
        if (allowZoom) EditorGUILayout.BeginVertical(GUI.skin.box);
        control.allowZoom = EditorGUILayout.Toggle("Allow Zoom", control.allowZoom);
        if (control.allowZoom)
        {
            control.zoomInOnDoubleClick = EditorGUILayout.Toggle("Zoom In On Double Click", control.zoomInOnDoubleClick);
            control.invertTouchZoom = EditorGUILayout.Toggle("Invert Touch Zoom", control.invertTouchZoom);
            DrawZoomLate();
        }
        if (allowZoom) EditorGUILayout.EndVertical();
    }

    protected virtual void DrawZoomLate()
    {
        
    }

    private void OnEnable()
    {
        control = (OnlineMapsControlBase3D) target;
    }

    public override void OnInspectorGUI()
    {
        bool dirty = false;

        OnlineMaps api = OnlineMapsControlBaseEditor.GetOnlineMaps(control);
        OnlineMapsControlBaseEditor.CheckTarget(api, OnlineMapsTarget.texture, ref dirty);

        OnlineMapsControlBaseEditor.CheckMultipleInstances(control, ref dirty);

        DrawPropsGUI(ref dirty);
        DrawMarkersGUI(ref dirty);

        if (dirty)
        {
            EditorUtility.SetDirty(api);
            Repaint();
        }
    }
}
