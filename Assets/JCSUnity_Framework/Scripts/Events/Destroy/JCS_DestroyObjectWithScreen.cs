/**
 * $File: JCS_DestroyObjectWithScreen.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    public class JCS_DestroyObjectWithScreen
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** JCS_DestroyableObject Runtime Variables **")]
        [SerializeField] private bool mDestroyWhenOutOfScreen = true;

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
