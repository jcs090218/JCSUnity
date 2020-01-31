/**
 * $File: JCS_IndieManager.cs $
 * $Date: 2017-05-16 20:02:48 $
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
    /// Game independent manager tag.
    /// </summary>
    public class JCS_IndieManager
        : JCS_Managers<JCS_IndieManager>
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private void Awake()
        {
            instance = this;
        }
    }
}
