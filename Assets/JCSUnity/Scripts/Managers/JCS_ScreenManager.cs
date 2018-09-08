/**
 * $File: JCS_ScreenManager.cs $
 * $Date: 2018-09-08 00:28:06 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;


namespace JCSUnity
{
    public delegate void OnScreenResize();

    /// <summary>
    /// Manage the screen.
    /// </summary>
    public class JCS_ScreenManager
        : JCS_Managers<JCS_ScreenManager>
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        public OnScreenResize onScreenResize = null;


        [Header("** Check Variables (JCS_ScreenManager) **")]

        [Tooltip("When the application start, what's the screen width?")]
        public int STARTING_SCREEN_WIDTH = 0;

        [Tooltip("When the application start, what's the screen height?")]
        public int STARTING_SCREEN_HEIGHT = 0;

        [Tooltip("Initialize aspect ratio width.")]
        public int INI_ASPECT_RATIO_WIDTH = 0;

        [Tooltip("Initialize aspect ratio height.")]
        public int INI_ASPECT_RATIO_HEIGHT = 0;

        [Tooltip("Current screen width.")]
        public float CURRENT_SCREEN_WIDTH = 0.0f;

        [Tooltip("Current screen height.")]
        public float CURRENT_SCREEN_HEIGHT = 0.0f;

        [Tooltip("Previous screen width.")]
        public float PREV_SCREEN_WIDTH = 0.0f;

        [Tooltip("Previous screen height.")]
        public float PREV_SCREEN_HEIGHT = 0.0f;


        [Header("** Runtime Variables (JCS_ScreenManager) **")]

        [Tooltip("Target aspect ratio screen width.")]
        [SerializeField]
        public int ASPECT_RATIO_SCREEN_WIDTH = 16;

        [Tooltip("Target aspect ratio screen height.")]
        [SerializeField]
        public int ASPECT_RATIO_SCREEN_HEIGHT = 9;


        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

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
            instance = this;
        }

        private void LateUpdate()
        {
            DoScreenType();
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// Do the task base on the screen type handle.
        /// </summary>
        private void DoScreenType()
        {
            switch (JCS_ScreenSettings.instance.SCREEN_TYPE)
            {
                case JCS_ScreenType.FORCE_ASPECT:
                    DoFoceAspectScreen();
                    break;
                case JCS_ScreenType.RESIZABLE:
                    DoResizableScreen();
                    break;
            }
        }

        /// <summary>
        /// Force aspect window.
        /// 
        /// SOURCE: https://gamedev.stackexchange.com/questions/86707/how-to-lock-aspect-ratio-when-resizing-game-window-in-unity
        /// AUTHOR: Entity in JavaScript.
        /// Modefied: Jen-Chieh Shen to C#.
        /// </summary>
        private void DoFoceAspectScreen()
        {
            if (Screen.fullScreen)
                return;

            int width = Screen.width;
            int height = Screen.height;

            // if the user is changing the width
            if (PREV_SCREEN_WIDTH != width)
            {
                // update the height
                float heightAccordingToWidth = width / ASPECT_RATIO_SCREEN_WIDTH * ASPECT_RATIO_SCREEN_HEIGHT;
                Screen.SetResolution(width, (int)Mathf.Round(heightAccordingToWidth), false, 0);
            }

            // if the user is changing the height
            if (PREV_SCREEN_HEIGHT != height)
            {
                // update the width
                float widthAccordingToHeight = height / ASPECT_RATIO_SCREEN_HEIGHT * ASPECT_RATIO_SCREEN_WIDTH;
                Screen.SetResolution((int)Mathf.Round(widthAccordingToHeight), height, false, 0);
            }

            this.PREV_SCREEN_WIDTH = width;
            this.PREV_SCREEN_HEIGHT = height;
        }

        /// <summary>
        /// Do the resizable window.
        /// </summary>
        private void DoResizableScreen()
        {
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            if (CURRENT_SCREEN_WIDTH == screenWidth &&
                CURRENT_SCREEN_HEIGHT == screenHeight)
                return;


            if (PREV_SCREEN_WIDTH == 0.0f || PREV_SCREEN_HEIGHT == 0.0f)
            {
                // If zero, set to the same value.
                PREV_SCREEN_WIDTH = screenWidth;
                PREV_SCREEN_HEIGHT = screenHeight;

                STARTING_SCREEN_WIDTH = screenWidth;
                STARTING_SCREEN_HEIGHT = screenHeight;

                float gcd = JCS_Mathf.GCD(STARTING_SCREEN_WIDTH, STARTING_SCREEN_HEIGHT);
                INI_ASPECT_RATIO_WIDTH = (int)(STARTING_SCREEN_WIDTH / gcd);
                INI_ASPECT_RATIO_HEIGHT = (int)(STARTING_SCREEN_HEIGHT / gcd);
            }
            else
            {
                // Record previous screen info.
                PREV_SCREEN_WIDTH = CURRENT_SCREEN_WIDTH;
                PREV_SCREEN_HEIGHT = CURRENT_SCREEN_HEIGHT;
            }

            // Update current screen info.
            CURRENT_SCREEN_WIDTH = screenWidth;
            CURRENT_SCREEN_HEIGHT = screenHeight;

            // Do callback.
            if (onScreenResize != null)
                onScreenResize.Invoke();
        }
    }
}
