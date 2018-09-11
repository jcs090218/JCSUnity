/**
 * $File: JCS_ScreenManager.cs $
 * $Date: 2018-09-12 02:50:44 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Manages screen action.
    /// </summary>
    public class JCS_ScreenManager
        : JCS_Managers<JCS_ScreenManager>
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        [Header("** Initialize Variables (JCS_ScreenManager) **")]

        [Tooltip("Resize screen once on this scene?")]
        public bool RESIZE_SCREEN_THIS_SCENE = false;

        [Tooltip("Aspect ratio screen width for this scene you want to resize.")]
        public int ASPECT_RATION_SCREEN_WIDTH_THIS_SCENE = 16;

        [Tooltip("Aspect ratio screen height for this scene you want to resize.")]
        public int ASPECT_RATION_SCREEN_HEIGHT_THIS_SCENE = 9;

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

        private void Start()
        {
            if (RESIZE_SCREEN_THIS_SCENE)
            {
                JCS_ScreenSettings ss = JCS_ScreenSettings.instance;

                // Apply new screen aspect ratio.
                ss.ASPECT_RATIO_SCREEN_WIDTH = ASPECT_RATION_SCREEN_WIDTH_THIS_SCENE;
                ss.ASPECT_RATIO_SCREEN_HEIGHT = ASPECT_RATION_SCREEN_HEIGHT_THIS_SCENE;

                // Resize the screen base on the new screen aspect ratio.
                ss.ForceAspectScreenOnce();
            }
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

    }
}
