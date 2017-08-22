
using System;
/**
* $File: JCS_DefaultClientHandler.cs $
* $Date: 2017-08-21 16:38:46 $
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
    /// Default message handler
    /// </summary>
    public class JCS_DefaultClientHandler
        : JCS_ClientHandler
    {
        /// <summary>
        /// Callback when message is sent.
        /// </summary>
        /// <param name="buffer"> buffer we sent. </param>
        public void MessageSent(byte[] buffer)
        {
            PrintSendPacket(buffer);
        }

        /// <summary>
        /// Callback when message received.
        /// </summary>
        /// <param name="buffer"> buffer we received. </param>
        public void MessageReceived(byte[] buffer)
        {
            // print it out for test
            PrintRecievedPacket(buffer);
        }


        /// <summary>
        /// Print out the message we are going to send.
        /// </summary>
        /// <param name="message"> meesage sending to print. </param>
        private void PrintSendPacket(System.Object message)
        {
#if (UNITY_EDITOR)
            if (!JCS_GameSettings.instance.DEBUG_MODE)
                return;

            byte[] encryptedBuffer = (byte[])message;

            // Print out the buffer for test
            JCS_Logger.Info("HeapBuffer[" + BitConverter.ToString(encryptedBuffer) + "]");
#endif
        }

        /// <summary>
        /// Print out the message we received.
        /// </summary>
        /// <param name="message"> message received to print. </param>
        private void PrintRecievedPacket(System.Object message)
        {
#if (UNITY_EDITOR)
            if (!JCS_GameSettings.instance.DEBUG_MODE)
                return;

            byte[] decryptedBuffer = (byte[])message;

            // Print out the buffer for test
            JCS_Logger.Info("HeapBuffer[" + BitConverter.ToString(decryptedBuffer) + "]");
#endif
        }
    }
}
