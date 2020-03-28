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
        /* Variables */

        [Header("** Check Varaibles (JCS_InputManager) **")]

        [Tooltip("Slide input receiver.")]
        [SerializeField]
        private JCS_SlideInput mSlideInput = null;

        [Tooltip("Handle mobile event component.")]
        [SerializeField]
        private JCS_MobileMouseEvent mMobileMouseEvent = null;

        /* Setter & Getter */

        public JCS_SlideInput GetGlobalSlideInput()
        {
#if UNITY_EDITOR
            if (JCS_GameSettings.instance.DEBUG_MODE)
            {
                JCS_Debug.LogWarning("You tried to access global slide input but doesn't exists");
                return null;
            }
#endif
            return this.mSlideInput;
        }
        public JCS_MobileMouseEvent GetGlobalMobileMouseEvent()
        {
#if UNITY_EDITOR
            if (JCS_GameSettings.instance.DEBUG_MODE)
            {
                JCS_Debug.LogWarning("You tried to access global mobile mouse event but doesn't exists");
                return null;
            }
#endif
            return this.mMobileMouseEvent;
        }

        /* Functions */

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            AddInputBaseOnPlatform();
        }

        /// <summary>
        /// Add the input type base on the platform type.
        /// </summary>
        private void AddInputBaseOnPlatform()
        {
            switch (JCS_ApplicationManager.instance.PLATFORM_TYPE)
            {
                case JCS_PlatformType.MOBILE:
                    this.mSlideInput = this.gameObject.AddComponent<JCS_SlideInput>();
                    this.mMobileMouseEvent = this.gameObject.AddComponent<JCS_MobileMouseEvent>();
                    break;
            }
        }
    }
}
