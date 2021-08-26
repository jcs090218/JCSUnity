/**
 * $File: FT_EnumConver_Test.cs $
 * $Date: 2021-08-16 00:31:39 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright © 2021 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class FT_EnumConver_Test : MonoBehaviour
{
    /* Variables */

    /* Setter & Getter */

    /* Functions */

    private void Awake()
    {
        Debug.Log((JCS_2D8Direction)JCS_2D4Direction.BOTTOM);
        Debug.Log((JCS_2D8Direction)JCS_2D4Direction.TOP);
        Debug.Log((JCS_2D8Direction)JCS_2D4Direction.LEFT);
        Debug.Log((JCS_2D8Direction)JCS_2D4Direction.RIGHT);
    }
}
