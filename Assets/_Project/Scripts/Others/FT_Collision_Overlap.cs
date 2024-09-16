/**
 * $File: FT_Collision_Overlap.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;

public class FT_Collision_Overlap : MonoBehaviour
{
    /* Variables */

    /* Setter & Getter */

    /* Functions */

    private void OnTriggerEnter(Collider other)
    {
        //BF_LiveObject bfLivObject = other.GetComponent<BF_LiveObject>();
        //if (bfLivObject == null)
        //    return;

        //bfLivObject.VelocityInfo.VelY = 0;
    }
}
