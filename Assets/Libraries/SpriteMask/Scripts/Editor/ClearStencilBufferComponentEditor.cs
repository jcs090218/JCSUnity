//----------------------------------------------
//                 SpriteMask
//          Copyright © 2015 TrueSoft
//             support@truesoft.pl
//----------------------------------------------
using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

[CustomEditor(typeof(ClearStencilBufferComponent))]
public class ClearStencilBufferComponentEditor : Editor
{
    private int lastSortingParamsGetFrame;
    private string[] sortingLayerNames;
    private int[] sortingLayerUniqueIDs;
    
    public override void OnInspectorGUI ()
    {
        ClearStencilBufferComponent csbc = (ClearStencilBufferComponent)target;
        
        bool isPrefab = PrefabUtility.GetPrefabType (csbc) == PrefabType.Prefab;
        if (isPrefab) {
            EditorGUILayout.HelpBox ("Prefab edit unavailable", MessageType.Info);
            return;
        }
        
        GUILayoutOption[] options = new GUILayoutOption[0];
        
        serializedObject.Update ();

        string msg = string.Concat ("To get SpriteMask work with UI Mask, Stencil buffer must be cleared before Unity UI is rendered. To do that, this object use 1 drawcall to fill the whole Stencil buffer with 0 value. " +
            "It is very important to set this object sortingOrder and sortingLayer values between unity UI rendering and SpriteMask rendering, for example:\n\n" +
            "   [Default, 10001+]   -> UI\n" +
            "   [Default, 10000]     -> This object (ClearStencilBufferComponent)\n" +
            "   [Default, 0 - 9999] -> SprieMask");
        EditorGUILayout.HelpBox (msg, MessageType.None);

        EditorGUILayout.Space ();
        
        msg = string.Concat ("Specify size of region that should be cleared.");
        EditorGUILayout.HelpBox (msg, MessageType.None);

        Vector2 currentSize = csbc.size;
        Vector2 newSize = currentSize;
        
        Vector2 currentPivot = csbc.pivot;
        Vector2 newPivot = currentPivot;
        newSize = EditorGUILayout.Vector2Field ("Size", currentSize, options);
        newPivot = EditorGUILayout.Vector2Field ("Pivot", currentPivot, options);
        
        if (GUI.changed) {
            if (currentSize != newSize) {
                Undo.RecordObject (target, "Size change");
                csbc.size = newSize;
                EditorUtility.SetDirty (target);
            }
            
            if (currentPivot != newPivot) {
                Undo.RecordObject (target, "Pivot change");
                csbc.pivot = newPivot;
                EditorUtility.SetDirty (target);
            }
        }
        
        EditorGUILayout.Space ();
        
        Renderer r = csbc.GetComponent <Renderer> ();
        if (r != null) {
            maybeGetSortingParams ();
            
            int selectedIdx = -1;
            string name = r.sortingLayerName;
            
            for (int i = 0; i < sortingLayerNames.Length; i++) {
                if (name.Equals (sortingLayerNames [i])) {
                    selectedIdx = i;
                }
            }
            
            if (selectedIdx == -1) {
                for (int i = 0; i < sortingLayerUniqueIDs.Length; i++) {
                    if (sortingLayerUniqueIDs [i] == 0) {
                        selectedIdx = i;
                    }
                }
            }
            
            int sortingLayerIdx = EditorGUILayout.Popup ("Sorting Layer", selectedIdx, sortingLayerNames);
            if (selectedIdx != sortingLayerIdx) {
                Undo.RecordObject (r, "Sorting Layer change");
                r.sortingLayerName = sortingLayerNames [sortingLayerIdx];
                EditorUtility.SetDirty (r);
            }
            
            int sortingOrder = EditorGUILayout.IntField ("Order in Layer", r.sortingOrder, options);
            if (sortingOrder != r.sortingOrder) {
                Undo.RecordObject (r, "Order in Layer");
                r.sortingOrder = sortingOrder;
                EditorUtility.SetDirty (r);
            }
        }
    }
    
    private void maybeGetSortingParams ()
    {
        if (Time.frameCount != lastSortingParamsGetFrame || sortingLayerNames == null || sortingLayerUniqueIDs == null) {
            lastSortingParamsGetFrame = Time.frameCount;
            
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty ("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            sortingLayerNames = (string[])sortingLayersProperty.GetValue (null, new object[0]);
            
            PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty ("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
            sortingLayerUniqueIDs = (int[])sortingLayerUniqueIDsProperty.GetValue (null, new object[0]);
        }
    }
}