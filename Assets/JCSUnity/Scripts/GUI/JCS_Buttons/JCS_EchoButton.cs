/**
 * $File: JCS_EchoButton.cs $
 * $Date: 2017-10-07 15:24:48 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JCSUnity
{
    /// <summary>
    /// Echo out a string on the console window when this
    /// button triggered.
    /// </summary>
    public class JCS_EchoButton
    : JCS_Button
    {
        [Header("** Runtime Variables (JCS_EchoButton) **")]

        [Tooltip("String to echo out on the console window.")]
        public string echoString = "echo Hello World!~";

        /// <summary>
        /// Default function to call this, so we dont have to
        /// search the function depends on name.
        /// 
        /// * Good for organize code and game data file in Unity.
        /// </summary>
        public override void JCS_ButtonClick()
        {
            base.JCS_ButtonClick();

            JCS_Debug.Log(echoString);
        }
    }
}
