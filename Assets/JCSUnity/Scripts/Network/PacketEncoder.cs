/**
 * $File: PacketEncoder.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using System.Collections;
using System;

namespace JCSUnity
{
    /// <summary>
    /// Interface of all encoder
    /// </summary>
    public interface PacketEncoder
    {
        System.Object Encode(Object message);
    }
}
