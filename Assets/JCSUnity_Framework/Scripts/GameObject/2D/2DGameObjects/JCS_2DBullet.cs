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

        [Header("** Absoreb Effect **")]
        [SerializeField] private bool mAbsorbEffect = false;
        [SerializeField] private float mTimeToAbsorb = 0.25f;
        [SerializeField] private float mAbsorbTime = 0.25f;
        private float mAbsorbEffectTimer = 0;
        private float mRecordMoveSpeed = 0;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            mGoStraightAction = this.GetComponent<JCS_2DGoStraightAction>();

            // set all action to this move speed.
            mGoStraightAction.MoveSpeed = MoveSpeed;

            // record down the move speed
            mRecordMoveSpeed = MoveSpeed;
        }

        private void Update()
        {
            if (mAbsorbEffect)
            {
                mAbsorbEffectTimer += Time.deltaTime;

                if(mTimeToAbsorb < mAbsorbEffectTimer)
                {
                    // start the effect
                    mGoStraightAction.MoveSpeed +=  (0 - mGoStraightAction.MoveSpeed) / mAbsorbTime * Time.deltaTime;

                    if (JCS_UsefualFunctions.WithInRange(-0.8f, 0.8f, mGoStraightAction.MoveSpeed))
                    {
                        mGoStraightAction.MoveSpeed = mRecordMoveSpeed;

                        // end the effect.
                        mAbsorbEffect = false;
                    }
                }

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
