/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Globalization;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnlineMapsControlBase))]
public class OnlineMapsControlBaseEditor:Editor
{
    public static void CheckMultipleInstances(OnlineMapsControlBase control, ref bool dirty)
    {
        OnlineMapsControlBase[] controls = control.GetComponents<OnlineMapsControlBase>();
        if (controls.Length > 1)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox("Problem detected:\nMultiple instances of controls.\nYou can use only one control.", MessageType.Error);

            int controlIndex = -1;

            for (int i = 0; i < controls.Length; i++)
            {
                if (GUILayout.Button("Use " + controls[i].GetType())) controlIndex = i;
            }

            if (controlIndex != -1)
            {
                OnlineMapsControlBase activeControl = controls[controlIndex];
                foreach (OnlineMapsControlBase c in controls) if (c != activeControl) DestroyImmediate(c);
                dirty = true;
            }

            EditorGUILayout.EndVertical();
        }
    }

    public static void CheckTarget(OnlineMaps api, OnlineMapsTarget target, ref bool dirty)
    {
        if (api == null) return;
        if (api.target == target) return;

        EditorGUILayout.BeginVertical(GUI.skin.box);

        string targetName = Enum.GetName(typeof(OnlineMapsTarget), target);
        targetName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(targetName);
        EditorGUILayout.HelpBox("Problem detected:\nWrong target.\nFor this control target must be " + targetName + "!", MessageType.Error);
        if (GUILayout.Button("Fix Target"))
        {
            api.target = target;
            dirty = true;
        }

        EditorGUILayout.EndVertical();
    }

    public static OnlineMaps GetOnlineMaps(OnlineMapsControlBase control)
    {
        if (control == null) return null;
        OnlineMaps api = control.GetComponent<OnlineMaps>();

        if (api == null)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUILayout.HelpBox("Problem detected:\nCan not find OnlineMaps component.", MessageType.Error);

            if (GUILayout.Button("Add OnlineMaps Component"))
            {
                api = control.gameObject.AddComponent<OnlineMaps>();
                UnityEditorInternal.ComponentUtility.MoveComponentUp(api);
                if (control is OnlineMapsTileSetControl) api.target = OnlineMapsTarget.tileset;
            }

            EditorGUILayout.EndVertical();
        }
        return api;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("Please do not use this Control.\nIt is the base class for other Controls.", OnlineMapsEditor.warningStyle);

        if (GUILayout.Button("Remove"))
        {
            DestroyImmediate(target);
        }
    }
}