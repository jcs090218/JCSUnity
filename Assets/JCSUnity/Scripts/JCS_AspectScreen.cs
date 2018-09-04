/**
 * $File: JCS_AspectScreen.cs $
 * $Date: 2018-09-03 22:27:30 $
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
    /// Make the proportional screen/window. 
    /// 
    /// SOURCE: https://gamedev.stackexchange.com/questions/86707/how-to-lock-aspect-ratio-when-resizing-game-window-in-unity
    /// AUTHOR: Entity in JavaScript.
    /// Modefied: Jen-Chieh Shen to C#.
    /// </summary>
    public class JCS_AspectScreen
        : MonoBehaviour
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        [Header("** Check Variables (JCS_AspectScreen) **")]

        [Tooltip("Record down the previous screen width.")]
        [SerializeField]
        private int mLastWidth = 0;

        [Tooltip("Record down the previous screen height.")]
        [SerializeField]
        private int mLastHeight = 0;


        [Header("** Runtime Variables (JCS_AspectScreen) **")]

        [Tooltip("Target aspect ratio screen width.")]
        [SerializeField]
        private int mAspectRatioScreenWidth = 16;

        [Tooltip("Target aspect ratio screen height.")]
        [SerializeField]
        private int mAspectRatioScreenHeight = 9;

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void LateUpdate()
        {
            if (Screen.fullScreen)
                return;

            int width = Screen.width;
            int height = Screen.height;

            // if the user is changing the width
            if (mLastWidth != width)
            {
                // update the height
                float heightAccordingToWidth = width / mAspectRatioScreenWidth * mAspectRatioScreenHeight;
                Screen.SetResolution(width, (int)Mathf.Round(heightAccordingToWidth), false, 0);
            }

            // if the user is changing the height
            if (mLastHeight != height)
            {
                // update the width
                float widthAccordingToHeight = height / mAspectRatioScreenHeight * mAspectRatioScreenWidth;
                Screen.SetResolution((int)Mathf.Round(widthAccordingToHeight), height, false, 0);
            }

            this.mLastWidth = width;
            this.mLastHeight = height;
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
