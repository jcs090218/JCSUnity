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
using UnityEngine.Events;


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
        public SelectionEnable selectionEnable = SelectionEnable;
        public SelectionDisable selectionDisable = SelectionDisable;
        public SelectionActive selectionActive = SelectionActive;

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        public JCS_ButtonSelectionGroup mButtonSelectionGroup = null;

        [Header("** Check Variables (JCS_ButtonSelection) **")]

        [Tooltip("Is this selection got active?")]
        [SerializeField]
        private bool mActive = false;


        [Header("** Initialize Variables (JCS_ButtonSelection) **")]

        [Tooltip("Deactive this button on Awake time?")]
        [SerializeField]
        private bool mDeactiveAtAwake = true;


        [Header("** Runtime Variables (JCS_ButtonSelection) **")]

        [Tooltip("Skip this selection?")]
        [SerializeField]
        private bool mSkip = false;

        [Tooltip("Button for selection group to handle.")]
        [SerializeField]
        private JCS_Button mButton = null;

        [Tooltip("Events when you enter this selection.")]
        [SerializeField]
        private UnityEvent mSelectedEvent = null;

        [Tooltip("List of effect when on this selection.")]
        [SerializeField]
        private JCS_UnityObject[] mEffects = null;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public bool DeactiveAtAwake { get { return this.mDeactiveAtAwake; } set { this.mDeactiveAtAwake = value; } }
        public JCS_Button Button { get { return this.mButton; } set { this.mButton = value; } }
        public UnityEvent SelectedEvent { get { return this.mSelectedEvent; } }
        public bool Active
        {
            get { return this.mActive; }
            set
            {
                this.mActive = value;
                DoActive();
            }
        }
        public JCS_ButtonSelectionGroup ButtonSelectionGroup { get { return this.mButtonSelectionGroup; } set { this.mButtonSelectionGroup = value; } }
        public bool Skip { get { return this.mSkip; } set { this.mSkip = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            if (mDeactiveAtAwake)
            {
                // Deactive every at start
                this.Active = false;
            }

            // let the button know this is going to be control in the group.
            if (mButton != null)
                mButton.ButtonSelection = this;
        }


        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        /// <summary>
        /// Do stuff if this selection been checked.
        /// </summary>
        public void DoSelection()
        {
            if (mButton != null)
                mButton.JCS_ButtonClick();

            if (mSelectedEvent != null)
                mSelectedEvent.Invoke();
        }

        /// <summary>
        /// Check if this selection get selected.
        /// </summary>
        /// <returns>
        /// true: selected.
        /// false: vice versa.
        /// </returns>
        public bool IsSelected()
        {
            return this.mActive;
        }

        /// <summary>
        /// Make this selection selected.
        /// </summary>
        public void MakeSelect()
        {
            mButtonSelectionGroup.SelectSelection(this);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /* Default Function Pointers. */
        private static void SelectionEnable() { }
        private static void SelectionDisable() { }
        private static void SelectionActive(bool act) { }

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
                {
                    effect.UpdateUnityData();
                    effect.LocalEnabled = act;
                }
            }
        }

    }
}
