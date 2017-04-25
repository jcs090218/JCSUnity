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

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(JCS_3DGoStraightAction))]
    public class JCS_Bubble
        : JCS_WeatherParticle
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_Bubble) **")]

        [Tooltip("Do the effect?")]
        [SerializeField]
        private bool mDoAction = true;

        [Tooltip("")]
        [SerializeField] [Range(0.0f, 10.0f)]
        private float mShakeMargin = 2;

        [Tooltip("How fast it move?")]
        [SerializeField] [Range(0.1f, 1000.0f)]
        private float mShakeSpeed = 1.0f;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool DoAction { get { return this.mDoAction; } set { this.mDoAction = value; } }
        public float ShakeSpeed { get { return this.mShakeSpeed; } set { this.mShakeSpeed = value; } }
        public float ShakeMargin { get { return this.mShakeMargin; } set { this.mShakeMargin = value; } }

        //========================================
        //      Unity's function
        //------------------------------

        private void Update()
        {
            if (!mDoAction)
                return;

            Vector3 newPos = this.transform.position;
            newPos.x += JCS_Random.Range(-mShakeMargin, mShakeMargin) * ShakeSpeed * Time.deltaTime;
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
