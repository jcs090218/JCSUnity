/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnlineMapsNGUITextureControl))]
public class OnlineMapsNGUITextureControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool dirty = false;

        OnlineMapsControlBase control = target as OnlineMapsControlBase;
        OnlineMapsControlBaseEditor.CheckMultipleInstances(control, ref dirty);

        OnlineMaps api = OnlineMapsControlBaseEditor.GetOnlineMaps(control);
        OnlineMapsControlBaseEditor.CheckTarget(api, OnlineMapsTarget.texture, ref dirty);

#if !NGUI
        if (GUILayout.Button("Enable NGUI")) OnlineMapsEditor.AddCompilerDirective("NGUI");
#else
        base.OnInspectorGUI();
#endif

        if (dirty)
        {
            EditorUtility.SetDirty(api);
            Repaint();
        }
    }
}