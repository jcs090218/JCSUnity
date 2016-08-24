/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnlineMapsLocationService))]
public class OnlineMapsLocationServiceEditor:Editor
{
    public static OnlineMapsLocationService ls;

    private static GUIStyle _toggleStyle;
    private bool showCreateMarker = true;
    private bool showGPSEmulator = true;
    private bool showUpdatePosition = true;

    private static GUIStyle toggleStyle
    {
        get
        {
            if (_toggleStyle == null)
            {
                _toggleStyle = new GUIStyle(GUI.skin.toggle);
                _toggleStyle.margin.top = 0;
            }
            return _toggleStyle;
        }
    }

    private void OnCreateMarkerGUI()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        bool createMarker = ls.createMarkerInUserPosition;
        if (createMarker)
        {
            EditorGUILayout.BeginHorizontal();
            showCreateMarker = GUILayout.Toggle(showCreateMarker, "", EditorStyles.foldout, GUILayout.ExpandWidth(false),
                GUILayout.Height(16));
        }

        ls.createMarkerInUserPosition = GUILayout.Toggle(ls.createMarkerInUserPosition, "Create Marker", toggleStyle);

        if (createMarker) EditorGUILayout.EndHorizontal();

        if (ls.createMarkerInUserPosition && showCreateMarker)
        {
            ls.markerType = (OnlineMapsLocationServiceMarkerType)EditorGUILayout.Popup("Type", (int)ls.markerType, new[] {"2D", "3D"});

            if (ls.markerType == OnlineMapsLocationServiceMarkerType.threeD)
            {
                ls.marker3DPrefab = EditorGUILayout.ObjectField("Prefab", ls.marker3DPrefab, typeof (GameObject), false) as GameObject;
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                ls.marker2DTexture = EditorGUILayout.ObjectField("Texture", ls.marker2DTexture, typeof (Texture2D), false) as Texture2D;
                if (EditorGUI.EndChangeCheck() && ls.marker2DTexture != null) OnlineMapsEditor.CheckMarkerTextureImporter(ls.marker2DTexture);
                ls.marker2DAlign = (OnlineMapsAlign)EditorGUILayout.EnumPopup("Align", ls.marker2DAlign);
            }

            ls.markerTooltip = EditorGUILayout.TextField("Tooltip", ls.markerTooltip);
            ls.useCompassForMarker = EditorGUILayout.Toggle("Use Compass", ls.useCompassForMarker);
        }

        EditorGUILayout.EndVertical();
    }

    private void OnEnable()
    {
        ls = target as OnlineMapsLocationService;
    }

    private void OnGPSEmulatorGUI()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        bool useGPSEmulator = ls.useGPSEmulator;
        if (useGPSEmulator)
        {
            EditorGUILayout.BeginHorizontal();
            showGPSEmulator = GUILayout.Toggle(showGPSEmulator, "", EditorStyles.foldout, GUILayout.ExpandWidth(false));
        }

        ls.useGPSEmulator = GUILayout.Toggle(ls.useGPSEmulator, "Use GPS Emulator", toggleStyle);

        if (useGPSEmulator) EditorGUILayout.EndHorizontal();

        if (ls.useGPSEmulator && showGPSEmulator)
        {
            ls.emulatorPosition = EditorGUILayout.Vector2Field("Position (Lng/Lat)", ls.emulatorPosition);
            ls.emulatorCompass = EditorGUILayout.FloatField("Compass (0-360)", ls.emulatorCompass);
        }

        EditorGUILayout.EndVertical();
    }

    public override void OnInspectorGUI()
    {
        bool dirty = false;

        EditorGUI.BeginChangeCheck();
        ls.desiredAccuracy = EditorGUILayout.FloatField("Desired Accuracy (meters)", ls.desiredAccuracy);

        OnUpdatePositionGUI();
        OnCreateMarkerGUI();
        OnGPSEmulatorGUI();

        if (EditorGUI.EndChangeCheck()) dirty = true;

        if (dirty) EditorUtility.SetDirty(ls);
    }

    private void OnUpdatePositionGUI()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        bool updatePosition = ls.updatePosition;
        if (updatePosition)
        {
            EditorGUILayout.BeginHorizontal();
            showUpdatePosition = GUILayout.Toggle(showUpdatePosition, "", EditorStyles.foldout, GUILayout.ExpandWidth(false),
                GUILayout.Height(16));
        }

        ls.updatePosition = GUILayout.Toggle(ls.updatePosition, "Update Map Position", toggleStyle);

        if (updatePosition) EditorGUILayout.EndHorizontal();

        if (ls.updatePosition && showUpdatePosition)
        {
            ls.updateDistance = EditorGUILayout.FloatField("Update Distance", ls.updateDistance);
            ls.autoStopUpdateOnInput = EditorGUILayout.Toggle("Auto Stop On Input", ls.autoStopUpdateOnInput);

            bool restoreAfter = ls.restoreAfter != 0;
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            restoreAfter = GUILayout.Toggle(restoreAfter, "", GUILayout.ExpandWidth(false));
            if (EditorGUI.EndChangeCheck()) ls.restoreAfter = restoreAfter ? 10 : 0;
            EditorGUI.BeginDisabledGroup(!restoreAfter);
            ls.restoreAfter = EditorGUILayout.IntField("Restore After (sec)", ls.restoreAfter);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }
}