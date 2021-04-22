#if UNITY_EDITOR
/**
 * $File: EditorUtil.cs $
 * $Date: 2021-04-18 16:35:05 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEditor;
using UnityEngine;

namespace PackageExporter
{
    public delegate void EmptyFunction();

    /// <summary>
    /// Utility for Editor Layout
    /// </summary>
    public static class EditorUtil
    {
        private const int INDENT_LEVEL = 0;  // default to 1

        public static string FormKey(string name)
        {
            return PackageExporterWindow.NAME + "." + name;
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
            EditorGUI.indentLevel += INDENT_LEVEL;
            func.Invoke();
            EditorGUI.indentLevel -= INDENT_LEVEL;
        }
    }
}
#endif