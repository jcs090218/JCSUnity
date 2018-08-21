/**
 * $File: JCS_Toggle.cs $
 * $Date: #CREATIONDATE# $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © #CREATEYEAR# by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Better version of checkbox/toggle GUI.
    /// </summary>
    [RequireComponent(typeof(JCS_ColorTweener))]
    public class JCS_Toggle
        : JCS_Button
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        private JCS_ColorTweener mColorTweener = null;


        [Header("** Initialize Variables (JCS_Toggle) **")]

        [Tooltip("Set the on/off position the same as the current " +
        "sign's position.")]
        [SerializeField]
        private bool mOnOffStartingPosition = true;

        [Tooltip("Is the color same as the editing status?")]
        [SerializeField]
        private bool mColorSameAsEditingStatus = true;


        [Header("** Runtime Variables (JCS_Toggle) **")]

        [Tooltip("Sign of this toggle.")]
        [SerializeField]
        private JCS_ToggleSign mToggleSign = null;

        [Tooltip("Is the toggle currently on or off?")]
        [SerializeField]
        private bool mIsOn = true;

        [Tooltip("Position when is on.")]
        [SerializeField]
        private Vector3 mOnPos = new Vector3(20.0f, 0.0f, 0.0f);


        [Header("- Color (JCS_Toggle) ")]

        [Tooltip("Position when is off.")]
        [SerializeField]
        private Vector3 mOffPos = new Vector3(-20.0f, 0.0f, 0.0f);

        [Tooltip("Button color when is on.")]
        [SerializeField]
        private Color mOnButtonColor = Color.white;

        [Tooltip("Button color when is off.")]
        [SerializeField]
        private Color mOffButtonColor = Color.white;

        [Tooltip("Background color when is on.")]
        [SerializeField]
        private Color mOnBackgroundColor = Color.white;

        [Tooltip("Background color when is off.")]
        [SerializeField]
        private Color mOffBackgroundColor = Color.white;


        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public bool IsOn
        {
            get { return this.mIsOn; }
            set
            {
                this.mIsOn = value;

                // Update toggle once.
                DoToggle();
            }
        }
        public bool OnOffStartingPosition { get { return this.mOnOffStartingPosition; } set { this.mOnOffStartingPosition = value; } }
        public bool ColorSameAsEditingStatus { get { return this.mColorSameAsEditingStatus; } set { this.mColorSameAsEditingStatus = value; } }
        public Vector3 OnPos { get { return this.mOnPos; } set { this.mOnPos = value; } }
        public Vector3 OffPos { get { return this.mOffPos; } set { this.mOffPos = value; } }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        protected override void Awake()
        {
            base.Awake();

            if (mToggleSign == null)
                this.mToggleSign = this.GetComponentInChildren<JCS_ToggleSign>();

            this.mColorTweener = this.GetComponent<JCS_ColorTweener>();
        }

        private void Start()
        {
            SetStartingPosition();
            SetCurrentColorStatus();

            // Update toggle GUI once.
            DoToggle();
        }

#if (UNITY_EDITOR)
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                Toggle();
        }
#endif

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        /// <summary>
        /// On click event.
        /// </summary>
        public override void JCS_OnClickCallback()
        {
            Toggle();
        }

        /// <summary>
        /// Do the toggle actio once.
        /// </summary>
        /// <returns>
        /// true, after toggle once is on.
        /// false, after toggle once is off.
        /// </returns>
        public bool Toggle()
        {
            // Toggle it.
            mIsOn = !mIsOn;

            // Do the real action.
            DoToggle();

            return mIsOn;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Set on/off position to the current transform position.
        /// </summary>
        private void SetStartingPosition()
        {
            if (!mOnOffStartingPosition)
                return;

            if (mIsOn)
            {
                this.mOnPos = mToggleSign.transform.localPosition;
            }
            else
            {
                this.mOffPos = mToggleSign.transform.localPosition;
            }
        }

        /// <summary>
        /// Set all the color info to the current color status.
        /// </summary>
        private void SetCurrentColorStatus()
        {
            if (!mColorSameAsEditingStatus)
                return;

            if (mIsOn)
            {
                mOnButtonColor = mToggleSign.ColorTweener.LocalColor;
                mOnBackgroundColor = mColorTweener.LocalColor;
            }
            else
            {
                mOffButtonColor = mToggleSign.ColorTweener.LocalColor;
                mOffBackgroundColor = mColorTweener.LocalColor;
            }
        }

        /// <summary>
        /// Do the actual toggle action.
        /// </summary>
        private void DoToggle()
        {
            DoToggle(this.mIsOn);
        }

        /// <summary>
        /// Do the actual toggle action.
        /// </summary>
        /// <param name="act"> Is on or off? </param>
        private void DoToggle(bool act)
        {
            if (act)
            {
                mToggleSign.TransformTweener.DoTween(mOnPos);
                mToggleSign.ColorTweener.DoTween(mOnButtonColor);
                mColorTweener.DoTween(mOnBackgroundColor);
            }
            else
            {
                mToggleSign.TransformTweener.DoTween(mOffPos);
                mToggleSign.ColorTweener.DoTween(mOffButtonColor);
                mColorTweener.DoTween(mOffBackgroundColor);
            }
        }

    }
}
