/**
 * $File: JCS_ScreenSettings.cs $
 * $Date: 2018-09-08 15:15:28 $
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
    /// Screen related settings.
    /// </summary>
    public class JCS_ScreenSettings
        : JCS_Settings<JCS_ScreenSettings>
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Check Variables (JCS_ScreenSettings) **")]

        [Tooltip("When the application start, what's the screen width?")]
        public int STARTING_SCREEN_WIDTH = 0;

        [Tooltip("When the application start, what's the screen height?")]
        public int STARTING_SCREEN_HEIGHT = 0;

        [Tooltip("Store the camera orthographic size value over scene.")]
        public float ORTHOGRAPHIC_SIZE = 10.0f;

        [Tooltip("Store the camera filed of view value over scene.")]
        public float FIELD_OF_VIEW = 90.0f;


        [Header("** Runtime Variables (JCS_ScreenSettings) **")]

        [Tooltip("Type of the screen handle.")]
        public JCS_ScreenType SCREEN_TYPE = JCS_ScreenType.RESIZABLE;

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
            instance = CheckSingleton(instance, this);
        }

        private void Start()
        {
            // NOTE(jenchieh): Here is the execution order implementation.
            // 'APPLICATION_STARTS' will be true after the first scene's 
            // main game loop is runs. 
            if (JCS_ApplicationSettings.instance.APPLICATION_STARTS)
            {
                Camera cam = JCS_Camera.main.GetCamera();
                cam.fieldOfView = FIELD_OF_VIEW;
                cam.orthographicSize = ORTHOGRAPHIC_SIZE;
            }
            // Otherwise, this will only run once at the time when 
            // the application is starts.
            else
            {
                STARTING_SCREEN_WIDTH = Screen.width;
                STARTING_SCREEN_HEIGHT = Screen.height;
            }
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

        /// <summary>
        /// Return width of the blackspace on the screen, if any 
        /// after resizing the screen.
        /// </summary>
        /// <returns></returns>
        public float BlackspaceWidth()
        {
            return Screen.width - STARTING_SCREEN_WIDTH;
        }

        /// <summary>
        /// Return height of the blackspace on the screen, if any 
        /// after resizing the screen.
        /// </summary>
        /// <returns></returns>
        public float BlackspaceHeight()
        {
            return Screen.height - STARTING_SCREEN_HEIGHT;
        }

        //----------------------
        // Protected Functions

        /// <summary>
        /// Instead of Unity Engine's scripting layer's DontDestroyOnLoad.
        /// I would like to use own define to transfer the old instance
        /// to the newer instance.
        /// 
        /// Every time when unity load the scene. The script have been
        /// reset, in order not to lose the original setting.
        /// transfer the data from old instance to new instance.
        /// </summary>
        /// <param name="_old"> old instance </param>
        /// <param name="_new"> new instance </param>
        protected override void TransferData(JCS_ScreenSettings _old, JCS_ScreenSettings _new)
        {
            _new.SCREEN_TYPE = _old.SCREEN_TYPE;

            _new.ORTHOGRAPHIC_SIZE = _old.ORTHOGRAPHIC_SIZE;
            _new.FIELD_OF_VIEW = _old.FIELD_OF_VIEW;

            _new.STARTING_SCREEN_WIDTH = _old.STARTING_SCREEN_WIDTH;
            _new.STARTING_SCREEN_HEIGHT = _old.STARTING_SCREEN_HEIGHT;
        }

        //----------------------
        // Private Functions

    }
}
