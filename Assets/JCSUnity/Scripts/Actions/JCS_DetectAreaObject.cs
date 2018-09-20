/**
 * $File: JCS_DetectAreaObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{
    /// <summary>
    /// Tag for "JCS_DetectAreaAction"
    /// </summary>
    [RequireComponent(typeof(JCS_2DLiveObject))]
    public class JCS_DetectAreaObject
        : MonoBehaviour
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Check Variables **")]
        [SerializeField] private JCS_2DLiveObject mLiveObject = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_2DLiveObject GetLiveObject() { return this.mLiveObject; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mLiveObject = this.GetComponent<JCS_2DLiveObject>();
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


    }
}
