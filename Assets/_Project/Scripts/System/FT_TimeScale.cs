/**
 * $File: FT_EchoGamepadButton.cs $
 * $Date: 2017-10-07 05:10:36 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using UnityEngine;

/// <summary>
/// Print time scale value in update.
/// </summary>
public class FT_TimeScale : MonoBehaviour
{
    /* Variables */

    /* Setter/Getter */

    /* Functions */

    private void Update()
    {
        print(Time.timeScale);
    }
}
