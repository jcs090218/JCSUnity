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
            ResetAllSelections();
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
        public void ResetAllSelections()
        {
            CloseAllSelections();

            mCurrentSelectIndex = mSelections.Count;

            if (!IsAllSelectionsSkip())
                NextSelection();
        }

        /// <summary>
        /// Close the selection area. Nothing will be high-lighted.
        /// </summary>
        public void CloseAllSelections()
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
        /// Change to the next button selection. (One dimensional)
        /// </summary>
        public void NextSelection()
        {
            if (IsAllSelectionsSkip())
                return;

            int tempSelectIndex = mCurrentSelectIndex;
            ++tempSelectIndex;

            SelectSelection(tempSelectIndex);

            // if skip keep looking for the next selection.
            if (mSelections[mCurrentSelectIndex].Skip)
                NextSelection();
        }

        /// <summary>
        /// Change to the previous button selection. (One dimensional)
        /// </summary>
        public void PrevSelection()
        {
            if (IsAllSelectionsSkip())
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
            if (selection == null)
                return;

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
        public bool IsAllSelectionsSkip()
        {
            foreach (JCS_ButtonSelection item in mSelections)
            {
                if (!item.Skip)
                    return false;
            }

            CloseAllSelections();
            return true;
        }

        /// <summary>
        /// Select the selection on the top. (Two Dimensional)
        /// </summary>
        public void UpSelection()
        {
            if (IsAllSelectionsSkip())
                return;

            SelectSelection(mSelections[mCurrentSelectIndex].UpSelection);

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].Skip)
                UpSelection();
        }

        /// <summary>
        /// Select the selection on the down. (Two Dimensional)
        /// </summary>
        public void DownSelection()
        {
            if (IsAllSelectionsSkip())
                return;

            SelectSelection(mSelections[mCurrentSelectIndex].DownSelection);

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].Skip)
                DownSelection();
        }

        /// <summary>
        /// Select the selection on the right. (Two Dimensional)
        /// </summary>
        public void RightSelection()
        {
            if (IsAllSelectionsSkip())
                return;

            SelectSelection(mSelections[mCurrentSelectIndex].RightSelection);

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].Skip)
                RightSelection();
        }

        /// <summary>
        /// /// Select the selection on the left. (Two Dimensional)
        /// </summary>
        public void LeftSelection()
        {
            if (IsAllSelectionsSkip())
                return;

            SelectSelection(mSelections[mCurrentSelectIndex].LeftSelection);

            // if skip keep looking for the previous selection.
            if (mSelections[mCurrentSelectIndex].Skip)
                LeftSelection();
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        private void EmptyCallbackSelectionChanged() { }
    }
}
