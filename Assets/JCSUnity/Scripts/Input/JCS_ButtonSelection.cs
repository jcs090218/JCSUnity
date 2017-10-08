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
    public delegate void SelectionEnable();
    public delegate void SelectionDisable();
    public delegate void SelectionActive(bool act);

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
        public SelectionEnable selectionEnable = null;
        public SelectionDisable selectionDisable = null;
        public SelectionActive selectionActive = null;

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
        public JCS_Button Button { get { return this.mButton; } set { this.mButton = value; } }
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
        private void Awake()
        {
            this.selectionActive = SelectionActive;
            this.selectionEnable = SelectionEnable;
            this.selectionDisable = SelectionDisable;
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /* Default Function Pointers. */
        private void SelectionEnable() { }
        private void SelectionDisable() { }
        private void SelectionActive(bool act) { }

        /// <summary>
        /// Do active and deactive.
        /// </summary>
        private void DoActive()
        {
            this.selectionActive.Invoke(this.mActive);

            if (this.mActive)
            {
                // Do stuff when active..
                selectionEnable.Invoke();
            }
            else
            {
                // Do stuff when deactive..
                selectionDisable.Invoke();
            }

            ActiveEffects(this.mActive);
        }

        /// <summary>
        /// Active the effects?
        /// </summary>
        /// <param name="act">
        /// true: active effects.
        /// false: deactive effects.
        /// </param>
        private void ActiveEffects(bool act)
        {
            for (int index = 0;
                   index < mEffects.Length;
                   ++index)
            {
                JCS_UnityObject effect = mEffects[index];

                if (effect != null)
                    effect.LocalEnabled = act;
            }
        }
        
    }
}
