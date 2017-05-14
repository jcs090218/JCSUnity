/**
 * $File: JCS_ToggleButton.cs $
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

    public delegate void ToggleFunc();

    /// <summary>
    /// Button that have two callback can toggle each other.
    /// </summary>
    public class JCS_ToggleButton
        : JCS_Button
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        private ToggleFunc mAcitveFunc = null;
        private ToggleFunc mDeactiveFunc = null;

        [Header("** Check Variables (JCS_ToggleButton) **")]
        [SerializeField] private bool mActive = false;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetActiveFunc(ToggleFunc func) { this.mAcitveFunc = func; }
        public void SetDeactiveFunc(ToggleFunc func) { this.mDeactiveFunc = func; }

        //========================================
        //      Unity's function
        //------------------------------


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        public override void JCS_ButtonClick()
        {
            // do the toggle function.
            if (mActive)
                // active do deactive.
                DoDeactiveFunc();
            else
                // deactive do active.
                DoActiveFunc();

            // toggle the boolean
            mActive = !mActive;

            // system func
            base.JCS_ButtonClick();
        }

        public void DoActiveFunc()
        {
            if (mAcitveFunc == null)
            {
                JCS_Debug.LogError(
                    this, "U have not set the ACTIVE function ptr...");

                return;
            }

            // do the action.
            mAcitveFunc.Invoke();
        }

        public void DoDeactiveFunc()
        {
            if (mDeactiveFunc == null)
            {
                JCS_Debug.LogError(
                    this, "U have not set the DEACTIVE function ptr...");

                return;
            }

            // do the action.
            mDeactiveFunc.Invoke();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
