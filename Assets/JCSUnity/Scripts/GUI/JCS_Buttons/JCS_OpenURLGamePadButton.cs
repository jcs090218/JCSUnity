/**
 * $File: JCS_OpenURLGamePadButton.cs $
 * $Date: 2017-10-27 11:50:47 $
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
    /// Open a URL in the default browser. (Game Pad)
    /// </summary>
    public class JCS_OpenURLGamePadButton
    : JCS_GamePadButton
    {
        [Header("** Runtime Variables (JCS_OpenURLButton) **")]

        [Tooltip("URL to open in the default browser.")]
        [SerializeField]
        private string mURL = "www.your-url.com";


        public string URL { get { return this.mURL; } set { this.mURL = value; } }


        public override void JCS_ButtonClick()
        {
            Application.OpenURL(mURL);
        }
    }
}
