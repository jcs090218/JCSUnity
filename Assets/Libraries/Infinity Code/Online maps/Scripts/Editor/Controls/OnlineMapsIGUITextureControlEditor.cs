/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnlineMapsIGUITextureControl))]
public class OnlineMapsIGUITextureControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool dirty = false;
        OnlineMapsControlBase control = target as OnlineMapsControlBase;
        OnlineMapsControlBaseEditor.CheckMultipleInstances(control, ref dirty);

        OnlineMaps api = OnlineMapsControlBaseEditor.GetOnlineMaps(control);
        OnlineMapsControlBaseEditor.CheckTarget(api, OnlineMapsTarget.texture, ref dirty);

#if !IGUI
        if (GUILayout.Button("Enable iGUI")) OnlineMapsEditor.AddCompilerDirective("IGUI");
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