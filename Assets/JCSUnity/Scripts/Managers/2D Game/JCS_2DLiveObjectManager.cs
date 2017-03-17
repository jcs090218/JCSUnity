/**
 * $File: JCS_2DLiveObjectManager.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Handle all the 2d live object in the scene.
    /// </summary>
    public class JCS_2DLiveObjectManager
        : MonoBehaviour
    {

        //----------------------
        // Public Variables
        public static JCS_2DLiveObjectManager instance = null;

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = this;
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Destroy all the live object in the scene.
        /// 全圖殺怪!
        /// </summary>
        public void DestroyAllLiveObject()
        {
            // Destroy all the live object in the scene.
            JCS_2DLiveObject[] jcsLiveObjects = Resources.FindObjectsOfTypeAll<JCS_2DLiveObject>();

            foreach (JCS_2DLiveObject lo in jcsLiveObjects)
            {
                // NOTE(JenChieh): kill the object that are clone!
                // or else it will effect the prefab object...
                if (lo.gameObject.name.Contains("(Clone)"))
                    lo.Die();
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
