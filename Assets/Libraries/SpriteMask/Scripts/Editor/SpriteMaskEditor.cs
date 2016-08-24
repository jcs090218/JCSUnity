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

[CustomEditor(typeof(SpriteMask))]
public class SpriteMaskEditor : Editor
{
    private static string[] shadersNames = new string[] {
        "Default",
        "Diffuse",
        "Specify"
    };
    private int lastSortingParamsGetFrame;
    private string[] sortingLayerNames;
    private int[] sortingLayerUniqueIDs;
    private bool customShader = false;
    private static bool foldoutList = false;
    private static bool foldoutMasking = false;
    private static bool foldoutAdvance = false;
    
    public override void OnInspectorGUI ()
    {
        SpriteMask mask = (SpriteMask)target;
        
        bool isPrefab = PrefabUtility.GetPrefabType (mask) == PrefabType.Prefab;
        if (isPrefab) {
            EditorGUILayout.HelpBox ("Prefab edit unavailable", MessageType.Info);
            return;
        }
        
        if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed") {
            // Apply undo
            mask.SendMessage ("RequestTypeApply", SendMessageOptions.DontRequireReceiver);
        }
        
        GUILayoutOption[] options = new GUILayoutOption[0];
        
        serializedObject.Update ();
        
        SpriteMask.Type currentType = mask.type;
        SpriteMask.Type newType = (SpriteMask.Type)EditorGUILayout.EnumPopup ("Type", currentType, options);
        
        if (GUI.changed) {
            if (currentType != newType) {
                Undo.RecordObject (target, "Type change");
                mask.type = newType;
                mask.SendMessage ("RequestTypeApply", SendMessageOptions.DontRequireReceiver);
                EditorUtility.SetDirty (target);
            }
        }
        
        EditorGUILayout.Space ();
        
        bool currentInverted = mask.inverted;
        bool newInverted = EditorGUILayout.Toggle ("Inverted", currentInverted, options);
        if (newInverted) {
            EditorGUILayout.HelpBox ("'Inverted' is an experimental feature. It may not work properly in some multi level use cases.", MessageType.Info);
        }
        if (GUI.changed) {
            if (currentInverted != newInverted) {
                Undo.RecordObject (target, "Inverted change");
                mask.inverted = newInverted;
                EditorUtility.SetDirty (target);
            }
        }
        
        bool currentShowMaskGraphics = mask.showMaskGraphics;
        bool newShowMaskGraphics = EditorGUILayout.Toggle ("Show Mask Graphics", currentShowMaskGraphics, options);
        if (GUI.changed) {
            if (currentShowMaskGraphics != newShowMaskGraphics) {
                Undo.RecordObject (target, "Show Mask Graphics change");
                mask.showMaskGraphics = newShowMaskGraphics;
                EditorUtility.SetDirty (target);
            }
        }
        
        if (newType != SpriteMask.Type.None) {
            EditorGUILayout.Space ();
            
            Vector2 currentSize = mask.size;
            Vector2 newSize = currentSize;
            
            Vector2 currentPivot = mask.pivot;
            Vector2 newPivot = currentPivot;
            
            Sprite currentSprite = mask.sprite;
            Sprite newSprite = currentSprite;
            
            Texture2D currentTexture = mask.texture;
            Texture2D newTexture = currentTexture;
            
            switch (newType) {
            case SpriteMask.Type.Sprite:
                newSprite = EditorGUILayout.ObjectField ("Sprite", currentSprite, typeof(Sprite), true, options) as Sprite;
                break;
            case SpriteMask.Type.Rectangle:
            case SpriteMask.Type.Texture:
                if (newType == SpriteMask.Type.Texture) {
                    newTexture = EditorGUILayout.ObjectField ("Texture", currentTexture, typeof(Texture2D), true, options) as Texture2D;
                } 
                newSize = EditorGUILayout.Vector2Field ("Size", currentSize, options);
                newPivot = EditorGUILayout.Vector2Field ("Pivot", currentPivot, options);
                break;
            }
            
            
            
            if (GUI.changed) {
                if (currentSize != newSize) {
                    Undo.RecordObject (target, "Size change");
                    mask.size = newSize;
                    EditorUtility.SetDirty (target);
                }
                
                if (currentPivot != newPivot) {
                    Undo.RecordObject (target, "Pivot change");
                    mask.pivot = newPivot;
                    EditorUtility.SetDirty (target);
                }
                
                if (currentSprite != newSprite) {
                    Undo.RecordObject (target, "Sprite change");
                    mask.sprite = newSprite;
                    EditorUtility.SetDirty (target);
                }
                
                if (currentTexture != newTexture) {
                    Undo.RecordObject (target, "Texture change");
                    mask.texture = newTexture;
                    EditorUtility.SetDirty (target);
                }
            }
            
            if (newType == SpriteMask.Type.Rectangle || newType == SpriteMask.Type.Texture) {
                EditorGUILayout.Space ();
                
                Renderer r = mask.GetComponent <Renderer> ();
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
        }
        
        EditorGUILayout.Space ();
        
        foldoutMasking = EditorGUILayout.Foldout (foldoutMasking, "Masked objects (outside this mask hierarchy)");
        if (foldoutMasking) {
            EditorGUILayout.HelpBox ("Masked objects are objects that are outside this mask and they should be masked by this mask. Each object (Transform) in the list and all their childs will masked out with this mask.", MessageType.Info);
            
            EditorGUI.indentLevel++;
            Transform trans = EditorGUILayout.ObjectField ("Drop here >>>", null, typeof(Transform), true, options) as Transform;
            if (trans != null) {
                if (!mask.maskedObjects.Contains (trans)) {
                    Debug.Log ("Adding: " + trans.name);
                    mask.maskedObjects.Add (trans);
                    mask.SendMessage ("Update", SendMessageOptions.DontRequireReceiver);
                } else {
                    Debug.LogWarning ("Exist: " + trans.name);
                }
            }
            
            foldoutList = EditorGUILayout.Foldout (foldoutList, "All items: " + mask.maskedObjects.Count.ToString ());
            if (foldoutList) {
                EditorGUI.indentLevel++;
                int deleteIndex = -1;
                for (int i=0; i<mask.maskedObjects.Count; i++) {
                    EditorGUILayout.BeginHorizontal (options);
                    GUI.enabled = false;
                    EditorGUILayout.ObjectField (string.Concat ("[", i.ToString (), "] >>>"), mask.maskedObjects [i], typeof(Transform), true);
                    GUI.enabled = true;
                    if (GUILayout.Button ("Delete", options)) {
                        deleteIndex = i;
                    }
                    EditorGUILayout.EndHorizontal ();
                } 
                EditorGUI.indentLevel--;
                
                if (deleteIndex != -1) {
                    SpriteMaskingComponent smc = mask.maskedObjects [deleteIndex].GetComponent<SpriteMaskingComponent> ();
                    if (smc != null) {
                        Debug.Log ("Deleting: " + smc);
                        
                        if (Application.isPlaying) {
                            Destroy (smc);
                        } else {
                            DestroyImmediate (smc);
                        }
                    }
                    mask.maskedObjects.RemoveAt (deleteIndex);
                }
            }
            EditorGUI.indentLevel--;
            
            EditorGUILayout.Separator ();
        }
        
        foldoutAdvance = EditorGUILayout.Foldout (foldoutAdvance, "Advance");
        if (foldoutAdvance) {
            EditorGUILayout.HelpBox ("Forces to use this object default material on all his childs. This is useful when moving sprite between different SpriteMask objects.", MessageType.Info);
            bool currentForceDefaultMaterialOnChilds = mask.forceDefaultMaterialOnChilds;
            bool newForceDefaultMaterialOnChilds = EditorGUILayout.Toggle ("Force default material on childs", currentForceDefaultMaterialOnChilds, options);
            if (GUI.changed && currentForceDefaultMaterialOnChilds != newForceDefaultMaterialOnChilds) {
                Undo.RecordObject (target, "Force Materal Set To Default On Sprites change");
                mask.forceDefaultMaterialOnChilds = newForceDefaultMaterialOnChilds;
                EditorUtility.SetDirty (target);
            }
            
            EditorGUILayout.Space ();
            
            Shader shader = mask.spritesShader;
            int idx = 0;
            if (customShader) {
                idx = 2;
            } else {
                if (shader != null) {
                    switch (shader.name) {
                    case SpriteMask.SHADER_SPRITE_DEFAULT:
                        idx = 0;
                        break;
                    case SpriteMask.SHADER_SPRITE_DIFFUSE:
                        idx = 1;
                        break;
                    default:
                        idx = 2;
                        customShader = true;
                        break;
                    }
                }
            }
            
            int newIdx = EditorGUILayout.Popup ("Sprites shader", idx, shadersNames, options);
            
            Shader newShader = null;
            if (newIdx == 2) {
                customShader = true;
                EditorGUI.indentLevel++;
                newShader = EditorGUILayout.ObjectField ("Shader", shader, typeof(Shader), true, options) as Shader;
                EditorGUI.indentLevel--;
            } else {
                customShader = false;
            }
            
            if (newIdx != idx) {
                switch (newIdx) {
                case 0:
                    newShader = Shader.Find (SpriteMask.SHADER_SPRITE_DEFAULT);
                    break;
                case 1:
                    newShader = Shader.Find (SpriteMask.SHADER_SPRITE_DIFFUSE);
                    break;
                }
            }

            if (newShader != null && shader != newShader) {
                Undo.RecordObject (target, "Shader change");
                mask.spritesShader = newShader;
                EditorUtility.SetDirty (target);
            }
        }
        
        string msg = string.Concat ("Instance ID: ", mask.GetInstanceID (), 
                                    "\nStencil ID: ", mask.stencilId, 
                                    " (level=", mask.level, 
                                    ", id=" + mask.maskIdPerLevel,
                                    ")");
        EditorGUILayout.HelpBox (msg, MessageType.None);
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