/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (OnlineMapsGUITextureControl))]
public class OnlineMapsGUITextureControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool dirty = false;

        OnlineMapsControlBase control = target as OnlineMapsControlBase;
        OnlineMapsControlBaseEditor.CheckMultipleInstances(control, ref dirty);

        OnlineMaps api = OnlineMapsControlBaseEditor.GetOnlineMaps(control);
        OnlineMapsControlBaseEditor.CheckTarget(api, OnlineMapsTarget.texture, ref dirty);

        base.OnInspectorGUI();

        if (dirty)
        {
            EditorUtility.SetDirty(api);
            Repaint();
        }
    } 
}