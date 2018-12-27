/**
 * $File: JCS_CounterEvent.cs $
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
    public delegate void VoidEvent();

    /// <summary>
    /// Event handle operate the task after a certain time.
    /// </summary>
    public class JCS_CounterEvent
        : JCS_Event
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //[Header("** Initialize Varialbes (JCS_CounterEvent) **")]

        //[SerializeField]
        //private float mDelayTime = 0;

        //private float mDelayTimer = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Update()
        {

        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Event to do after the certain time.
        /// </summary>
        public void Event()
        {

        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
