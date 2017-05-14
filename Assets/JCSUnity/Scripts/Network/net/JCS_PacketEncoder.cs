/**
 * $File: JCS_PacketEncoder.cs$
 * $Date: $
 * $Reveision: $
 * $Creator: Jen-Chieh Shen $
 */
using System.Collections;
using System;

namespace JCSUnity
{
    public class JCS_PacketEncoder : PacketEncoder
    {

        public System.Object Encode(System.Object message)
        {
            byte[] unencrypted = (byte[])message;

            int packetLength = JCS_NetworkConstant.ENCODE_BUFFER_LEN + unencrypted.Length;

            // Check packet length
            if (packetLength < 0 || packetLength > JCS_NetworkConstant.OUTBUFSIZE)
            {
                // TODO(JenChieh): split the packet system
                JCS_Debug.LogError(
                    "JCS_PacketEncoder", 
                    "Packet u are sending is to big!");

                return null;
            }

            byte[] encryptedBuffer = new byte[packetLength];

            // encrypt the packet for security usage
            for (int index = 0;
                index < JCS_NetworkConstant.ENCODE_BUFFER_LEN;
                ++index)
            {
                encryptedBuffer[index] = JCS_NetworkConstant.ENCODE_BUFFER[index];
            }

            // apply message
            for (int index = JCS_NetworkConstant.ENCODE_BUFFER_LEN;
                index < packetLength;
                ++index)
            {
                encryptedBuffer[index] = unencrypted[index - JCS_NetworkConstant.ENCODE_BUFFER_LEN];
            }

            return encryptedBuffer;
        }

    }
}
