/**
 * $File: FT_Utility.cs $
 * $Date: 2017-05-16 22:14:17 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;

public class FT_Utility : MonoBehaviour
{
    /* Variables */

    public List<SpriteRenderer> something = null;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        something = JCS_Array.RemoveEmpty(something);
    }
}
