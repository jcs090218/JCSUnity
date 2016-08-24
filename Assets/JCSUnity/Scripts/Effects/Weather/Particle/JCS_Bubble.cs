/**
 * $File: JCS_Bubble.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    [RequireComponent(typeof(JCS_2DGoStraightAction))]
    public class JCS_Bubble
        : JCS_WeatherParticle
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [SerializeField]
        [Range(0.0f, 10.0f)]
        private float mShake = 2;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------

        private void Update()
        {
            Vector3 newPos = this.transform.position;
            newPos.x += JCS_Utility.JCS_FloatRange(-mShake, mShake);
            this.transform.position = newPos;
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
}
