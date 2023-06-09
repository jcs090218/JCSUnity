﻿/**
 * $File: JCS_PanelRoot.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using MyBox;

namespace JCSUnity
{
    /// <summary>
    /// Panel will make sure all the following child object fit the 
    /// screen size!
    /// </summary>
    public class JCS_PanelRoot : JCS_BaseDialogueObject
    {
        /* Variables */

        [Separator("Check Variables (JCS_PanelRoot)")]

        [Tooltip("Delta ratio change of the screen width.")]
        [SerializeField]
        [ReadOnly]
        private float mPanelDeltaWidthRatio = 0.0f;

        [Tooltip("Delta ratio change of the screen height.")]
        [SerializeField]
        [ReadOnly]
        private float mPanelDeltaHeightRatio = 0.0f;

        /* Setter & Getter */

        public float PanelDeltaWidthRatio { get { return this.mPanelDeltaWidthRatio; } }
        public float PanelDeltaHeightRatio { get { return this.mPanelDeltaHeightRatio; } }

        /* Functions */

        protected override void Awake()
        {
            base.Awake();

            // NOTE: not sure is this the correct position for the code or not.
            DoResize();
        }

        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Attempt to get panel root from parent.
        /// </summary>
        /// <param name="trans"> Transform use to search. </param>
        /// <returns> A panel root object. </returns>
        public static JCS_PanelRoot GetFromParent(Transform trans)
        {
            if (trans.parent)
                return trans.parent.GetComponent<JCS_PanelRoot>();
            return null;
        }

        private void DoResize()
        {
            var screenS = JCS_ScreenSettings.instance;

            JCS_ScreenSizef size = screenS.StartingScreenSize();

            float currentWidth = mRectTransform.sizeDelta.x;
            float currentHeight = mRectTransform.sizeDelta.y;

            mPanelDeltaWidthRatio = size.width / currentWidth;
            mPanelDeltaHeightRatio = size.height / currentHeight;

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
                mRectTransform.sizeDelta = new Vector2(size.width, size.height);
            }

            FitPerfectSize();
            AddPanelChild();
        }

        /// <summary>
        /// Fit screen size base on Unity Engine architecture.
        /// </summary>
        private void FitPerfectSize()
        {
            // make toward to the canvas center point
            {
                Vector3 newPosition = mRectTransform.localPosition;

                // This was `camera position`, but we don't need to
                // add up the camera position because Canvas has their 
                // own coordinate system or you can call it's Canvas Space.
                var centerPos = Vector3.zero;

                // Find the distance between the dialogue object and 
                // the center (which is camera in this case)
                float distanceX = newPosition.x - centerPos.x;
                float distanceY = newPosition.y - centerPos.y;

                newPosition.x = (distanceX * mPanelDeltaWidthRatio);
                newPosition.y = (distanceY * mPanelDeltaHeightRatio);

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

            // loop through the child object and add on to it.
            for (int index = 0; index < transform.childCount; ++index)
            {
                Transform child = tempTrans.GetChild(index);

                // Only added once.
                if (child.GetComponent<JCS_PanelChild>() != null || child.GetComponent<JCS_PanelRoot>() != null)
                    continue;

                var panelChild = child.gameObject.AddComponent<JCS_PanelChild>();
                panelChild.PanelRoot = this;
            }
        }
    }
}
