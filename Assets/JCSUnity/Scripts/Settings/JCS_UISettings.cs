/**
 * $File: JCS_UISettings.cs $
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
    /// UI related settings.
    /// </summary>
    public class JCS_UISettings : JCS_Settings<JCS_UISettings>
    {
        /* Variables */

        // No one care about how black screen look so I just make it unseen 
        // in the inspector.
        public const string BLACK_SCREEN_PATH = "UI/System/JCS_BlackScreen";
        public const string BLACK_SLIDE_SCREEN_PATH = "UI/System/JCS_BlackSlideScreen";
        public const string BLACK_SCREEN_NAME = "JCS_BlackScreen";
        public const string WHITE_SCREEN_PATH = "UI/System/JCS_WhiteScreen";
        public const string WHITE_SCREEN_NAME = "JCS_WhiteScreen";

        public const string FADE_SCREEN_PATH = "UI/System/JCS_FadeScreen";
        public const string FADE_SCREEN_NAME = "JCS_FadeScreen";

        [Separator("Initialize Variables (JCS_UISettings)")]

        [Tooltip("Resize the UI in runtime.")]
        public bool resizeUI = true;

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            CheckInstance(this);
        }

        /// <summary>
        /// Make limit so not all the data override the by the new data!
        /// </summary>
        /// <param name="_old"> old data we copy from </param>
        /// <param name="_new"> new data we copy to </param>
        protected override void TransferData(JCS_UISettings _old, JCS_UISettings _new)
        {
            // ResizeUI option should always be the same!
            _new.resizeUI = _old.resizeUI;
        }
    }
}
