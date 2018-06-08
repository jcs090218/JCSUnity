/**
 * $File: JCS_PanelChild.cs $
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

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class JCS_PanelChild
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private RectTransform mRectTransform = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();

            // Rely on "Script Execution Order"
            {
                JCS_PanelRoot jpr = this.GetComponentInParent<JCS_PanelRoot>();

                // get all the same class object on this game object.
                JCS_PanelChild[] tempPanelChild = null;
                tempPanelChild = this.GetComponents<JCS_PanelChild>();

                // only do it once.
                if (tempPanelChild.Length == 1 &&
                    tempPanelChild[0] == this)
                {
                    FitPerfectSize(
                    jpr.PanelDeltaWidthRatio,
                    jpr.PanelDeltaHeightRatio);
                }

                // since we add this script assuming we are 
                // int the fit perfect size mode
                // see "JCS_PanelRoot" -> mFitScreenSize variables
                AddPanelChild();
            }
        }


        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xRatio"></param>
        /// <param name="yRatio"></param>
        public void FitPerfectSize(float xRatio, float yRatio)
        {
            Vector3 newPosition = mRectTransform.localPosition;
            newPosition.x = newPosition.x / xRatio;
            newPosition.y = newPosition.y / yRatio;


            float guiWidth = mRectTransform.sizeDelta.x;
            float guiHeight = mRectTransform.sizeDelta.y;

            guiWidth = guiWidth / xRatio;
            guiHeight = guiHeight / yRatio;

            /*
             * NOTE(jenchieh): 
             * Cool, `sizeDelta' will actually change the `localPosition'
             * now since version 2017.4.
             * 
             * So we set the `sizeDelta' (width and height) first, then
             * set the `localPosition'.
             */
            {
                mRectTransform.sizeDelta = new Vector2(guiWidth, guiHeight);

                // set to the new position
                mRectTransform.localPosition = newPosition;
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Add all child with same effect.
        /// </summary>
        private void AddPanelChild()
        {
            Transform tempTrans = this.transform;

            for (int index = 0;
                index < transform.childCount;
                ++index)
            {
                Transform child = tempTrans.GetChild(index);

                child.gameObject.AddComponent<JCS_PanelChild>();
            }
        }

    }
}
