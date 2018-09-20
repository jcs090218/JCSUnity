/**
 * $File: JCS_2DShootInAngle.cs $
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
    /// Do the shoot action in specific angle.
    /// </summary>
    [RequireComponent(typeof(JCS_ShootAction))]
    public class JCS_2DShootInAngle 
        : MonoBehaviour
        , JCS_Action
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_ShootAction mShootAction = null;

        [SerializeField] private float mMinDegree = 120;
        [SerializeField] private float mMaxDegree = 240;

        [Tooltip("Degree per bullet shoot.")]
        [SerializeField] private float mDegreePerShoot = 10;

        //private float mDelayTimer = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_ShootAction GetShootAction() { return this.mShootAction; }
        public float MinDegree { get { return this.mMinDegree; } }
        public float MaxDegree { get { return this.mMaxDegree; } }
        public float DegreePerShoot { get { return this.mDegreePerShoot; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            this.mShootAction = this.GetComponent<JCS_ShootAction>();
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
