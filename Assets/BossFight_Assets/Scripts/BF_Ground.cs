/**
 * $File: BF_Ground.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

public class BF_Ground 
    : MonoBehaviour 
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------

    private void OnTriggerEnter(Collider other)
    {
        BF_LiveObject bfLivObject = other.GetComponent<BF_LiveObject>();
        if (bfLivObject == null)
            return;

        // in some reason collider keep breaking...
        bfLivObject.VelocityInfo.VelY = 0;
    }

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
