/**
 * $File: JCS_BlackSlideScreen.cs $
 * $Date: 2016-12-10 06:05:30 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// Black Slide Screen
    /// </summary>
    [RequireComponent(typeof(JCS_SlideEffect))]
    public class JCS_BlackSlideScreen
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_SlideEffect mSlideEffect = null;

        [SerializeField]
        private RectTransform mFadeScreenTop = null;

        [SerializeField]
        private RectTransform mFadeScreenBottom = null;

        [SerializeField]
        private RectTransform mFadeScreenRight = null;

        [SerializeField]
        private RectTransform mFadeScreenLeft = null;

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
            mSlideEffect = this.GetComponent<JCS_SlideEffect>();
        }

        private void Start()
        {
            // everytime it reload the scene.
            // move to the last child make sure everything get cover by this.
            JCS_Utility.MoveToTheLastChild(this.transform);
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Start sliding the screen in the scene
        /// </summary>
        /// <param name="align"> align type </param>
        public void StartSlideIn(JCS_Align align, float time)
        {
            // NOTE(jenchieh): this is just some tweeking.
            mSlideEffect.Friction = time / 2;

            mSlideEffect.Deactive();
        }

        /// <summary>
        /// Start sliding the screen out the scene
        /// </summary>
        /// <param name="align"> align type </param>
        public void StartSlideOut(JCS_Align align, float time)
        {
            float imageSize = 1200;
            float distanceX = JCS_Canvas.instance.GetAppRect().sizeDelta.x + imageSize;
            float distanceY = JCS_Canvas.instance.GetAppRect().sizeDelta.y + imageSize;
            

            // NOTE(jenchieh): this is just some tweeking.
            mSlideEffect.Friction = time;

            switch (align)
            {
                // going left showing from right
                // ============--------------
                case JCS_Align.ALIGN_RIGHT:
                    {
                        mSlideEffect.Axis = JCS_Axis.AXIS_X;
                        mSlideEffect.Distance = JCS_Mathf.ToNegative(distanceX);
                    }
                    break;

                // going right showing from left
                // -----------=============
                case JCS_Align.ALIGN_LEFT:
                    {
                        mSlideEffect.Axis = JCS_Axis.AXIS_X;
                        mSlideEffect.Distance = JCS_Mathf.ToPositive(distanceX);
                    }
                    break;

                case JCS_Align.ALIGN_BOTTOM:
                    {
                        mSlideEffect.Axis = JCS_Axis.AXIS_Y;
                        mSlideEffect.Distance = JCS_Mathf.ToPositive(distanceY);
                    }
                    break;
                case JCS_Align.ALIGN_TOP:
                    {
                        mSlideEffect.Axis = JCS_Axis.AXIS_Y;
                        mSlideEffect.Distance = JCS_Mathf.ToNegative(distanceY);
                    }
                    break;
            }

            // start sliding
            mSlideEffect.Active();
        }

        /// <summary>
        /// Is done sliding screen
        /// </summary>
        /// <returns>
        /// true: done sliding
        /// false: not done sliding
        /// </returns>
        public bool IsDoneSliding()
        {
            return mSlideEffect.IsIdle(70);
        }

        /// <summary>
        /// Move the panel to front
        /// </summary>
        public void MoveToTheLastChild()
        {
            Transform parent = this.transform.parent;

            Vector3 recordPos = this.transform.localPosition;
            Vector3 recordScale = this.transform.localScale;
            Quaternion recordRot = this.transform.localRotation;

            // this part will mess up the transform
            // so we record all we need and set it back
            {
                this.transform.SetParent(null);
                this.transform.SetParent(parent);
            }

            // here we set it back!
            this.transform.localPosition = recordPos;
            this.transform.localScale = recordScale;
            this.transform.localRotation = recordRot;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
