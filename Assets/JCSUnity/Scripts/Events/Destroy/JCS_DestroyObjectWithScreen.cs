/**
 * $File: JCS_DestroyObjectWithScreen.cs $
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
    /// Destroy the gameobject when the object is no longer render 
    /// on the screen.
    /// </summary>
    public class JCS_DestroyObjectWithScreen
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_DestroyObjectWithScreen) **")]

        [Tooltip("Trigger event flag.")]
        [SerializeField]
        private bool mDestroyWhenOutOfScreen = true;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool DestroyWhenOutOfScreen { get { return this.mDestroyWhenOutOfScreen; } set { this.mDestroyWhenOutOfScreen = value; } }

        //========================================
        //      Unity's function
        //------------------------------

        private void OnBecameInvisible()
        {
            if (!DestroyWhenOutOfScreen)
                return;

            Destroy(this.gameObject);
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
