/**
 * $File: JCS_PacketDecoder.cs$
 * $Date: $
 * $Reveision: $
 * $Creator: Jen-Chieh Shen $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public class JCS_PacketDecoder : PacketDecoder
    {

        public System.Object Decode(System.Object message)
        {
            byte[] undecrypted = (byte[])message;

            int packetLength = undecrypted.Length - JCS_NetworkConstant.DECODE_BUFFER_LEN;

            // Check packet length
            if (undecrypted.Length < 0 || undecrypted.Length > JCS_NetworkConstant.INBUFSIZE)
            {
                // TODO(JenChieh): split the packet system
                JCS_GameErrors.JcsErrors("JCS_PacketDecoder", 
                    "Packet recieved is too big!!!");

                return null;
            }

            // decrypt packet and check if damaged / wrong packet
            for (int index = 0;
                index < JCS_NetworkConstant.DECODE_BUFFER_LEN;
                ++index)
            {
                if ((char)undecrypted[index] != (char)JCS_NetworkConstant.DECODE_BUFFER[index])
                {
                    JCS_GameErrors.JcsErrors(
                        "JCS_PacketDecoder",
                        "Wrong Packet Header!!!");

                    return null;
                }
            }

            // Get the real message
            byte[] decryptedBuffer = new byte[packetLength];
            for (int index = 0;
                index < packetLength;
                ++index)
            {
                decryptedBuffer[index] = undecrypted[index + JCS_NetworkConstant.DECODE_BUFFER_LEN];
            }

            return decryptedBuffer;
        }

    }
}
