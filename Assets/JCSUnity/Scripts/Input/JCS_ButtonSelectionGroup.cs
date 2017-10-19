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

            // let them know the grouper.
            foreach (JCS_ButtonSelection bs in mSelections)
                bs.ButtonSelectionGroup = this;

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
        /// Add a selection into list.
        /// </summary>
        /// <param name="selection"></param>
        public void AddSelection(JCS_ButtonSelection selection)
        {
            mSelections.Add(selection);
        }

        /// <summary>
        /// Remove a selection from the list.
        /// </summary>
        /// <param name="selection"></param>
        public void RemoveSelection(JCS_ButtonSelection selection)
        {
            mSelections.Remove(selection);
        }

        /// <summary>
        /// Reset everytime a player enter a area.
        /// </summary>
        public void ResetSelection()
        {
            CloseSelection();

            mCurrentSelectIndex = mSelections.Count;

            if (!IsAllSelectionSkip())
                NextSelection();
        }

        /// <summary>
        /// Close the selection area. Nothing will be high-lighted.
        /// </summary>
        public void CloseSelection()
        {
            foreach (JCS_ButtonSelection item in mSelections)
                item.Active = false;

            mCurrentSelectIndex = 0;
        }

        /// <summary>
        /// Run the selection button.
        /// </summary>
        public void OkaySelection()
        {
            mSelections[mCurrentSelectIndex].DoSelection();
        }

        /// <summary>
        /// Change to the next button selection.
        /// </summary>
        public void NextSelection()
        {
            if (IsAllSelectionSkip())
                return;

            int tempSelectIndex = mCurrentSelectIndex;
            ++tempSelectIndex;

            SelectSelection(tempSelectIndex);

            // if skip keep looking for the next selection.
            if (mSelections[mCurrentSelectIndex].Skip)
                NextSelection();
        }

        /// <summary>
        /// Change to the previous button selection.
        /// </summary>
        public void PrevSelection()
        {
            if (IsAllSelectionSkip())
                return;

            int tempSelectIndex = mCurrentSelectIndex;
            --tempSelectIndex;

            SelectSelection(tempSelectIndex);

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].Skip)
                PrevSelection();
        }

        /// <summary>
        /// Selection this selection.
        /// </summary>
        /// <param name="selectionIndex"> index to select. </param>
        public void SelectSelection(int selectionIndex)
        {
            // no need to do anything.
            if (mCurrentSelectIndex == selectionIndex)
                return;

            if (JCS_Utility.WithInArrayRange(mCurrentSelectIndex, mSelections))
            {
                // disable current active selection.
                mSelections[mCurrentSelectIndex].Active = false;
            }

            this.mCurrentSelectIndex = selectionIndex;

            this.mCurrentSelectIndex = JCS_Utility.LoopInArray(this.mCurrentSelectIndex, mSelections);

            // active the new active selection.
            mSelections[mCurrentSelectIndex].Active = true;

            selectionChanged.Invoke();
        }

        /// <summary>
        /// Selection this selection.
        /// </summary>
        /// <param name="selection"> selection to select. </param>
        public void SelectSelection(JCS_ButtonSelection selection)
        {
            /* 
             * Time complexity: O(n)
             * 
             * NOTE(jenchieh): might need to change this if we there are 
             * more than 30 selections.
             */
            for (int index = 0;
                index < mSelections.Count;
                ++index)
            {
                JCS_ButtonSelection bs = mSelections[index];

                if (bs == selection)
                {
                    SelectSelection(index);
                    return;
                }
            }

            JCS_Debug.LogError(@"Try to select a selection, but seems like the 
selection is not in the group...");
        }

        /// <summary>
        /// Check if all selections are skipped?
        /// </summary>
        /// <returns>
        /// true: all selections are skipped.
        /// false: at least one selection is not skipped.
        /// </returns>
        public bool IsAllSelectionSkip()
        {
            foreach (JCS_ButtonSelection item in mSelections)
            {
                if (!item.Skip)
                    return false;
            }

            CloseSelection();
            return true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        private void EmptyCallbackSelectionChanged() { }
    }
}
