/**
 * $File: JCS_ButtonSelection.cs $
 * $Date: 2017-10-07 14:41:08 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// One of the button selection for button selection group.
    /// 
    /// NOTE(jenchieh): this must use with JCS_ButtonSelectionGroup
    /// class or else is useless.
    /// </summary>
    public class JCS_ButtonSelection
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Check Variables (JCS_ButtonSelection) **")]

        private bool mActive = false;

        [Header("** Runtime Variables (JCS_ButtonSelection) **")]

        [Tooltip("Button for selection group to handle.")]
        [SerializeField]
        private JCS_Button mButton = null;

        [Tooltip("List of effect when on this selection.")]
        [SerializeField]
        private JCS_UnityObject[] mEffects = null;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public JCS_Button GetButton() { return this.mButton; }
        public bool Active
        {
            get { return this.mActive; }
            set
            {
                this.mActive = value;
                DoActive();
            }
        }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Do active and deactive.
        /// </summary>
        private void DoActive()
        {
            if (mActive)
            {
                // Do stuff when active..
            }
            else
            {
                // Do stuff when deactive..
            }

            for (int index = 0;
                   index < mEffects.Length;
                   ++index)
            {
                JCS_UnityObject effect = mEffects[index];

                if (effect != null)
                    effect.LocalEnabled = mActive;
            }
        }

    }
}
