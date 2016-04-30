/**
 * $File: PacketDecoder.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{
    /// <summary>
    /// Interface of all decoder
    /// </summary>
    public interface PacketDecoder
    {
        System.Object Decode(System.Object message);
    }
}
