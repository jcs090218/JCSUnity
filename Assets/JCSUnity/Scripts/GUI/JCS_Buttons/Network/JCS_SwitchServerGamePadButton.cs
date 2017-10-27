/**
 * $File: JCS_SwitchServerGamePadButton.cs $
 * $Date: 2017-10-27 12:19:15 $
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
    /// Button when switching the server. For example, switch 
    /// channel server. (Game Pad)
    /// </summary>
    public class JCS_SwitchServerGamePadButton
        : JCS_GamePadButton
    {
        [Header("** Runtime Variables (JCS_SwitchServerGamePadButton) **")]

        [Tooltip("Target host name to switch.")]
        [SerializeField]
        private string mHost = JCS_NetworkConstant.SERVER_HOST;

        [Tooltip("Target Port to switch.")]
        [SerializeField]
        private int mPort = JCS_NetworkConstant.LOGIN_SERVER_PORT;

        [Tooltip("Force to switch server.")]
        [SerializeField]
        private bool mForce = false;

        /* setter/getter */
        public int Port { get { return this.mPort; } set { this.mPort = value; } }
        public string Host { get { return this.mHost; } set { this.mHost = value; } }


        public override void JCS_ButtonClick()
        {
            base.JCS_ButtonClick();

            // Switch the server.
            JCS_NetworkSettings.instance.SwitchServer(mHost, mPort, mForce);
        }
    }
}
