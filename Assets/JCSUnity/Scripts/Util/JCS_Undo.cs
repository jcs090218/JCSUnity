#if UNITY_EDITOR
/**
 * $File: JCS_Undo.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2026 by Shen, Jen-Chieh $
 */
using System;
using UnityEditor;

namespace JCSUnity
{
    /// <summary>
    /// Undo and redo utilities.
    /// </summary>
    public static class JCS_Undo
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Like `Undo.RegisterCompleteObjectUndo` but a wrapper.
        /// </summary>
        public static void RegisterComplete(
            UnityEngine.Object objectToUndo, string name, Action action)
        {
            Undo.RegisterCompleteObjectUndo(objectToUndo, name);

            action?.Invoke();

            EditorUtility.SetDirty(objectToUndo);
        }
    }
}
#endif
