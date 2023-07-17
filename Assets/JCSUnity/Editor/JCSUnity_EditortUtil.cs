#if UNITY_EDITOR
/**
 * $File: JCSUnity_EditortUtil.cs $
 * $Date: 2021-04-18 16:35:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEditor;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Utility for Editor Layout
    /// </summary>
    public static class JCSUnity_EditortUtil
    {
        /* Variables */

        /* Setter/Getter */

        /* Functions */

        public static string FormKey(string name)
        {
            return JCSUnity_EditorWindow.NAME + "." + name;
        }

        public static void CreateGroup(EmptyFunction func, bool flexibleSpace = false)
        {
            BeginHorizontal(() =>
            {
                BeginVertical(() =>
                {
                    Indent(func);
                });
            },
            flexibleSpace);
        }

        public static void BeginHorizontal(EmptyFunction func, bool flexibleSpace = false)
        {
            GUILayout.BeginHorizontal();
            if (flexibleSpace) GUILayout.FlexibleSpace();
            func.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void BeginVertical(EmptyFunction func)
        {
            GUILayout.BeginVertical("box");
            func.Invoke();
            GUILayout.EndVertical();
        }

        public static void Indent(EmptyFunction func)
        {
            EditorGUI.indentLevel++;
            func.Invoke();
            EditorGUI.indentLevel--;
        }
    }
}
#endif
