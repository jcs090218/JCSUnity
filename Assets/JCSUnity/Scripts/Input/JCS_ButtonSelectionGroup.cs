/**
 * $File: JCS_ButtonSelectionGroup.cs $
 * $Date: 2017-10-07 14:18:04 $
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
    public delegate void SelectionChanged();

    /// <summary>
    /// Group certain button and wait for selection instead of 
    /// just using the mouse to select the key. Usually work 
    /// with gamepad/joystick/console.
    /// </summary>
    public class JCS_ButtonSelectionGroup
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        // Callback triggered when selection has changed.
        public SelectionChanged selectionChanged = null;

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Check Variables (JCS_ButtonSelectionGroup) **")]

        [Tooltip("Current selecting index.")]
        [SerializeField]
        private int mCurrentSelectIndex = 0;


        [Header("** Runtime Variables (JCS_ButtonSelectionGroup) **")]

        [Tooltip("")]
        [SerializeField]
        private List<JCS_ButtonSelection> mSelections = null;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            this.mSelections = JCS_Utility.RemoveEmptySlot(mSelections);

            selectionChanged = EmptyCallbackSelectionChanged;
        }

        private void Start()
        {
            // reset once.
            ResetSelection();
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        /// <summary>
        /// Reset everytime a player enter a area.
        /// </summary>
        private void ResetSelection()
        {
            CloseSelection();

            // only enable the first one.
            mSelections[0].Active = true;
        }

        /// <summary>
        /// Close the selection area. Nothing will be high-lighted.
        /// </summary>
        private void CloseSelection()
        {
            foreach (JCS_ButtonSelection item in mSelections)
            {
                item.Active = false;
            }

            mCurrentSelectIndex = 0;
        }

        /// <summary>
        /// Run the selection button.
        /// </summary>
        public void OkaySelection()
        {
            mSelections[mCurrentSelectIndex].GetButton().JCS_ButtonClick();
        }

        /// <summary>
        /// Change to the next button selection.
        /// </summary>
        public void NextSelection()
        {
            // disable current active selection.
            mSelections[mCurrentSelectIndex].Active = false;

            ++mCurrentSelectIndex;

            // loop through the array, if at head of the array we set it to the tail.
            if (mCurrentSelectIndex >= mSelections.Count)
                mCurrentSelectIndex = 0;

            // active the new active selection.
            mSelections[mCurrentSelectIndex].Active = true;

            selectionChanged.Invoke();
        }

        /// <summary>
        /// Change to the previous button selection.
        /// </summary>
        public void PrevSelection()
        {
            // disable current active selection.
            mSelections[mCurrentSelectIndex].Active = false;

            --mCurrentSelectIndex;

            // loop through the array, if at the tail of the array set it to head.
            if (mCurrentSelectIndex < 0)
                mCurrentSelectIndex = mSelections.Count - 1;

            // active the new active selection.
            mSelections[mCurrentSelectIndex].Active = true;

            selectionChanged.Invoke();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        private void EmptyCallbackSelectionChanged() { }
    }
}
