
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
using System.IO;


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
            // convert byte array to stream
            Stream stream = new MemoryStream(buffer);

            // using byte reader for the stream.
            BinaryReader br = new BinaryReader(stream);
            JCS_BinaryReader jcsbr = new JCS_BinaryReader(br);

            short packetId = jcsbr.ReadShort();

            JCS_PacketHandler packetHandler = JCS_PacketProcessor.GetProcessor().GetHandler(packetId);

            // Deserialize the client from the other end.
            //BBS_Client client = new BBS_Client();
            //client.Deserialize(br);
            JCS_Client client = null;

            if (packetHandler != null && packetHandler.validateState(client))
            {
                // set the client and packet data buffer sequence.
                packetHandler.Client = client;
                packetHandler.PacketData = jcsbr;

                // register request.
                JCS_ServerRequestProcessor.instance.RegisterRequest(packetHandler.handlePacket, jcsbr, client);
            }
            else
            {
                JCS_Debug.Log("Exception during processing packet: " + packetHandler);
            }
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
