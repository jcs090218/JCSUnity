/**
 * $File: JCS_2DBullet.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// For scripter and programmer,
    /// Override this if u want to implement this class
    /// </summary>
    [RequireComponent(typeof(JCS_2DGoStraightAction))]
    [RequireComponent(typeof(JCS_DestroyObjectWithTime))]
    [RequireComponent(typeof(JCS_HitCountEvent))]
    [RequireComponent(typeof(JCS_DestroyAnimEffect))]
    public class JCS_2DBullet
        : JCS_Bullet
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_2DGoStraightAction mGoStraightAction = null;

        [Header("** Initialize Variables  **")]
        [SerializeField] private JCS_Axis mAxis = JCS_Axis.AXIS_X;
        [Tooltip("bullet's velocity")]
        [SerializeField] private float mMoveSpeed = 10;

        [Header("** Runtime Variables (init variable will became runtime variables) **")]
        [SerializeField] private bool mKeepOverride = false;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_Axis Axis { get { return this.mAxis; } set { this.mAxis = value; } }
        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }
        public bool KeepOverride { get { return this.mKeepOverride; } set { this.mKeepOverride = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mGoStraightAction = this.GetComponent<JCS_2DGoStraightAction>();
            mAxis = mGoStraightAction.Axis;
            mMoveSpeed = mGoStraightAction.MoveSpeed;
        }

        private void Update()
        {
            if (mKeepOverride)
            {
                mAxis = mGoStraightAction.Axis;
                mMoveSpeed = mGoStraightAction.MoveSpeed;
            }
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
