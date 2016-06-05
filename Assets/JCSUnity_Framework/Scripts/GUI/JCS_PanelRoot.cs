/**
 * $File: JCS_PanelRoot.cs $
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

    /// <summary>
    /// Panel will make sure all the following
    /// child object fit the screen size!
    /// </summary>
    public class JCS_PanelRoot
        : JCS_BaseDialogueObject
    {

        //----------------------
        // Public Variables
        [HideInInspector] public float mPanelDeltaWidthRatio = 0;
        [HideInInspector] public float mPanelDeltaHeightRatio = 0;

        //----------------------
        // Private Variables

        [Header("** Initialize Variables (JCS_PanelRoot) **")]
        [SerializeField] private bool mFitScreenSize = true;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------

        protected override void Start()
        {
            if (mFitScreenSize)
            {
                FitPerfectSize();

                AddPanelChild();
            }
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
        private void FitPerfectSize()
        {
            // get app rect
            RectTransform appRect = JCS_UIManager.instance.GetJCSCanvas().GetAppRect();

            float newWidth = appRect.sizeDelta.x;
            float newHeight = appRect.sizeDelta.y;

            float currentWidth = mRectTransform.sizeDelta.x;
            float currentHeight = mRectTransform.sizeDelta.y;

            mPanelDeltaWidthRatio = currentWidth / newWidth;
            mPanelDeltaHeightRatio = currentHeight / newHeight;

            Vector3 newPosition = mRectTransform.localPosition;

            // make toward to the camera position
            Camera cam = JCS_Camera.main.GetCamera();


            // Find the distance between the dialogue object and the center (which is camera in this case)
            float distanceX = mRectTransform.localPosition.x - cam.transform.localPosition.x;
            float distanceY = mRectTransform.localPosition.y - cam.transform.localPosition.y;

            newPosition.x = (distanceX / mPanelDeltaWidthRatio);
            newPosition.y = (distanceY / mPanelDeltaHeightRatio);

            // set to the new position
            mRectTransform.localPosition = newPosition;

            // set the width and height from app rect
            mRectTransform.sizeDelta = appRect.sizeDelta;
        }
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
