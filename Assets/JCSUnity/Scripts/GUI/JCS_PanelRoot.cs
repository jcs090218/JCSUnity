/**
 * $File: JCS_PanelRoot.cs $
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
    /// Panel will make sure all the following
    /// child object fit the screen size!
    /// </summary>
    public class JCS_PanelRoot
        : JCS_BaseDialogueObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Check Variables (JCS_PanelRoot) **")]

        [Tooltip("Delta ratio change of the screen width.")]
        [SerializeField]
        private float mPanelDeltaWidthRatio = 0;

        [Tooltip("Delta ratio change of the screen height.")]
        [SerializeField]
        private float mPanelDeltaHeightRatio = 0;


        [Header("** Initialize Variables (JCS_PanelRoot) **")]

        [Tooltip("Fit the whole screen size?")]
        [SerializeField]
        private bool mFitScreenSize = true;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool FitScreenSize { get { return this.mFitScreenSize; } set { this.mFitScreenSize = value; } }
        public float PanelDeltaWidthRatio { get { return this.mPanelDeltaWidthRatio; } }
        public float PanelDeltaHeightRatio { get { return this.mPanelDeltaHeightRatio; } }

        //========================================
        //      Unity's function
        //------------------------------
        protected override void Awake()
        {
            base.Awake();

            // NOTE(jenhiche): not sure is this the 
            // correct position for the code or not.
            if (mFitScreenSize)
            {
                FitPerfectSize();

                AddPanelChild();
            }
        }

        protected override void Start()
        {
            base.Start();
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

        /// <summary>
        /// Fit screen size base on Unity Engine architecture.
        /// </summary>
        private void FitPerfectSize()
        {
            JCS_ScreenSettings ss = JCS_ScreenSettings.instance;

            // get app rect
            RectTransform appRect = JCS_Canvas.instance.GetAppRect();

            float newWidth = appRect.sizeDelta.x;
            float newHeight = appRect.sizeDelta.y;

            /* Adjust the panel width and height base on the
             * blackspaces width and height.
             */
            {
                float blackspace_width = ss.BlackspaceWidth();
                float blackspace_height = ss.BlackspaceHeight();

                bool blackspace_w_valid = (JCS_Mathf.isPositive(blackspace_width) || blackspace_width == 0.0f);
                bool blackspace_h_valid = (JCS_Mathf.isPositive(blackspace_height) || blackspace_height == 0.0f);

                if (ss.STARTING_SCREEN_WIDTH != 0)
                {
                    // There is blackspaces on the horizontal axis. (left and right)
                    if (blackspace_w_valid)
                        newWidth -= blackspace_width;
                    // Otherwise should be on the vertical axis. (top and bottom)
                    else
                        newWidth += blackspace_height;
                }

                if (ss.STARTING_SCREEN_HEIGHT != 0)
                {
                    // There is blackspaces on the vertical axis. (top and bottom)
                    if (blackspace_h_valid)
                        newHeight -= blackspace_height;
                    // Otherwise should be on the horizontal axis. (left and right)
                    else
                        newHeight += blackspace_width;
                }
            }

            float currentWidth = mRectTransform.sizeDelta.x;
            float currentHeight = mRectTransform.sizeDelta.y;

            mPanelDeltaWidthRatio = currentWidth / newWidth;
            mPanelDeltaHeightRatio = currentHeight / newHeight;

            Vector3 newPosition = mRectTransform.localPosition;

            if (JCS_Camera.main != null)
            {
                // make toward to the camera position
                Camera cam = JCS_Camera.main.GetCamera();

                if (cam != null)
                {
                    // Find the distance between the dialogue object and 
                    // the center (which is camera in this case)
                    float distanceX = mRectTransform.localPosition.x - cam.transform.localPosition.x;
                    float distanceY = mRectTransform.localPosition.y - cam.transform.localPosition.y;

                    newPosition.x = (distanceX / mPanelDeltaWidthRatio);
                    newPosition.y = (distanceY / mPanelDeltaHeightRatio);
                }
            }

            /*
             * NOTE(jenchieh): 
             * Cool, `sizeDelta' will actually change the `localPosition'
             * now since version 2017.4.
             * 
             * So we set the `sizeDelta' (width and height) first, then
             * set the `localPosition'.
             */
            {
                // set the width and height to the new app rect
                mRectTransform.sizeDelta = new Vector2(newWidth, newHeight);

                // set to the new position
                mRectTransform.localPosition = newPosition;
            }
        }

        /// <summary>
        /// Add the panel child to all panel child.
        /// </summary>
        private void AddPanelChild()
        {
            Transform tempTrans = this.transform;

            // loop through the child object and 
            // add on to it.
            for (int index = 0;
                index < transform.childCount;
                ++index)
            {
                Transform child = tempTrans.GetChild(index);

                // Only added once.
                if (child.GetComponent<JCS_PanelChild>() != null)
                    continue;

                JCS_PanelChild panelChild = child.gameObject.AddComponent<JCS_PanelChild>();
                panelChild.PanelRoot = this;
            }
        }

    }
}
