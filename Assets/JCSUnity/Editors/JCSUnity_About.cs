#if (UNITY_EDITOR)
/**
 * $File: JCSUnity_About.cs $
 * $Date: 2017-01-22 05:03:30 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using UnityEditor;


namespace JCSUnity
{

    /// <summary>
    /// About info window.
    /// </summary>
    public class JCSUnity_About
        : EditorWindow      // TODO(jenchieh): change to normal window.
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private static int WINDOW_WIDTH = 400;
        private static int WINDOW_HEIGHT = 200;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void OnGUI()
        {
            GUILayout.Label("About JCSUnity", EditorStyles.boldLabel);

            // TODO(jenchieh): info should be out source.
            GUILayout.Label("Author: Jen-Chieh Shen");
            GUILayout.Label("Email: jayces090218@gmail.com");
            GUILayout.Label("Current Version: 1.3.9");

            // GUI.Button that is drawn in the Label style.
            if (GUILayout.Button("Source: https://github.com/jcs090218/JCSUnity_Framework", "Label"))
            {
                string url = "https://github.com/jcs090218/JCSUnity_Framework";
                Application.OpenURL(url);
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// About JCSUnity.
        /// </summary>
        [MenuItem("JCSUnity/About", false, 100)]
        private static void AboutJCSUnity()
        {
            JCSUnity_About window = (JCSUnity_About)GetWindow(typeof(JCSUnity_About));
            window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            window.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
            window.Show();
        }
    }
}

#endif
