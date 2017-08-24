/**
 * $File: JCS_PacketHandler.cs $
 * $Date: 2017-08-21 17:47:52 $
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
    /// Base packet handler.
    /// </summary>
    public interface JCS_PacketHandler<CLIENT>
    {
        /// <summary>
        /// Handle packet.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="client"></param>
        void handlePacket(BinaryReader br, CLIENT client);

        /// <summary>
        /// This method validates some general state constrains.
        /// For example that the Client has to be logged in for this
        /// packet.When the method returns false the Client should
        /// be disconnected.Further validation based on the content
        /// of the packet and disconnecting the client if it's invalid
        /// in handlePacket is recommended.
        /// </summary>
        /// <param name="client"> the client </param>
        /// <returns> validate state? </returns>
        bool validateState(CLIENT client);
    }
}
