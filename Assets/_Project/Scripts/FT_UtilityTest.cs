/**
 * $File: FT_UtilityTest.cs $
 * $Date: 2017-05-16 22:14:17 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using UnityEngine;

public class FT_UtilityTest : MonoBehaviour
{
    /* Variables */

    public List<SpriteRenderer> something = null;

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        something = JCSUnity.JCS_Util.RemoveEmptySlot<SpriteRenderer>(something);
    }
}
