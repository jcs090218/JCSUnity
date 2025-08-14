/**
 * $File: BF_Ground.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

public class BF_Ground : MonoBehaviour
{
    /* Variables */

    /* Setter & Getter */

    /* Functions */

    private void OnTriggerEnter(Collider other)
    {
        var bfLivObject = other.GetComponent<BF_LiveObject>();

        if (bfLivObject == null)
            return;

        // in some reason collider keep breaking...
        bfLivObject.velocityInfo.velY = 0;
    }
}
