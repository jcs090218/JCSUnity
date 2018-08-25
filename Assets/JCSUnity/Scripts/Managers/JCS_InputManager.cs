/**
 * $File: JCS_InputManager.cs $
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
    /// Take care of the cross platform input.
    /// </summary>
    public class JCS_InputManager
        : JCS_Managers<JCS_InputManager>
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables


        [Header("** Check Varaibles (JCS_InputManager) **")]

        [Tooltip("")]
        [SerializeField]
        private JCS_SlideInput mSlideInput = null;

        [Tooltip("Handle mobile even component.")]
        [SerializeField]
        private JCS_MobileMouseEvent mMobileMouseEvent = null;


        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public void SetJCSSlideInput(JCS_SlideInput sl) { this.mSlideInput = sl; }
        public JCS_SlideInput GetJCSSlideInput() { return this.mSlideInput; }
        public void SetJCSMobileMouseEvent(JCS_MobileMouseEvent me) { this.mMobileMouseEvent = me; }
        public JCS_MobileMouseEvent GetJCSMobileMouseEvent() { return this.mMobileMouseEvent; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            AddInputBaseOnPlatform();
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
        /// Add the input type base on the platform type.
        /// </summary>
        private void AddInputBaseOnPlatform()
        {
            switch (JCS_ApplicationManager.instance.PLATFORM_TYPE)
            {
                case JCS_PlatformType.MOBILE:
                    this.gameObject.AddComponent<JCS_SlideInput>();
                    this.gameObject.AddComponent<JCS_MobileMouseEvent>();
                    break;
            }
        }

    }
}
