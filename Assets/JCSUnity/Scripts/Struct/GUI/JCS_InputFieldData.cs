/**
 * $File: JCS_InputFieldData.cs $
 * $Date: 2018-08-26 00:03:33 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System;

namespace JCSUnity
{
    /// <summary>
    /// Input field data, data we need to record it down.
    /// </summary>
    [Serializable]
    public class JCS_InputFieldData : JCS_IUIComponentData
    {
        public string text = "";
    }
}
