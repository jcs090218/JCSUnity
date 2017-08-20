/**
 * $File: JCS_Logger.cs $
 * $Date: 2017-08-20 12:34:33 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.IO;


namespace System.Runtime.CompilerServices
{
    sealed class CallerFilePathAttribute : Attribute { }
    sealed class CallerMemberNameAttribute : Attribute { }
    sealed class CallerLineNumberAttribute : Attribute { }
}

namespace JCSUnity
{

    /// <summary>
    /// Common Logger for JCSUnity.
    /// 
    /// This log more detail than just a log from 'JCS_Debug'.
    /// </summary>
    public static class JCS_Logger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(
            string msg,
            [CallerFilePathAttribute] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumberAttribute] int line = 0)
        {
#if (UNITY_EDITOR)
            Debug.Log("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=");
            Debug.Log("INFO: " + msg);
            Debug.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + Path.GetFileName(file) + " " + member + "(" + line + ")");
            Debug.Log("=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=\n");
#endif
        }

    }
}
