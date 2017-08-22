
using System;
/**
* $File: JCS_NetworkSettings.cs $
* $Date: 2017-08-20 07:08:46 $
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
    /// Store all the network settings.
    /// </summary>
    public class JCS_NetworkSettings
        : JCS_Settings<JCS_NetworkSettings>
    {

        /*******************************************/
        /*            Public Variables             */
        /*******************************************/

        [Header("** Online Game Configuration **")]
        public bool ONLINE_MODE = false;

        public string HOST_NAME = "127.0.0.1";
        public int PORT = 5454;

        private static JCS_GameSocket GAME_SOCKET = null;
        private static JCS_ClientHandler PRESET_CLIENT_HANDLER = null;

        /*******************************************/
        /*           Private Variables             */
        /*******************************************/

        /*******************************************/
        /*           Protected Variables           */
        /*******************************************/

        /*******************************************/
        /*             setter / getter             */
        /*******************************************/
        public static void PresetClientHandler(JCS_ClientHandler handler) { PRESET_CLIENT_HANDLER = handler; }
        public static JCS_ClientHandler GetPresetClientHandler() { return PRESET_CLIENT_HANDLER; }

        /*******************************************/
        /*            Unity's function             */
        /*******************************************/
        private void Awake()
        {
            instance = CheckSingleton(instance, this);
        }

        private void OnApplicationQuit()
        {
            if (JCS_NetworkSettings.instance.ONLINE_MODE)
                GAME_SOCKET.Close();
        }

        /*******************************************/
        /*              Self-Define                */
        /*******************************************/
        //----------------------
        // Public Functions

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
        protected override void TransferData(JCS_NetworkSettings _old, JCS_NetworkSettings _new)
        {
            _new.ONLINE_MODE = _old.ONLINE_MODE;
            _new.HOST_NAME = _old.HOST_NAME;
            _new.PORT = _old.PORT;
        }

        /// <summary>
        /// Create the socket and connect to the host and 
        /// port provided.
        /// </summary>
        /// <param name="hostname"> host name </param>
        /// <param name="port"> port number </param>
        /// <returns> Sucess or vice versa. </returns>
        public static bool CreateNetwork(string hostname, int port, JCS_ClientHandler handler = null)
        {
            if (GAME_SOCKET != null)
                return false;

            GAME_SOCKET = new JCS_GameSocket(handler);
            GAME_SOCKET.Connect(hostname, port);

            return true;
        }

        /// <summary>
        /// Close the current socket the safe way.
        /// </summary>
        public static void CloseSocket()
        {
            if (GAME_SOCKET == null)
                return;

            GAME_SOCKET.Close();
        }

        /// <summary>
        /// Return the Game socket we are using.
        /// </summary>
        /// <returns> socket. </returns>
        public static JCS_GameSocket GetGameSocket()
        {
            return GAME_SOCKET;
        }

        //----------------------
        // Private Functions

    }
}
