/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnlineMapsDFGUITextureControl))]
public class OnlineMapsDFGUITextureControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool dirty = false;

        OnlineMapsControlBase control = target as OnlineMapsControlBase;
        OnlineMapsControlBaseEditor.CheckMultipleInstances(control, ref dirty);

        OnlineMaps api = OnlineMapsControlBaseEditor.GetOnlineMaps(control);
        OnlineMapsControlBaseEditor.CheckTarget(api, OnlineMapsTarget.texture, ref dirty);

#if !DFGUI
        if (GUILayout.Button("Enable DFGUI")) OnlineMapsEditor.AddCompilerDirective("DFGUI");
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