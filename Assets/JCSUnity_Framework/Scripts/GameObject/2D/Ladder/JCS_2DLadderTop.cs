/**
 * $File: JCS_2DLadderTop.cs $
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

    [RequireComponent(typeof(BoxCollider))]
    public class JCS_2DLadderTop
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private BoxCollider mBoxCollider = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public BoxCollider GetBoxCollider() { return this.mBoxCollider; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mBoxCollider = this.GetComponent<BoxCollider>();
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
