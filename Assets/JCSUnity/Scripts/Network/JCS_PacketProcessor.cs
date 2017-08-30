/**
 * $File: JCS_PacketProcessor.cs $
 * $Date: 2017-08-25 11:18:45 $
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
    /// Packet Handler generator.
    /// </summary>
    public class JCS_PacketProcessor
    {
        private static JCS_PacketProcessor instance = null;

        private JCS_PacketHandler[] mHandlers = null;

        // singleton
        public static JCS_PacketProcessor GetProcessor()
        {
            if (instance == null)
                instance = new JCS_PacketProcessor();
            return instance;
        }

        private JCS_PacketProcessor()
        {
            int maxRecvOp = 0;
            foreach (JCS_RecvPacketType op in JCS_Utility.GetValues<JCS_RecvPacketType>())
            {
                int opId = (int)op;

                if (opId > maxRecvOp)
                    maxRecvOp = opId;
            }

            mHandlers = new JCS_PacketHandler[maxRecvOp + 1];

            // NOTE(jenchieh): just use the channel server for now.
            Reset(JCS_ClientMode.CHANNEL_SERVER);
        }

        /// <summary>
        /// Get the packet by the packet id.
        /// </summary>
        /// <param name="packetId"> packet id. </param>
        /// <returns> packet handller that handles the packet by the 
        /// packet id. </returns>
        public JCS_PacketHandler GetHandler(short packetId)
        {
            if (packetId > mHandlers.Length)
                return null;

            JCS_PacketHandler handler = mHandlers[packetId];
            if (handler != null)
                return handler;
            return null;
        }

        public void RegisterHandler(JCS_RecvPacketType code, JCS_PacketHandler handler)
        {
            mHandlers[(int)code] = handler;
        }

        /// <summary>
        /// Reset all the packet handler list by mode.
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        private void Reset(JCS_ClientMode mode)
        {
            mHandlers = new JCS_PacketHandler[mHandlers.Length];

            // General
            RegisterHandler(JCS_RecvPacketType.HELLO, new JCS_HelloHandler());

            if (mode == JCS_ClientMode.LOGIN_SERVER)
            {

            }
            else if (mode == JCS_ClientMode.CHANNEL_SERVER)
            {

            }
        }
    }
}
