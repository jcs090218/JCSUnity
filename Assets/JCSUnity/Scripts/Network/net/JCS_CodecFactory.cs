/**
 * $File: $
 * $Date: $
 * $Reveision: $
 * $Creator: Jen-Chieh Shen $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public class JCS_CodecFactory
    {

        private static JCS_CodecFactory instance = null;

        private PacketDecoder mDecoder = null;
        private PacketEncoder mEncoder = null;


        private JCS_CodecFactory()
        {
            // Create Decoder and Encoder
            this.mDecoder = new JCS_PacketDecoder();
            this.mEncoder = new JCS_PacketEncoder();
        }

        public static JCS_CodecFactory GetInstance()
        {
            // singleton.
            if (instance == null)
                instance = new JCS_CodecFactory();
            return instance;
        }

        public PacketEncoder GetEncoder() { return this.mEncoder; }
        public PacketDecoder GetDecoder() { return this.mDecoder; }

    }
}
