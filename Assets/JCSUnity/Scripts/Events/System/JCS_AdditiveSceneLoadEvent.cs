/**
 * $File: JCS_AddictiveSceneLoadEvent.cs $
 * $Date: 2025-08-27 05:10:36 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2025 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Event that loads a list of additive scenes.
    /// 
    /// This is used for working with multiple scenes and use
    /// thoses scenes as an overlays.
    /// </summary>
    public class JCS_AdditiveSceneLoadEvent : MonoBehaviour
    {
        /* Variables */

        [Separator("Initialize Variables (JCS_AdditiveSceneLoadEvent)")]

        [Tooltip("A list of addictive scene to load.")]
        [Scene]
        public List<string> sceneNames = null;

        [Tooltip("Load scene asynchronously.")]
        [SerializeField]
        private bool mUseAsync = false;

        /* Setter & Getter */

        public bool useAsync { get { return mUseAsync; } set { mUseAsync = value; } }

        /* Functions */

        private void Awake()
        {
            foreach (string sceneName in sceneNames)
            {
                if (mUseAsync)
                    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                else
                    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }
    }
}
